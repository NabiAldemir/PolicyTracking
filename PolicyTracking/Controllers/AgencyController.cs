using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using EntityLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.ViewModels;
using PolicyTrackingWebUI.ViewModels;

namespace PolicyTracking.Controllers
{
    public class AgencyController : Controller
    {
        ILocationService _locationService;
        IAgencyService _agencyService;
        IAddressService _addressService;

        public AgencyController(ILocationService locationService, IAgencyService agencyService, IAddressService addressService)
        {
            _locationService = locationService;
            _agencyService = agencyService;
            _addressService = addressService;
        }

        public async Task<IActionResult> Index()
        {
            var values = _agencyService.GetList();
            var addressValues = _addressService.GetAllAddresses();

            var Cities = await _locationService.GetProvincesAsync();
            var cityList = Cities.Select(x => new SelectListItem(x.sehir_adi, x.sehir_id.ToString())).ToList();
            var neighbourhoodList = new List<SelectListItem>();
            var districtList = new List<SelectListItem>();

            var viewModel = new AgencyAddressViewModel
            {
                Agencies = values,
                Addresses = addressValues,
                Cities = cityList,
                Districts = districtList,
                Neighbourhoods = neighbourhoodList
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddAgency(AddAgencyWithAddress model)
        {
            var Validator = new AgencyValidator();
            var results = Validator.Validate(model);

            if (results.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = "-";
            }
            if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                model.PhoneNumber = "-";
            }
                //Şehir ismini gönderdiğimiz id den alma
                var cities = await _locationService.GetProvincesAsync();
                var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == model.City);

                //İlçe ismini gönderdiğimiz id den alma
                var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == model.District);

                //Mahalle/Sokak ismini gönderdiğimiz id den alma
                var neighborhods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                var neighborhood = neighborhods.FirstOrDefault(x => x.mahalle_id.ToString() == model.Neighbourhood);
                model.City = city.sehir_adi;
                model.Neighbourhood = neighborhood.mahalle_adi;
                model.District = district.ilce_adi;
            
            
                _agencyService.AddAgencyWithAddress(model);
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
        public async Task<IActionResult> EditAgency(UpdateAgencyWithAddress model)
        {
            //var Validator = new AgencyUpdateValidator();
            //var results = Validator.Validate(p);

            //if (results.IsValid)
            //{
                //Şehir ismini gönderdiğimiz id den alma
                var cities = await _locationService.GetProvincesAsync();
                var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == model.City);

                //İlçe ismini gönderdiğimiz id den alma
                var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == model.District);

                //Mahalle/Sokak ismini gönderdiğimiz id den alma
                var neighborhods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                var neighborhood = neighborhods.FirstOrDefault(x => x.mahalle_id.ToString() == model.Neighbourhood);
                model.City = city.sehir_adi;
                model.Neighbourhood = neighborhood.mahalle_adi;
                model.District = district.ilce_adi;
                _agencyService.UpdateAgencyWithAddress(model);
                return Json(new { success = true, message = "Başarılı" });
            //}
            //else
            //{
            //    var errorlist = results.Errors
            //      .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
            //      .ToList();
            //    return Json(new { success = false, errors = errorlist });
            //}
        }
        [HttpPost]
        public IActionResult DeleteAgency(int id)
        {
            try
            {
                var value = _agencyService.TGetById(id);
                _agencyService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
        }

        [HttpGet]
        public IActionResult GetAddressInformation(int id)
        {
            var agencyValues = _agencyService.Get(x => x.Id == id).FirstOrDefault();
            var addressValues = _addressService.Get(x => x.AgencyId == id).FirstOrDefault();
            var list = new AgencyWithAddress
            {
                AgencyName = agencyValues.Name,
                AddressValues = new Address
                {
                    Country = addressValues.Country,
                    City = addressValues.City,
                    District = addressValues.District,
                    Neighbourhood = addressValues.Neighbourhood,
                    Street = addressValues.Street,
                    DoorNumber = addressValues.DoorNumber,
                    PostalCode = addressValues.PostalCode
                },
            };
            return Json(list);
        }
    }
}
