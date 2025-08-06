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
    public class AddressManager : IAddressService
    {
        IAddressDal _addressDal;
        private readonly IMapper _mapper;

        public AddressManager(IAddressDal addressDal, IMapper mapper)
        {
            _addressDal = addressDal;
            _mapper = mapper;
        }

        public List<Address> Get(Expression<Func<Address, bool>> filter)
        {
           return _addressDal.Get(filter);
        }

        public List<Address> GetAllAddresses()
        {
           return _addressDal.GetAllAddresses();
        }

        public List<Address> GetList()
        {
            return _addressDal.GetListAll();
        }

        public List<Address> GetListAll(Expression<Func<Address, bool>> filter)
        {
            return _addressDal.GetListAll(filter);
        }

        public void TAdd(Address t)
        {
            _addressDal.Insert(t);
        }

        public void TDelete(Address t)
        {
            _addressDal.Delete(t);
        }

        public Address TGetById(int id)
        {
            return _addressDal.GetById(id);
        }

        public void TUpdate(Address t)
        {
            _addressDal.Update(t);
        }

        public void TUpdate(AddressUpdateDTO t)
        {
            var originalData = _addressDal.GetOne(x=>x.Id == t.Id);
            if (originalData == null) throw new Exception("Data to update not found");
            _mapper.Map(t, originalData);
            _addressDal.Update(originalData);
        }
    }
}
