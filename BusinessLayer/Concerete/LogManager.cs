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
    public class LogManager : ILogService
    {
        ILogDal _logDal;

        public LogManager(ILogDal logDal)
        {
            _logDal = logDal;
        }

        public List<Log> Get(Expression<Func<Log, bool>> filter)
        {
            return _logDal.Get(filter);
        }

        public List<Log> GetList()
        {
            return _logDal.GetListAll();
        }

        public List<Log> GetListAll(Expression<Func<Log, bool>> filter)
        {
            return _logDal.GetListAll(filter);
        }

        public List<Log> GetListAllLogs()
        {
            return _logDal.GetListAllLogs();
        }

        public List<string?> GetTableNames()
        {
            return _logDal.GetTableNames();
        }

        public IQueryable<Log> QueryableLog()
        {
            return _logDal.QueryableLog();
        }

        public void TAdd(Log t)
        {
             _logDal.Insert(t);
        }

        public void TDelete(Log t)
        {
             _logDal.Delete(t);
        }

        public Log TGetById(int id)
        {
             return _logDal.GetById(id);
        }

        public void TUpdate(Log t)
        {
            _logDal.Update(t);
        }
    }
}
