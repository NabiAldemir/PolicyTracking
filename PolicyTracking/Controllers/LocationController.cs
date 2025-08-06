using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTracking.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<JsonResult> GetDistricts(int provinceId)
        {
            var districts = await _locationService.GetDistrictsAsync(provinceId);
            var result = districts.Select(x => new {
                id = x.ilce_id,
                name = x.ilce_adi
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetNeighborhoods(int districtId)
        {
            var neighborhoods = await _locationService.GetNeighborhoodsAsync(districtId);
            var result = neighborhoods.Select(x => new {
                id = x.mahalle_id,
                name = x.mahalle_adi
            }).ToList();

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetLocationIdToName(string city, string district, string neighbourhood)
        {
            try
            {
                var cities = await _locationService.GetProvincesAsync();
                var cityId = cities.FirstOrDefault(x => x.sehir_adi == city)?.sehir_id ?? 0;

                var districts = await _locationService.GetDistrictsAsync(cityId);
                var districtId = districts.FirstOrDefault(x => x.ilce_adi == district)?.ilce_id ?? 0;

                var neighbourhoods = await _locationService.GetNeighborhoodsAsync(districtId);
                var neighbourhoodId = neighbourhoods.FirstOrDefault(x => x.mahalle_adi == neighbourhood)?.mahalle_id ?? 0;

                return Json(new
                {
                    cityId,
                    districtId,
                    neighbourhoodId
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }

        }
    }
}
