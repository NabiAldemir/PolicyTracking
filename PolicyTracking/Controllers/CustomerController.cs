using System.Security.Claims;
using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using EntityLayer.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IMapper _mapper;
        IAddressService _addressService;
        private ICustomerService _customerService;
        private IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        ILocationService _locationService;

        public CustomerController(IMapper mapper, IAddressService addressService, ICustomerService customerService, IUserService userService, UserManager<AppUser> userManager, ILocationService locationService)
        {
            _mapper = mapper;
            _addressService = addressService;
            _customerService = customerService;
            _userService = userService;
            _userManager = userManager;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var customerValues = _customerService.GetList();
            ViewBag.customerCount = _customerService.GetList().Count();


            var Cities = await _locationService.GetProvincesAsync();
            var cityList = Cities.Select(x => new SelectListItem(x.sehir_adi, x.sehir_id.ToString())).ToList();
            var neighbourhoodList = new List<SelectListItem>();
            var districtList = new List<SelectListItem>();
            var addresses = _addressService.GetList();

            var viewModel = new CustomerViewModel
            {
                Customers = customerValues,
                Cities = cityList,
                Districts = districtList,
                Neighbourhoods = neighbourhoodList,
                Addresses = addresses,
            };
            return View(viewModel);
        }

        public IActionResult GetAddressInformation(int id)
        {
            var customerValues = _customerService.GetList().Where(x => x.Id == id).FirstOrDefault();
            var addressValues = _addressService.GetList().Where(x => x.CustomerId == id).ToList();


            var list = new CustomerWithAddress
            {
                CustomerId = customerValues.Id,
                CustomerName = customerValues.Name + " " + customerValues.Surname+customerValues.CompanyName,
                Addresses = addressValues.Select(x => new CustomerAdressDto
                {
                    Country = x.Country,
                    City = x.City,
                    District = x.District,
                    Neighbourhood = x.Neighbourhood,
                    Street = x.Street,
                    DoorNumber = x.DoorNumber,
                    PostalCode = x.PostalCode,
                    Title = x.Title
                }).ToList()
            };
            return Json(list);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CreateCustomerModel model)
        {
            var validator = new CustomerValidator(_customerService);
            var results = await validator.ValidateAsync(model);

            var username = User.Identity.Name;
            var userId = _userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            if (results.IsValid)
            {
                if (string.IsNullOrEmpty(model.PhoneNumber2))
                {
                    model.PhoneNumber2 = "-";
                }
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    model.IdentityNumber = "-";
                }

                if (model.Addresses is not null)
                {
                    var allCities = await _locationService.GetProvincesAsync();

                    foreach (var address in model.Addresses)
                    {
                        var city = allCities.FirstOrDefault(x => x.sehir_id.ToString() == address.City);
                        if (city != null)
                        {
                            var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                            var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == address.District);

                            if (district != null)
                            {
                                var neighborhoods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                                var neighborhood = neighborhoods.FirstOrDefault(x => x.mahalle_id.ToString() == address.Neighbourhood);

                                // ID'leri isimle değiştiriyoruz
                                address.City = city.sehir_adi;
                                address.District = district.ilce_adi;
                                address.Neighbourhood = neighborhood.mahalle_adi;
                            }
                        }
                    }
                }
                _customerService.CreateCustomer(model, userId);
                return Json(new { success = true, message = "Başarılı" });
            }
            else
            {
                var errorList = results.Errors
                    .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                    .ToList();

                return Json(new { success = false, errors = errorList });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(UpdateCustomerModel model)
        {
            var validator = new CustomerUpdateValidator(_customerService);
            var results = await validator.ValidateAsync(model);

            var username = User.Identity.Name;
            var userId = _userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            if (results.IsValid)
            {
                if(model.IdentityNumber is null)
                {
                    model.IdentityNumber = "-";
                }

                if (model.Addresses is not null)
                {
                    var allCities = await _locationService.GetProvincesAsync();

                    foreach (var address in model.Addresses)
                    {
                        var city = allCities.FirstOrDefault(x => x.sehir_id.ToString() == address.City);
                        if (city != null)
                        {
                            var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                            var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == address.District);

                            if (district != null)
                            {
                                var neighborhoods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                                var neighborhood = neighborhoods.FirstOrDefault(x => x.mahalle_id.ToString() == address.Neighbourhood);

                                // ID'leri isimle değiştiriyoruz
                                address.City = city.sehir_adi;
                                address.District = district.ilce_adi;
                                address.Neighbourhood = neighborhood.mahalle_adi;
                            }
                        }
                    }
                }
                _customerService.UpdateCustomer(model, userId);
                return Json(new { success = true, message = "Müşteri Bilgileri Başarıyla Güncellendi!" });
            }
            else
            {
                var errorList = results.Errors
                    .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                    .ToList();

                return Json(new { success = false, errors = errorList });
            }
        }
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                var value = _customerService.TGetById(id);
                _customerService.TDelete(value);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Başarısız" });
            }
        }

        public IActionResult FilterCustomers(int? customerTypeId,string? identityNumber,string? customer,string taxNumber)
        {
            var query = _customerService.QueryableCustomer();

            if (customerTypeId.HasValue)
            {
                query = query.Where(x => x.CustomerTypeId == customerTypeId);
            }
            if (!string.IsNullOrEmpty(identityNumber))
            {
                query = query.Where(x => x.IdentityNumber == identityNumber);
            }
            if (!string.IsNullOrEmpty(customer))
            {
                query = query.Where(x => (x.Name.ToLower() + " " + x.Surname.ToLower()+x.CompanyName.ToLower()).Replace(" ", "") == customer.ToLower().Replace(" ", ""));
            }
            if (!string.IsNullOrEmpty(taxNumber))
            {
                query = query.Where(x => x.TaxNumber == taxNumber);
            }

            var result = query.Select(p => new
            {
                customerTypeId = p.CustomerTypeId,
                identityNumber = p.IdentityNumber,
                name = p.Name+" "+p.Surname+p.CompanyName,
                taxNumber = p.TaxNumber,
                taxOffice = p.TaxOffice,
                phoneNumber1 = p.PhoneNumber1,
                phoneNumber2 = p.PhoneNumber2,
                email = p.Email,
                companyName = p.CompanyName,
                id = p.Id,
                status = p.Status,
                dateOfBirth = p.DateOfBirth,
                gender = p.Gender,
                addresses = p.Address.Select(a => new
                {
                    addressId = a.Id,
                    country = a.Country,
                    city = a.City,
                    district = a.District,
                    neighbourhood = a.Neighbourhood,
                    street = a.Street,
                    doorNumber = a.DoorNumber,
                    postalCode = a.PostalCode,
                    customerId = a.CustomerId,
                }).ToList()
            }).ToList();
            return Json(result);
        }

        [HttpGet("GetCustomerAddresses")]
        public IActionResult GetCustomerAddresses(int id)
        {
            try
            {
                var addresses = _addressService.GetList()
                    .Where(x => x.CustomerId == id)
                    .Select(a => new CustomerAdressDto
                    {
                        Id = a.Id,
                        Country = a.Country,
                        City = a.City,
                        District = a.District,
                        Neighbourhood = a.Neighbourhood,
                        Street = a.Street ?? string.Empty,
                        DoorNumber = a.DoorNumber,
                        PostalCode = a.PostalCode
                    })
                    .ToList();

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerInformation(int id, string addAddressTrigger)
        {
            try
            {
                // Müşteri bilgilerini al
                var customerInfo = _customerService.Get(x => x.Id == id).FirstOrDefault();
                var addressInfo = _addressService.Get(x => x.CustomerId == id);
                var cities = await _locationService.GetProvincesAsync();
                var cityList = cities.Select(x => new SelectListItem(x.sehir_adi, x.sehir_id.ToString())).ToList();
                var modelCity = _addressService.GetList()
                    .Where(x => x.CustomerId == id)
                    .Select(x => x.City)
                    .ToList();
                var model = _mapper.Map<UpdateCustomerViewModel>(customerInfo);

                model.Cities = cityList;
                model.Addresses = addressInfo;

                return PartialView("./Partials/CustomerUpdateModal", model);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressesById(int id)
        {
            var addresses = _addressService.GetList().Where(x => x.CustomerId == id).ToList();
            return Json(addresses);
        }

    }
}
