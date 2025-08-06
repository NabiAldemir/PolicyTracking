using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.EntityFramework
{
    public class EfActionsRepository : GenericRepository<Actions, Context>, IActionsDal
    {
        public EfActionsRepository(Context context)
        : base(context)
        {
        }
    }
}
