using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class HousingController : Controller
    {
        IHousingService _housingService;
        ICustomerService _customerService;
        ILocationService _locationService;

        public HousingController(IHousingService housingService, ICustomerService customerService, ILocationService locationService)
        {
            _housingService = housingService;
            _customerService = customerService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var values = _housingService.GetAllList();
            var customerValues = _customerService.GetList();
            var customerList = customerValues.Select(x => new SelectListItem(x.Name+" "+x.Surname + x.CompanyName, x.Id.ToString())).ToList();

            var Cities = await _locationService.GetProvincesAsync();
            var cityList = Cities.Select(x => new SelectListItem(x.sehir_adi, x.sehir_id.ToString())).ToList();
            var neighbourhoodList = new List<SelectListItem>();
            var districtList = new List<SelectListItem>();

            var viewmodel = new HousingViewModel
            {
                Customers = customerList,
                Housings = values,
                Cities = cityList,
                Districts = districtList,
                Neighbourhoods = neighbourhoodList,
            };
            return View(viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> AddHousing(Housing p) 
        {
            var Validator = new HousingValidator();
            var results =await Validator.ValidateAsync(p);
            if (results.IsValid)
            {
                var cities = await _locationService.GetProvincesAsync();
                var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == p.City);

                var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == p.District);

                var neighbourhoods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                var neighbourhood = neighbourhoods.FirstOrDefault(x => x.mahalle_id.ToString() == p.Neighbourhood);


                p.Neighbourhood = neighbourhood.mahalle_adi;
                p.City = city.sehir_adi;
                p.District = district.ilce_adi;
                _housingService.TAdd(p);
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
        public async Task<IActionResult> EditHousing(HousingUpdateDTO p)
        {
            var Validator = new HousingUpdateValidator();
            var results =await Validator.ValidateAsync(p);
            if (results.IsValid)
            {
                var cities = await _locationService.GetProvincesAsync();
                var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == p.City);

                var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == p.District);

                var neighbourhoods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                var neighbourhood = neighbourhoods.FirstOrDefault(x => x.mahalle_id.ToString() == p.Neighbourhood);


                p.Neighbourhood = neighbourhood.mahalle_adi;
                p.City = city.sehir_adi;
                p.District = district.ilce_adi;
                _housingService.TUpdate(p);
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
        public IActionResult DeleteHousing(int id)
        {
            try
            {
                var value = _housingService.TGetById(id);
                _housingService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }

        }
    }
}
