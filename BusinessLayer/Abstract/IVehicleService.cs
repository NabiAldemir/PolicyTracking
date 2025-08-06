using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IVehicleService:IGenericService<Vehicle>
    {
        public bool PlateExists(string plate);
        void TUpdate(VehicleUpdateDTO t);
        public List<Vehicle> GetVehicleWithCustomer();
    }
}
