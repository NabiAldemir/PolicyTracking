using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface ILogDal:IGenericDal<Log>
    {
        public List<Log> GetListAllLogs();
        public List<string?> GetTableNames();
        public IQueryable<Log> QueryableLog();
    }
}
