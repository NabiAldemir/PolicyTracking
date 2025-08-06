using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class CompanyInformationController : Controller
    {
        ICompanyInformationService _companyInformationService;

        public CompanyInformationController(ICompanyInformationService companyInformationService)
        {
            _companyInformationService = companyInformationService;
        }

        [HttpGet]
        public IActionResult AddCompanyInformation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCompanyInformation(AddCompanyInformationViewModel p)
        {
            var validator = new CompanyInformationValidator();
            var results = validator.Validate(p);
            CompanyInformation c = new CompanyInformation();
            if (results.IsValid)
            {
                c.CompanyName = p.companyName;

                if (p.imageUrl != null)
                {
                    var extension = Path.GetExtension(p.imageUrl.FileName);
                    var newimagename = Guid.NewGuid() + extension;

                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CompanyImageFiles");

                    // 📂 Klasör yoksa oluştur
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var location = Path.Combine(uploadFolder, newimagename);

                    using (var stream = new FileStream(location, FileMode.Create))
                    {
                        p.imageUrl.CopyTo(stream);
                    }

                    c.CompanyLogoUrl = "/CompanyImageFiles/" + newimagename;
                }
                var lastValue = _companyInformationService.GetList().OrderByDescending(x => x.CreateDate).FirstOrDefault();
                if (lastValue != null)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", lastValue.CompanyLogoUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    _companyInformationService.TDelete(lastValue);
                }
                _companyInformationService.TAdd(c);
                return Json(new { success = true, message = "Başarılı" });
            }
            else
            {
                var errorlist = results.Errors
                  .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                  .ToList();
                return Json(new { success = false, errors = errorlist });
            }  
        }
    }
}
