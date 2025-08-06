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
    public class ActionsManager : IActionsService
    {
        IActionsDal _actionsDal;

        public ActionsManager(IActionsDal actionsDal)
        {
            _actionsDal = actionsDal;
        }

        public List<Actions> Get(Expression<Func<Actions, bool>> filter)
        {
            return _actionsDal.Get(filter);
        }

        public List<Actions> GetList()
        {
            return _actionsDal.GetListAll();
        }

        public List<Actions> GetListAll(Expression<Func<Actions, bool>> filter)
        {
            return _actionsDal.GetListAll(filter);
        }

        public void TAdd(Actions t)
        {
            _actionsDal.Insert(t);
        }

        public void TDelete(Actions t)
        {
            _actionsDal.Delete(t);
        }

        public Actions TGetById(int id)
        {
            return _actionsDal.GetById(id);
        }

        public void TUpdate(Actions t)
        {
            _actionsDal.Update(t);
        }
    }
}
