using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EntityLayer.Signatures;
using EntityLayer.Concrete;
using Newtonsoft.Json;
using System.Threading;
using DataAccessLayer.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Interceptors;

public sealed class LogInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context as Context;
        if (context != null)
        {
            var entries = context.ChangeTracker.Entries<ILoggableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            context.PendingLogs.Clear();
            foreach (var entry in entries)
            {
                if (entry.Entity is not Log)
                {
                    var originalData = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
                    var currentData = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
                    context.PendingLogs.Add((entry, entry.State, originalData));
                }
            }
        }
        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context as Context;
        if (context != null)
        {
            var logList = new List<Log>();
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

            foreach (var (entry, originalState, originalData) in context.PendingLogs)
            {
                var entityType = context.Model.FindEntityType(entry.Entity.GetType());
                var tableName = entityType?.GetTableName();
                var translatedTableName = TableNameMap.TryGetValue(tableName ?? "", out var trName)
                    ? trName
                    : tableName;

                int actionId = originalState switch
                {
                    EntityState.Added => 1,
                    EntityState.Modified => 2,
                    EntityState.Deleted => 3,
                    _ => 0
                };

                if (actionId == 1)
                {
                    var primitiveData = ExtractPrimitiveProperties(entry.Entity);
                    string changedData = JsonConvert.SerializeObject(primitiveData, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    logList.Add(new Log
                    {
                        ActionId = actionId,
                        TableName = translatedTableName,
                        ChangedData = changedData,
                        UserId = userId,
                        Status = true,
                        Description = "Ekleme İşlemi Gerçekleştirildi!",
                        CreateDate = DateTime.Now
                    });
                }
                else if (actionId == 2)
                {
                    var currentData = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]);

                    var differences = GetDifferences(originalData, currentData);

                    if (differences.changed.Count > 0)
                    {
                        string changedDataJson = JsonConvert.SerializeObject(differences.changed, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        });

                        string originalDataJson = JsonConvert.SerializeObject(differences.original, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        });

                        logList.Add(new Log
                        {
                            ActionId = actionId,
                            TableName = translatedTableName,
                            ChangedData = changedDataJson,
                            OriginalData = originalDataJson,
                            UserId = userId,
                            Status = true,
                            Description = "Güncelleme İşlemi Gerçekleştirildi!",
                            CreateDate = DateTime.Now
                        });
                    }
                }
                else if (actionId == 3)
                {
                    //var primitiveData = ExtractPrimitiveProperties(entry.Entity);
                    string originalDatajson = JsonConvert.SerializeObject(originalData, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    logList.Add(new Log
                    {
                        ActionId = actionId,
                        TableName = translatedTableName,
                        UserId = userId,
                        OriginalData = originalDatajson,
                        Status = true,
                        Description = "Silme İşlemi Gerçekleştirildi!",
                        CreateDate = DateTime.Now
                    });
                }
            }
            if (logList.Count > 0)
            {
                context.Set<Log>().AddRange(logList);
                context.SaveChanges();
            }

            context.PendingLogs.Clear();
        }
        return base.SavedChanges(eventData, result);
    }
    private object ExtractPrimitiveProperties(object entity)
    {
        var properties = entity.GetType()
            .GetProperties()
            .Where(p =>
            {
                var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                return type.IsPrimitive ||
                       type == typeof(string) ||
                       type == typeof(DateTime) ||
                       type == typeof(decimal) ||
                       type == typeof(Guid) ||
                       type.IsEnum;
            })
            .ToDictionary(p => p.Name, p => p.GetValue(entity));

        return properties;
    }

    private (Dictionary<string, object> original, Dictionary<string, object> changed)
    GetDifferences(Dictionary<string, object> originalData, Dictionary<string, object> currentData)
    {
        var originalDiff = new Dictionary<string, object>();
        var changedDiff = new Dictionary<string, object>();

        foreach (var key in originalData.Keys)
        {
            if (!currentData.ContainsKey(key)) continue;

            var originalValue = originalData[key];
            var currentValue = currentData[key];

            if ((originalValue == null && currentValue != null) ||
                (originalValue != null && !originalValue.Equals(currentValue)))
            {
                originalDiff[key] = originalValue;
                changedDiff[key] = currentValue;
            }
        }

        return (originalDiff, changedDiff);
    }

    private static readonly Dictionary<string, string> TableNameMap = new()
    {
        {"Actions","İşlemler"},
                {"Addresses","Adresler"},
                {"Agencies" , "Acenteler"},
                {"AspNetRoles","Roller" },
                {"AspNetUsers","Kullanıcılar" },
                {"CompanyInformations","Şirket Bilgileri" },
                {"Customers","Müşteriler" },
                {"Housings","Konutlar" },
                {"InstitutionalCustomers","Kurumsal Müşteriler" },
                {"Logs","Loglar" },
                {"Policies","Poliçeler" },
                {"PolicyTypes","Poliçe Türleri" },
                {"Vehicles","Araçlar" },
                {"AspNetUserRoles","Kullanıcı-Rol Map" }
    };

}


