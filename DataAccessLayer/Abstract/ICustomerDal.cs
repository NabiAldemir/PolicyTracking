using System.Linq.Expressions;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace DataAccessLayer.Abstract
{
    public interface ICustomerDal:IGenericDal<Customer>
    {
        public List<Customer> GetAllWithAddress();
        public Task<Customer?> GetAsync(Expression<Func<Customer, bool>> filter);
        public void CreateCustomer(CreateCustomerModel model, int userId);
        public IQueryable<Customer> QueryableCustomer();
        public void UpdateCustomer(UpdateCustomerModel model, int userId);
        Task<bool> AnyAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
