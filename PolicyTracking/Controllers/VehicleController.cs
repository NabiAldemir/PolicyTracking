using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.ViewModels;
using static BusinessLayer.ValidationRules.VehicleValidator;

namespace PolicyTracking.Controllers
{
    public class VehicleController : Controller
    {
        IVehicleService _vehicleService;
        ICustomerService _customerService;

        public VehicleController(IVehicleService vehicleService, ICustomerService customerService)
        {
            _vehicleService = vehicleService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            var customerValues = _customerService.GetList();
            var customerList = customerValues.Select(x=> new SelectListItem(x.Name+" "+x.Surname+x.CompanyName,x.Id.ToString())).ToList();
            var values = _vehicleService.GetVehicleWithCustomer();
            var viewmodel = new VehicleViewModel
            {
                CustomerSelect = customerList,
                Vehicles = values,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public IActionResult AddVehicle(Vehicle p) 
        {
            var Validator = new VehicleValidator();
            var results = Validator.Validate(p);
            if (results.IsValid)
            {
                _vehicleService.TAdd(p);
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
        public IActionResult DeleteVehicle(int id)
        {
            try
            {
                var value=_vehicleService.TGetById(id);
                _vehicleService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
        }
        [HttpPost]
        public IActionResult EditVehicle(VehicleUpdateDTO p)
        {
            var Validator = new VehicleUpdateValidator();
            var results = Validator.Validate(p);
            if (results.IsValid)
            {
                _vehicleService.TUpdate(p);
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
