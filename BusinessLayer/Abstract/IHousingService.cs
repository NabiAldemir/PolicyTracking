using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IHousingService:IGenericService<Housing>
    {
        public void TUpdate(HousingUpdateDTO t);
        public List<Housing> GetAllList();
        public Task<Housing?> GetAsync(Expression<Func<Housing, bool>> filter);
    }
}
