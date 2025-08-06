using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PolicyTracking.Models;

namespace DataAccessLayer.EntityFramework
{
    public class EfUserRepository : GenericRepository<AppUser, Context>, IUserDal
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public EfUserRepository(Context context,UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        : base(context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task DeleteUser(int id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new Exception("User to delete not found");
            var originalData = ExtractPrimitiveProperties(user);

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loggedInUserId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

                var log = new Log
                {
                    ActionId = 3,
                    TableName = nameof(AppUser),
                    OriginalData = JsonConvert.SerializeObject(originalData),
                    UserId = loggedInUserId,
                    Status = true,
                    Description = "Silme İşlemi Gerçekleştirildi!",
                    CreateDate = DateTime.Now
                };

                c.Logs.Add(log);
                await c.SaveChangesAsync();
            }
        }
        public async Task AddUser(UserSignUpViewModel p)
        {
            try
            {
                AppUser user = new AppUser()
                {
                    Email = p.Mail,
                    UserName = p.UserName,
                    Name = p.Name,
                    Surname = p.Surname,
                    //Status = true,
                };

                var result = await _userManager.CreateAsync(user, p.Password);

                if (result.Succeeded)
                {
                    if(p.UserTypeId == 1)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else if (p.UserTypeId == 2)
                    {
                        var customer = c.Customers.FirstOrDefault(x => x.Id == p.CustomerId);
                        customer.UserId = user.Id;
                        c.Customers.Update(customer);
                        await _userManager.AddToRoleAsync(user, "Customer");
                        c.SaveChanges();
                    }

                    var httpContext = _httpContextAccessor.HttpContext;
                    var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var loggedInUserId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

                    var context = c;

                    var primitiveData = ExtractPrimitiveProperties(user);
                    string changedData = JsonConvert.SerializeObject(primitiveData);

                    var log = new Log
                    {
                        ActionId = 1, // Added
                        TableName = nameof(AppUser),
                        ChangedData = changedData,
                        UserId = loggedInUserId,
                        Status = true,
                        Description = loggedInUserId == 0
                            ? "Ekleme İşlemi Gerçekleştirildi!"
                            : "Kullanıcı Kayıt İşlemi Gerçekleştirildi!",
                        CreateDate = DateTime.Now
                    };

                    context.Logs.Add(log);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task UpdateUser(UserUpdateDTO t)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(t.Id.ToString());

                var oldData = ExtractPrimitiveProperties(user) as Dictionary<string, object>;

                user.UserName = t.UserName;
                user.Name = t.Name;
                user.Surname = t.Surname;
                user.Email = t.Email;

                var result = await _userManager.UpdateAsync(user);

                var httpContext = _httpContextAccessor.HttpContext;
                var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var loggedInUserId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

                var newData = ExtractPrimitiveProperties(user) as Dictionary<string, object>;

                var (originalDiff, changedDiff) = GetDifferences(oldData, newData);

                string originalDataJson = JsonConvert.SerializeObject(originalDiff);
                string changedDataJson = JsonConvert.SerializeObject(changedDiff);

                var log = new Log
                {
                    ActionId = 2,
                    TableName = nameof(AppUser),
                    OriginalData = originalDataJson,
                    ChangedData = changedDataJson,
                    UserId = loggedInUserId,
                    Status = true,
                    Description = "Güncelleme İşlemi Gerçekleştirildi!",
                    CreateDate = DateTime.Now
                };

                c.Logs.Add(log);
                await c.SaveChangesAsync();
            }
            
            catch (Exception)
            {
                throw;

            }

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

        public async Task<bool> AnyAsync(Expression<Func<AppUser, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await c.Users
                .AnyAsync(predicate, cancellationToken);
        }

        public async Task UpdateUserPassword(UserUpdateDTO model, Dictionary<string, object?> oldData, Dictionary<string, object?> newData)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userIdClaim = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

            var (originalDiff, changedDiff) = GetDifferences(oldData, newData);

            string originalDataJson = JsonConvert.SerializeObject(originalDiff);
            string changedDataJson = JsonConvert.SerializeObject(changedDiff);

            var log = new Log
            {
                ActionId = 2,
                TableName = nameof(AppUser),
                OriginalData = originalDataJson,
                ChangedData = changedDataJson,
                UserId = loggedInUserId,
                Status = true,
                Description = "Güncelleme İşlemi Gerçekleştirildi!",
                CreateDate = DateTime.Now
            };

            c.Logs.Add(log);
            await c.SaveChangesAsync();
        }
    }
}
