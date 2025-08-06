using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concerete
{
    public class RoleManager : IRoleService
    {
        IRoleDal _roleDal;

        public RoleManager(IRoleDal roleDal)
        {
            _roleDal = roleDal;
        }

        public List<AppRole> Get(Expression<Func<AppRole, bool>> filter)
        {
            return _roleDal.Get(filter);
        }

        public List<AppRole> GetList()
        {
            return _roleDal.GetListAll();
        }

        public List<AppRole> GetListAll(Expression<Func<AppRole, bool>> filter)
        {
            return _roleDal.GetListAll(filter);
        }

        public void TAdd(AppRole t)
        {
            _roleDal.Insert(t);
        }

        public void TDelete(AppRole t)
        {
            _roleDal.Delete(t);
        }

        public AppRole TGetById(int id)
        {
            return _roleDal.GetById(id);
        }

        public void TUpdate(AppRole t)
        {
            _roleDal.Update(t);
        }
    }
}
