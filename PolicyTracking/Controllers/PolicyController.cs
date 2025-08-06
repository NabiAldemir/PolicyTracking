using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.Xml;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.LocationSelect;
using EntityLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class PolicyController : Controller
    {
        IPolicyService _policyService;
        IAgencyService _agencyService;
        IPolicyTypeService _policyTypeService;
        ICustomerService _customerService;
        IVehicleService _vehicleService;
        IUserService _userService;
        private readonly ILocationService _locationService;

        public PolicyController(IPolicyService policyService, IAgencyService agencyService, IPolicyTypeService policyTypeService, ICustomerService customerService, IVehicleService vehicleService, IUserService userService, ILocationService locationService)
        {
            _policyService = policyService;
            _agencyService = agencyService;
            _policyTypeService = policyTypeService;
            _customerService = customerService;
            _vehicleService = vehicleService;
            _userService = userService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index(string? policyNumber = null)
        {
            //Kullanıcını id değerini view e gönderiyoruz
            var username = User.Identity.Name;
            var userId = _userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            var customerValues = _customerService.GetList();
            var policyTypeValues = _policyTypeService.GetList();
            var agencyValues = _agencyService.GetList();
            var values = _policyService.GetListAllForPolicy();
            var agencyList = agencyValues.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            var policyTypeList = (from x in policyTypeValues
                                  select new SelectListItem
                                  {
                                      Text = x.Name,
                                      Value = x.Id.ToString(),
                                  }).ToList();
            var customerList = (from x in customerValues
                                select new SelectListItem
                                {
                                    Text = x.Name + " " + x.Surname + x.CompanyName,
                                    Value = x.Id.ToString(),
                                }).ToList();
            var policyTypeVehicleId = 1;
            var policyTypeHousingId = 2;

            //Lokasyon Verileri
            var provinces = await _locationService.GetProvincesAsync();
            var provinceList = provinces.Select(x => new SelectListItem
            {
                Text = x.sehir_adi,
                Value = x.sehir_id.ToString(),
            }).ToList();

            // İl ve ilçe seçilmediği için boş olarak gönder
            var districtList = new List<SelectListItem>();
            var neighborhoodList = new List<SelectListItem>();

            var viewModel = new PolicyViewModel
            {
                UserId = userId,
                policyNumber = policyNumber,
                policyTypeVehicleId = policyTypeVehicleId,
                policyTypeHousingId = policyTypeHousingId,
                Policies = values,
                AgnncyList = agencyList,
                Customers = customerList,
                PolicyTypeList = policyTypeList,
                ProvinceList = provinceList,
                DistrictList = districtList,
                NeighborhoodList = neighborhoodList
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddPolicy(CreatePolicyModel p)
        {
            var validator = new CreatePolicyValidator(_policyTypeService, _policyService);
            var results = await validator.ValidateAsync(p);
            if (results.IsValid)
            {
                p.PolicyTypeId = p.SelectedType;
                var username = User.Identity.Name;
                var userId = _userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
                if (p.SelectedType == 3 ||p.SelectedType == 4 || p.SelectedType == 5)
                {
                    //Şehir ismini gönderdiğimiz id den alma
                    var cities = await _locationService.GetProvincesAsync();
                    var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == p.City);

                    //İlçe ismini gönderdiğimiz id den alma
                    var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                    var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == p.District);

                    //Mahalle/Sokak ismini gönderdiğimiz id den alma
                    var neighborhods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                    var neighborhood = neighborhods.FirstOrDefault(x => x.mahalle_id.ToString() == p.Neighbourhood);
                    p.City = city.sehir_adi;
                    p.Neighbourhood = neighborhood.mahalle_adi;
                    p.District = district.ilce_adi;
                }
                p.Status = true;
                _policyService.CreatePolicy(p, userId);
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
        [HttpGet]
        public JsonResult CheckPlate(string plate)
        {
            var exists = _vehicleService.PlateExists(plate);
            return new JsonResult(exists);
        }
        [HttpPost]
        public IActionResult DeletePolicy(int id)
        {
            try
            {
                var values = _policyService.TGetById(id);
                _policyService.TDelete(values);
                return Json(new { success = true, message = "Başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditPolicy(UpdatePolicyModel p)
        {
            var validator = new UpdatePolicyValidator(_policyTypeService, _policyService);
            var results = await validator.ValidateAsync(p);
            if (results.IsValid)
            {
                p.PolicyTypeId = p.selectType;
                if (p.selectType == 3 || p.selectType == 4 || p.selectType == 5)
                {
                    //Şehir ismini gönderdiğimiz id den alma
                    var cities = await _locationService.GetProvincesAsync();
                    var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == p.City);

                    //İlçe ismini gönderdiğimiz id den alma
                    var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                    var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == p.District);

                    //Mahalle/Sokak ismini gönderdiğimiz id den alma
                    var neighborhods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                    var neighborhood = neighborhods.FirstOrDefault(x => x.mahalle_id.ToString() == p.Neighbourhood);
                    p.City = city.sehir_adi;
                    p.Neighbourhood = neighborhood.mahalle_adi;
                    p.District = district.ilce_adi;
                }
                p.Status = true;
                var username = User.Identity.Name;
                var userId = _userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
                _policyService.UpdatePolicy(p, userId);
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
        public async Task<IActionResult> FilterPolicies(int? policyTypeId, int? agencyId, int? customerId,
            string startDateFrom, string startDateTo, string endDateFrom, string endDateTo,
            string policyNumber, string plate, string cityId, string districtId, string neighbourhoodId,int userId)
        {
            var username = User.Identity.Name;
            var nameSurname = _userService.Get(x => x.UserName == username).Select(a => a.Name + " " + a.Surname).FirstOrDefault();

            var query = _policyService.QueryablePolicy();

            if (!string.IsNullOrEmpty(cityId))
            {
                var cities = await _locationService.GetProvincesAsync();
                var city = cities.FirstOrDefault(x => x.sehir_id.ToString() == cityId);

                if (city != null)
                {
                    query = query.Where(p => p.Housing.City == city.sehir_adi);

                    if (!string.IsNullOrEmpty(districtId))
                    {
                        var districts = await _locationService.GetDistrictsAsync(city.sehir_id);
                        var district = districts.FirstOrDefault(x => x.ilce_id.ToString() == districtId);

                        if (district != null)
                        {
                            query = query.Where(p => p.Housing.District == district.ilce_adi);

                            if (!string.IsNullOrEmpty(neighbourhoodId))
                            {
                                var neighborhoods = await _locationService.GetNeighborhoodsAsync(district.ilce_id);
                                var neighborhood = neighborhoods.FirstOrDefault(x => x.mahalle_id.ToString() == neighbourhoodId);

                                if (neighborhood != null)
                                {
                                    query = query.Where(p => p.Housing.Neighbourhood == neighborhood.mahalle_adi);
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(policyNumber))
            {
                query = query.Where(p => p.PolicyNumber == policyNumber);
            }
            if (!string.IsNullOrEmpty(plate))
            {
                query = query.Where(p => p.Vehicle.Plate.ToLower().Replace(" ", "") == plate.ToLower().Replace(" ", ""));
            }

            if (policyTypeId.HasValue)
            {
                query = query.Where(p => p.PolicyTypeId == policyTypeId);
            }
            if (agencyId.HasValue)
            {
                query = query.Where(p => p.AgencyId == agencyId);
            }
            if (customerId.HasValue)
            {
                query = query.Where(p => p.CustomerId == customerId);
            }
            if (!string.IsNullOrEmpty(startDateFrom))
            {
                if (DateTime.TryParseExact(startDateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate))
                {
                    query = query.Where(p => p.StartDate >= parsedStartDate);
                }
            }
            if (!string.IsNullOrEmpty(startDateTo))
            {
                if (DateTime.TryParseExact(startDateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate))
                {
                    query = query.Where(p => p.StartDate <= parsedStartDate);
                }
            }
            if (!string.IsNullOrEmpty(endDateFrom))
            {
                if (DateTime.TryParseExact(endDateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
                {
                    query = query.Where(p => p.EndDate >= parsedEndDate);
                }
            }
            if (!string.IsNullOrEmpty(endDateTo))
            {
                if (DateTime.TryParseExact(endDateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
                {
                    query = query.Where(p => p.EndDate <= parsedEndDate);
                }
            }

            var result = query.Select(p => new
            {
                policyNumber = p.PolicyNumber,
                agencyName = p.Agency.Name,
                grossPremium = p.GrossPremium,
                netPremium = p.NetPremium,
                commission = p.Commission,
                pdfUrl = p.PdfUrl,
                policyTypeName = p.PolicyType.Name,
                startDate = p.StartDate.ToString("dd-MM-yyyy"),
                endDate = p.EndDate.ToString("dd-MM-yyyy"),
                description = p.Description,
                customerName = p.Customer.Name + " " + p.Customer.Surname + p.Customer.CompanyName,
                vehicleId = p.VehicleId,
                plate = p.Vehicle != null ? p.Vehicle.Plate : null,
                brand = p.Vehicle != null ? p.Vehicle.Brand : null,
                model = p.Vehicle != null ? p.Vehicle.Model : null,
                docSerialNumber = p.Vehicle != null ? p.Vehicle.DocSerialNumber : null,
                productionYear = p.Vehicle != null ? p.Vehicle.ProductionYear : 0,
                country = p.Housing != null ? p.Housing.Country : null,
                city = p.Housing != null ? p.Housing.City : null,
                district = p.Housing != null ? p.Housing.District : null,
                neighbourhood = p.Housing != null ? p.Housing.Neighbourhood : null,
                street = p.Housing != null ? p.Housing.Street : null,
                postalCode = p.Housing != null ? p.Housing.PostalCode : null,
                totalArea = p.Housing != null ? p.Housing.TotalArea : null,
                doorNumber = p.Housing != null ? p.Housing.DoorNumber : null,
                constructionYear = p.Housing != null ? p.Housing.ConstructionYear : null,
                buildingCoverageAmount = p.Housing != null ? p.Housing.BuildingCoverageAmount : 0,
                furnitureCoverageAmount = p.Housing != null ? p.Housing.FurnitureCoverageAmount : 0,
                username = nameSurname,
                policyTypeId = p.PolicyTypeId,
                id = p.Id,
                customerId = p.CustomerId,
                agencyId = p.AgencyId,
            }).ToList();
            return Json(result);
        }

        public async Task<IActionResult> AddPdfUrl(IFormFile pdfFile,int policyId)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return Json(new { success = false, message = "Dosya geçersiz." });

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(pdfFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var pdfUrl = "/pdfs/" + uniqueFileName;
            _policyService.AddPdfUrl(pdfUrl,policyId);

            return Json(new { success = true });
        }



    }
}
