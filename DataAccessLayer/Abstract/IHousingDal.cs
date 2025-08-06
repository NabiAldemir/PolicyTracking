using System.Linq.Expressions;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface IHousingDal:IGenericDal<Housing>
    {
        public List<Housing> GetAllList();
        public Task<Housing?> GetAsync(Expression<Func<Housing, bool>> filter);
    }
}
