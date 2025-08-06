using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfLogRepository:GenericRepository<Log,Context> , ILogDal
    {
        public EfLogRepository(Context context)
        : base(context)
        {
        }

        public List<Log> GetListAllLogs()
        {
            return c.Logs
                .Include(x=>x.Actions)
                .Include(x => x.AppUser)
                .ToList();
        }
        public List<string?> GetTableNames()
        {
            var tableNameMap = new Dictionary<string, string>
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

            var excludedTables = new List<string>
            {
                "AspNetRoleClaims",
                "AspNetUserClaims",
                "AspNetUserLogins",
                "AspNetUserTokens"
            };


            return c.Model.GetEntityTypes()
          .Select(x => x.GetTableName())
          .Distinct()
          .Where(name => !excludedTables.Contains(name!))
          .Select(name => tableNameMap.TryGetValue(name!,out var TurkishName) ? TurkishName : name)
          .ToList();
        }
        public IQueryable<Log> QueryableLog() 
        {
            return c.Logs
                .Include(x => x.Actions)
                .Include(x => x.AppUser)
                .AsQueryable();
        }
    }
}
