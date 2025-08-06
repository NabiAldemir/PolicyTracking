using System;
using System.Collections.Generic;
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
    public class EfPolicyRepository : GenericRepository<Policy, Context>, IPolicyDal
    {
        private readonly IMapper mapper;

        public EfPolicyRepository(Context context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public List<Policy> GetListAllForPolicy()
        {
            return c.Policies
                .Include(x => x.Agency)
                .Include(x => x.PolicyType)
                .Include(x => x.Housing)
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
                .Include(x => x.AppUser)
                .ToList();
        }
        public List<Policy> GetUpcomingExpiringPolicies(int days)
        {
            var today = DateTime.Now;
            var upcomingDate = today.AddDays(days);
            return c.Policies
                .Include(x => x.Agency)
                .Include(x => x.PolicyType)
                .Include(x => x.Housing)
                .Include(x => x.Customer)
                .Include(x => x.Vehicle)
                .Include(x => x.AppUser)
                .Where(p => p.EndDate >= today && p.EndDate <= upcomingDate)
                .ToList();
        }
        //public List<Policy> CreatePolicy()
        //{
        //    using (var c = new Context())
        //    {
        //        // Todo: eğer işlemlerden birisi başarısız olursa uygulanmış veritabanı işlemleri geri alınmalı yoksa tutarsızlık olur.


        //        // 1) Ploiçe bilgilerinden poliçe kaydı 

        //        // 2) Eğer Taşıt Sigortası ise: araç bilgilerinden Araç KAydı insert edilecek 

        //    }
        //}
        public void CreatePolicy(CreatePolicyModel model, int userId)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                int? vehicleId = null;
                int? housingId = null;

                if (model.SelectedType == 1 || model.SelectedType == 2)
                {
                    var vehicle = mapper.Map<Vehicle>(model);
                    c.Vehicles.Add(vehicle);
                    c.SaveChanges();
                    vehicleId = vehicle.Id;
                }
                else if (model.SelectedType == 3 || model.SelectedType ==4 || model.SelectedType == 5)
                {
                    var housing = mapper.Map<Housing>(model);
                    c.Housings.Add(housing);
                    c.SaveChanges();
                    housingId = housing.Id;
                }
                var policy = mapper.Map<Policy>(model);

                policy.VehicleId = vehicleId;
                policy.HousingId = housingId;
                policy.UserId = userId;
                c.Policies.Add(policy);
                c.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdatePolicy(UpdatePolicyModel model, int userId)
        {
            using var transaction = c.Database.BeginTransaction();

            try
            {
                var policy = c.Policies.Include(p => p.Vehicle).Include(p => p.Housing).FirstOrDefault(p => p.Id == model.Id);
                if (policy == null)
                    throw new Exception("Poliçe bulunamadı.");

                if (model.selectType == 1 || model.selectType == 2)
                {
                    if (policy.Vehicle != null)
                    {
                        var vehicle = policy.Vehicle;
                        var updateVehicle = mapper.Map(model,vehicle);
                        c.Vehicles.Update(updateVehicle);
                        c.SaveChanges();
                    }
                }
                else if (model.selectType == 3 || model.selectType == 4 || model.selectType == 5)
                {

                    if (policy.Housing != null)
                    {
                        var housing = policy.Housing;
                        var updatedHousing = mapper.Map(model, housing);
                        c.Housings.Update(updatedHousing);
                        c.SaveChanges();
                    }
                }
                var updatedPolicy = mapper.Map(model, policy);
                c.Policies.Update(updatedPolicy);
                c.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public IQueryable<Policy> QueryablePolicy()
        {
            return c.Policies
                .Include(p => p.PolicyType)
                .Include(p => p.Customer)
                .Include(p => p.Agency)
                .AsQueryable();
        }

        //public IQueryable<Policy> QueryablePolicyForCustomer()
        //{
        //    return c.Policies.Where()
        //        .Include(p => p.PolicyType)
        //        .Include(p => p.Customer)
        //        .Include(p => p.Agency)
        //        .AsQueryable();
        //}
        public async Task<bool> AnyAsync(Expression<Func<Policy, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await c.Policies
                .AnyAsync(predicate, cancellationToken);
        }

        public void AddPdfUrl(string pdfUrl,int policyId)
        {
            var policy = c.Policies.FirstOrDefault(p => p.Id == policyId);
            policy.PdfUrl = pdfUrl;
            c.SaveChanges();
        }

        public List<Policy> GetListAllForPolicy(Expression<Func<Policy, bool>> filter)
        {
            return c.Policies
            .Where(filter)
            .Include(x => x.Agency)
            .Include(x => x.PolicyType)
            .Include(x => x.Housing)
            .Include(x => x.Customer)
            .Include(x => x.Vehicle)
            .Include(x => x.AppUser)
            .ToList();
        }
    }
}
