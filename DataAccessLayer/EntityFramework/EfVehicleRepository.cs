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
    public class EfVehicleRepository:GenericRepository<Vehicle, Context>, IVehicleDal
    {
        public EfVehicleRepository(Context context)
         : base(context)
        {
        }

        public bool PlateExists(string plate)
        {
            return c.Vehicles.Any(x => x.Plate == plate);
        }

        public List<Vehicle> GetVehicleWithCustomer() 
        { 
            return c.Vehicles.Include(x => x.Customer).ToList();
        }
    }
}
