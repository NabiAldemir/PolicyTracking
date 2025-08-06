using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class AddressController : Controller
    {
        IAddressService _addressService;
        ICustomerService _customerService;
        ILocationService _locationService;

        public AddressController(IAddressService addressService, ICustomerService customerService, ILocationService locationService)
        {
            _addressService = addressService;
            _customerService = customerService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var values = _addressService.GetAllAddresses();
            var customerValues = _customerService.GetList();
            var customerList = customerValues.Select(x => new SelectListItem(x.Name + " " + x.Surname + x.CompanyName, x.Id.ToString())).ToList();

            var Cities = await _locationService.GetProvincesAsync();
            var cityList = Cities.Select(x => new SelectListItem(x.sehir_adi, x.sehir_id.ToString())).ToList();
            var neighbourhoodList = new List<SelectListItem>();
            var districtList = new List<SelectListItem>();
            var viewModel = new AddressViewModel
            {
                Customers = customerList,
                Addresses = values,
                Cities = cityList,
                Districts = districtList,
                Neighbourhoods = neighbourhoodList
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(Address p)
        {
            try
            {
                var Validator = new AddressValidator();
                var results = await Validator.ValidateAsync(p);
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

                    _addressService.TAdd(p);
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
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressUpdateDTO p)
        {
            var Validator = new AddressUpdateValidator();
            var results = await Validator.ValidateAsync(p);
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
                _addressService.TUpdate(p);
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
        public IActionResult DeleteAddress(int id)
        {
            try
            {
                var value = _addressService.TGetById(id);
                _addressService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }

        }
    }
}
