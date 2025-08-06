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
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concerete
{
    public class PolicyManager : IPolicyService
    {
        IPolicyDal _policyDal;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public PolicyManager(IPolicyDal policyDal, IMapper mapper, Context context)
        {
            _policyDal = policyDal;
            _mapper = mapper;
            _context = context;
        }

        public void CreatePolicy(CreatePolicyModel model, int userId)
        {
          _policyDal.CreatePolicy(model,userId);
        }

        public List<Policy> Get(Expression<Func<Policy, bool>> filter)
        {
           return _policyDal.Get(filter);
        }

        public List<Policy> GetList()
        {
            return _policyDal.GetListAll();
        }

        public List<Policy> GetListAll(Expression<Func<Policy, bool>> filter)
        {
           return _policyDal.GetListAll(filter);
        }

        public List<Policy> GetListAllForPolicy()
        {
            return _policyDal.GetListAllForPolicy();
        }

        public List<Policy> GetUpcomingExpiringPolicies(int days)
        {
            return _policyDal.GetUpcomingExpiringPolicies(days);
        }

        public void TAdd(Policy t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(Policy t)
        {
            _policyDal.Delete(t);
        }

        public Policy TGetById(int id)
        {
            return _policyDal.GetById(id);
        }

        public void TUpdate(Policy t)
        {
            _policyDal.Update(t);
        }

        public void UpdatePolicy(UpdatePolicyModel model, int userId)
        {
            _policyDal.UpdatePolicy(model,userId);
        }

        public IQueryable<Policy> QueryablePolicy()
        {
            return _policyDal.QueryablePolicy();
        }

        public Task<bool> AnyAsync(Expression<Func<Policy, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _policyDal.AnyAsync(predicate, cancellationToken);
        }

        public void AddPdfUrl(string pdfUrl, int policyId)
        {
            _policyDal.AddPdfUrl(pdfUrl, policyId);
        }

        public List<Policy> GetListAllForPolicy(Expression<Func<Policy, bool>> filter)
        {
            return _policyDal.GetListAllForPolicy(filter);
        }
    }
}
