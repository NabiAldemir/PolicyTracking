using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace BusinessLayer.Concerete
{
    public class CustomerManager : ICustomerService
    {
        ICustomerDal _customerDal;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IMapper mapper)
        {
            _customerDal = customerDal;
            _mapper = mapper;
        }

        public List<Customer> GetList()
        {
            return _customerDal.GetListAll();
        }

        public List<Customer> GetListAll(Expression<Func<Customer, bool>> filter)
        {
            return _customerDal.GetListAll(filter);
        }

        public void TAdd(Customer t)
        {
            t.Status=true;
            _customerDal.Insert(t);
        }

        public void TDelete(Customer t)
        {
            _customerDal.Delete(t);
        }

        public Customer TGetById(int id)
        {
            return _customerDal.GetById(id);
        }

        public void TUpdate(Customer t)
        {
            _customerDal.Update(t);
        }
        public void TUpdate(CustomerUpdateDTO t)
        {
            var originalData = _customerDal.GetOne(x => x.Id == t.Id);
            if (originalData == null) throw new Exception("Data not found");
            _mapper.Map(t, originalData);
            _customerDal.Update(originalData);
        }

        public List<Customer> GetAllWithAddress()
        {
            return _customerDal.GetAllWithAddress();
        }

        public List<Customer> Get(Expression<Func<Customer, bool>> filter)
        {
            return _customerDal.Get(filter);
        }

        public async Task<Customer?> GetAsync(Expression<Func<Customer, bool>> filter)
        {
            return await _customerDal.GetAsync(filter);
        }

        public void CreateCustomer(CreateCustomerModel model, int userId)
        {
            _customerDal.CreateCustomer(model, userId);
        }

        public IQueryable<Customer> QueryableCustomer()
        {
            return _customerDal.QueryableCustomer();
        }

        public void UpdateCustomer(UpdateCustomerModel model, int userId)
        {
            _customerDal.UpdateCustomer(model, userId);
        }

        public async Task<bool> AnyAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _customerDal.AnyAsync(predicate,cancellationToken);
        }
    }
}
