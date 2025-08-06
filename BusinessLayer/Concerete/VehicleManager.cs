using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concerete
{
    public class VehicleManager : IVehicleService
    {
        IVehicleDal _vehicleDal;
        private readonly IMapper _mapper;

        public VehicleManager(IVehicleDal vehicleDal, IMapper mapper)
        {
            _vehicleDal = vehicleDal;
            _mapper = mapper;
        }

        public List<Vehicle> Get(Expression<Func<Vehicle, bool>> filter)
        {
            return _vehicleDal.Get(filter);
        }

        public List<Vehicle> GetList()
        {
            return _vehicleDal.GetListAll();
        }

        public List<Vehicle> GetListAll(Expression<Func<Vehicle, bool>> filter)
        {
            return _vehicleDal.GetListAll(filter);
        }

        public bool PlateExists(string plate)
        {
            return _vehicleDal.PlateExists(plate);
        }

        public void TAdd(Vehicle t)
        {
            _vehicleDal.Insert(t);
        }

        public void TDelete(Vehicle t)
        {
            _vehicleDal.Delete(t);
        }

        public Vehicle TGetById(int id)
        {
            return _vehicleDal.GetById(id);
        }

        public void TUpdate(VehicleUpdateDTO t)
        {
            var originalData = _vehicleDal.GetOne(x => x.Id == t.Id);
            if (originalData == null) throw new Exception("Data to update not exist!");
            var mapper = _mapper.Map(t,originalData);
            _vehicleDal.Update(originalData);
        }

        public void TUpdate(Vehicle t)
        {
            _vehicleDal.Update(t);
        }

        public List<Vehicle> GetVehicleWithCustomer()
        {
            return _vehicleDal.GetVehicleWithCustomer();
        }
    }
}
