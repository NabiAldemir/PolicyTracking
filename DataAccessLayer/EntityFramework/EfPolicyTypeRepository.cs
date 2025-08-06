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

namespace DataAccessLayer.EntityFramework
{
    public class EfPolicyTypeRepository : GenericRepository<PolicyType, Context>, IPolicyTypeDal
    {
        public EfPolicyTypeRepository(Context context)
        : base(context)
        {
        }
    }
}
