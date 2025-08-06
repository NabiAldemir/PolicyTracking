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
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BusinessLayer.Concerete
{
    public class HousingManager : IHousingService
    {
        IHousingDal _housingDal;
        private readonly IMapper _mapper;

        public HousingManager(IHousingDal housingDal, IMapper mapper)
        {
            _housingDal = housingDal;
            _mapper = mapper;
        }

        public List<Housing> Get(Expression<Func<Housing, bool>> filter)
        {
            return  _housingDal.Get(filter);
        }

        public List<Housing> GetList()
        {
            return _housingDal.GetListAll();
        }

        public List<Housing> GetListAll(Expression<Func<Housing, bool>> filter)
        {
            return _housingDal.GetListAll(filter);
        }

        public void TAdd(Housing t)
        {
            _housingDal.Insert(t);
        }

        public void TDelete(Housing t)
        {
            _housingDal.Delete(t);
        }

        public Housing TGetById(int id)
        {
            return _housingDal.GetById(id);
        }

        public void TUpdate(Housing t)
        {
            _housingDal.Update(t);
        }
        public void TUpdate(HousingUpdateDTO t)
        {
            var originalData = _housingDal.GetOne(x=> x.Id == t.Id);
            if (originalData == null) throw new Exception("Data to update not found");
            var changedData = _mapper.Map(t,originalData);
            _housingDal.Update(originalData);
        }

        public List<Housing> GetAllList()
        {
            return _housingDal.GetAllList();
        }

        public async Task<Housing?> GetAsync(Expression<Func<Housing, bool>> filter)
        {
            return await _housingDal.GetAsync(filter);
        }
    }
}
