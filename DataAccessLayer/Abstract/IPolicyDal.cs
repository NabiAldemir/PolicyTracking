using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Models;

namespace DataAccessLayer.Abstract
{
    public interface IPolicyDal:IGenericDal<Policy>
    {
        List<Policy> GetListAllForPolicy();
        List<Policy> GetUpcomingExpiringPolicies(int days);
        public void CreatePolicy(CreatePolicyModel model,int userId);
        public void UpdatePolicy(UpdatePolicyModel model, int userId);
        public IQueryable<Policy> QueryablePolicy();
        public Task<bool> AnyAsync(Expression<Func<Policy, bool>> predicate, CancellationToken cancellationToken = default);
        public void AddPdfUrl(string pdfUrl, int policyId);
        public List<Policy> GetListAllForPolicy(Expression<Func<Policy, bool>> filter);
    }
}
