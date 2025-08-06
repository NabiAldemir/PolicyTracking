using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class EfHousingRepository : GenericRepository<Housing, Context>, IHousingDal
    {
        public EfHousingRepository(Context context)
        : base(context)
        {
        }
        public List<Housing> GetAllList()
        {
           return c.Housings.Include(x => x.Customer).ToList();
        }
        public async Task<Housing?> GetAsync(Expression<Func<Housing, bool>> filter)
        {
            return await c.Housings.FirstOrDefaultAsync(filter);
        }
    }
}
