using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IVehicleDal:IGenericDal<Vehicle>
    {
        public bool PlateExists(string plate);
        public List<Vehicle> GetVehicleWithCustomer();
    }
}
