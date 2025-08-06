using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class PolicyTypeController : Controller
    {
        IPolicyTypeService _policyTypeService;

        public PolicyTypeController(IPolicyTypeService policyTypeService)
        {
            _policyTypeService = policyTypeService;
        }

        public IActionResult Index()
        {
            var values = _policyTypeService.GetList();
            var viewmodel = new PolicyTypeViewModel
            {
                PolicyTypes = values,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public IActionResult AddPolicyType(PolicyType p)
        {
            var Validator = new PolicyTypeValidator();
            var results = Validator.Validate(p);
            if (results.IsValid)
            {
                _policyTypeService.TAdd(p);
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

        [HttpPost]
        public IActionResult EditPolicyType(PolicyTypeUpdateDTO p)
        {
            var Validator = new PolicyTypeUpdateValidator();
            var results = Validator.Validate(p);
            if (results.IsValid)
            {
                _policyTypeService.TUpdate(p);
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
        [HttpPost]
        public IActionResult DeletePolicyType(int id)
        {
            try
            {
                var value = _policyTypeService.TGetById(id);
                _policyTypeService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
            
        }
    }
}
