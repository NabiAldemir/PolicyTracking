using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concerete
{
    public class CompanyInformationManager : ICompanyInformationService
    {
        ICompanyInformationDal _companyInformationDal;

        public CompanyInformationManager(ICompanyInformationDal companyInformationDal)
        {
            _companyInformationDal = companyInformationDal;
        }

        public List<CompanyInformation> Get(Expression<Func<CompanyInformation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<CompanyInformation> GetList()
        {
            return _companyInformationDal.GetListAll();
        }

        public List<CompanyInformation> GetListAll(Expression<Func<CompanyInformation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void TAdd(CompanyInformation t)
        {
             _companyInformationDal.Insert(t);
        }

        public void TDelete(CompanyInformation t)
        {
             _companyInformationDal.Delete(t);
        }

        public CompanyInformation TGetById(int id)
        {
             return _companyInformationDal.GetById(id);
        }

        public void TUpdate(CompanyInformation t)
        {
             _companyInformationDal.Update(t);
        }
    }
}
