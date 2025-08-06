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
    public class PolicyTypeManager : IPolicyTypeService
    {
        IPolicyTypeDal _policyTypeDal;
        private readonly IMapper _mapper;

        public PolicyTypeManager(IPolicyTypeDal policyTypeDal, IMapper mapper)
        {
            _policyTypeDal = policyTypeDal;
            _mapper = mapper;
        }

        public List<PolicyType> Get(Expression<Func<PolicyType, bool>> filter)
        {
            return _policyTypeDal.Get(filter);
        }

        public List<PolicyType> GetList()
        {
            return _policyTypeDal.GetListAll();
        }

        public List<PolicyType> GetListAll(Expression<Func<PolicyType, bool>> filter)
        {
            return _policyTypeDal.GetListAll(filter);
        }

        public void TAdd(PolicyType t)
        {
            _policyTypeDal.Insert(t);
        }

        public void TDelete(PolicyType t)
        {
            try
            {
               _policyTypeDal.Delete(t);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PolicyType TGetById(int id)
        {
           return _policyTypeDal.GetById(id);
        }

        public void TUpdate(PolicyTypeUpdateDTO t)
        {
            var originalData = _policyTypeDal.GetOne(x => x.Id == t.Id);
            if (originalData == null) throw new Exception("Data to update not exist!");

            var mapper = _mapper.Map(t, originalData);

            _policyTypeDal.Update(originalData);
        }

        public void TUpdate(PolicyType t)
        {
            _policyTypeDal.Update(t);
        }
    }
}
