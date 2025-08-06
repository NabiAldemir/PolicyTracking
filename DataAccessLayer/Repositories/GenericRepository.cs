using System.Linq.Expressions;
using System.Security.Claims;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.LogHelper;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T, TC> : IGenericDal<T>
        where T : class
      where TC : IdentityDbContext<AppUser, AppRole, int>
    {
        protected TC c { get; set; }

        public GenericRepository(TC context)
        {
            c = context;
        }
        public void Delete(T t)
        {
            c.Remove(t);
            c.SaveChanges();
        }
        public T GetById(int id)
        {
            return c.Set<T>().Find(id);
        }
        public T? GetOne(Expression<Func<T, bool>> filter)
        {
            return c.Set<T>().FirstOrDefault(filter);
        }
        public List<T> Get(Expression<Func<T, bool>> filter)
        {
            return c.Set<T>().Where(filter).ToList();
        }

        public List<T> GetListAll(Expression<Func<T, bool>> filter)
        {
            return c.Set<T>().Where(filter).ToList();
        }

        public List<T> GetListAll()
        {
            return c.Set<T>().ToList();
        }

        public void Insert(T t)
        {
            var statusProp = typeof(T).GetProperty("Status");
            if (statusProp != null && statusProp.CanWrite)
            {
                statusProp.SetValue(t, true);
            }
            c.Set<T>().Add(t);
            c.SaveChanges();
        }


        public void Update(T t)
        {
            var statusProp = typeof(T).GetProperty("Status");
            if (statusProp != null && statusProp.CanWrite)
            {
                statusProp.SetValue(t, true);
            }
            c.Entry(t).State = EntityState.Modified;
            c.Entry(t).Property("CreateDate").IsModified = false;
            c.SaveChanges();
        }
    }
}
