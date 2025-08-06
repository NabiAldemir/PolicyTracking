using System.Globalization;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using PolicyTracking.ViewModels;

namespace PolicyTracking.ViewComponents.CompanyInformation
{
    public class AddCompanyInformation:ViewComponent
    {
        ICompanyInformationService _companyInformationService;

        public AddCompanyInformation(ICompanyInformationService companyInformationService)
        {
            _companyInformationService = companyInformationService;
        }

        public IViewComponentResult Invoke()
        {
            var companyInfo = _companyInformationService.GetList().OrderByDescending(x=>x.CreateDate).FirstOrDefault();

            if (companyInfo != null)
            {
                ViewBag.CompanyName = companyInfo.CompanyName;
                ViewBag.CompanyLogoUrl = companyInfo.CompanyLogoUrl;
            }
            else
            {
                ViewBag.CompanyName = "Firma Adı";
                ViewBag.CompanyLogoUrl = "Logo";
            }

            return View();
        }


    }
}
