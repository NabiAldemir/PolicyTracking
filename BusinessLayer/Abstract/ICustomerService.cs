using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace BusinessLayer.Abstract
{
    public interface ICustomerService:IGenericService<Customer>
    {
        public void TUpdate(CustomerUpdateDTO t);
        public List<Customer> GetAllWithAddress();
        public Task<Customer?> GetAsync(Expression<Func<Customer, bool>> filter);
        public void CreateCustomer(CreateCustomerModel model, int userId);
        public IQueryable<Customer> QueryableCustomer();
        public void UpdateCustomer(UpdateCustomerModel model, int userId);
        Task<bool> AnyAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
