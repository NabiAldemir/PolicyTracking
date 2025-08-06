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
    public class EfAddressRepository : GenericRepository<Address, Context>, IAddressDal
    {
        public EfAddressRepository(Context context)
        : base(context)
        {
        }
        public List<Address> GetAllAddresses()
        {
            return c.Addresses
                .ToList();
        }
    }
}
