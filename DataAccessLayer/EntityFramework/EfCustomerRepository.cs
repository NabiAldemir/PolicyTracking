using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using EntityLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfCustomerRepository : GenericRepository<Customer, Context>, ICustomerDal
    {
        private readonly IMapper _mapper;
        public EfCustomerRepository(Context context, IMapper mapper)
        : base(context)
        {
            _mapper = mapper;
        }
        public List<Customer> GetAllWithAddress()
        {
            return c.Customers.Include(a => a.Address).ToList();
        }
        public async Task<Customer?> GetAsync(Expression<Func<Customer, bool>> filter)
        {
            return await c.Customers.FirstOrDefaultAsync(filter);
        }

        public void CreateCustomer(CreateCustomerModel model, int userId)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                var customer = _mapper.Map<Customer>(model);
                customer.Status = true;
                c.Customers.Add(customer);
                c.SaveChanges();


                if (model.Addresses is not null)
                {
                    foreach (var item in model.Addresses)
                    {
                        var newAddress = new Address
                        {
                            CustomerId = customer.Id,
                            City = item.City,
                            Country = item.Country,
                            District = item.District,
                            Neighbourhood = item.Neighbourhood,
                            Street = item.Street,
                            DoorNumber = item.DoorNumber,
                            PostalCode = item.PostalCode,
                            Status = true,
                            Title = item.Title
                        };
                        c.Addresses.Add(newAddress);
                    }
                }

                c.SaveChanges();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public IQueryable<Customer> QueryableCustomer()
        {
            return c.Customers
                .Include(x => x.Address)
                .AsQueryable();
        }

        public void UpdateCustomer(UpdateCustomerModel model, int userId)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                var customer = c.Customers.Include(x => x.Address).FirstOrDefault(p => p.Id == model.Id);
                if (customer == null)
                    throw new Exception("Müşteri Bulunamadı");

                _mapper.Map(model, customer);

                c.Customers.Update(customer);

                var adressesIdInDb = c.Addresses.Where(x => x.CustomerId == model.Id).Select(x => x.Id).ToList();

               

                if (model.Addresses is not null)
                {
                    var adressesIdInModel = model.Addresses.Select(x => x.Id).ToList();


                    if (adressesIdInModel != adressesIdInDb)
                    {
                        foreach (var addressId in adressesIdInDb)
                        {
                            if (!adressesIdInModel.Contains(addressId))
                            {
                                var deletedAddress = c.Addresses.FirstOrDefault(x => x.Id == addressId);
                                c.Addresses.Remove(deletedAddress);
                            }
                        }
                    }
                    foreach (var item in model.Addresses)
                    {
                        if (item.Id != 0)
                        {
                            var originalAddress = c.Addresses.FirstOrDefault(x => x.Id == item.Id);
                            _mapper.Map(item, originalAddress);
                            c.Addresses.Update(originalAddress);
                        }
                        else
                        {
                            var newAddress = _mapper.Map<Address>(item);
                            newAddress.CustomerId = customer.Id;
                            newAddress.Status = true;
                            c.Addresses.Add(newAddress);
                        }

                    }
                }
                else
                {
                    foreach (var item in adressesIdInDb)
                    {
                        var deletedAddress = c.Addresses.FirstOrDefault(x => x.Id == item);
                        c.Addresses.Remove(deletedAddress);
                    }
                }

                c.SaveChanges();
                transaction.Commit();
            }

            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await c.Customers
                .AnyAsync(predicate, cancellationToken);
        }
    }
}
