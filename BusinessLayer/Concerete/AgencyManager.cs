using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace BusinessLayer.Concerete
{
    public class AgencyManager : IAgencyService
    {
        IAgencyDal _agencyDal;
        private readonly IMapper _mapper;

        public AgencyManager(IAgencyDal agencyDal, IMapper mapper)
        {
            _agencyDal = agencyDal;
            _mapper = mapper;
        }

        public List<Agency> Get(Expression<Func<Agency, bool>> filter)
        {
            return _agencyDal.Get(filter);
        }

        public List<Agency> GetList()
        {
            return _agencyDal.GetListAll();
        }

        public List<Agency> GetListAll(Expression<Func<Agency, bool>> filter)
        {
           return _agencyDal.GetListAll(filter);
        }

        public void TAdd(Agency t)
        {
            _agencyDal.Insert(t);
        }

        public void TDelete(Agency t)
        {
            _agencyDal.Delete(t);
        }

        public Agency TGetById(int id)
        {
            return _agencyDal.GetById(id);
        }

        public void TUpdate(Agency t)
        {
            _agencyDal.Update(t);
        }
        public void TUpdate(AgencyUpdateDTO t)
        {
            var originalData = _agencyDal.GetOne(x => x.Id == t.Id);
            if (originalData == null) throw new Exception("Data to update not found");
            var changedData = _mapper.Map(t, originalData);
            _agencyDal.Update(originalData);
        }

        public void AddAgencyWithAddress(AddAgencyWithAddress model)
        {
            _agencyDal.AddAgencyWithAddress(model);
        }

        public void UpdateAgencyWithAddress(UpdateAgencyWithAddress model)
        {
            _agencyDal.UpdateAgencyWithAddress(model);
        }
    }
}
