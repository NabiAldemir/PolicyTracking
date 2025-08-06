using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.LocationSelect;

namespace BusinessLayer.Abstract
{
    public interface ILocationService
    {
        public Task<List<City>> GetProvincesAsync();
        public Task<List<District>> GetDistrictsAsync(int provinceId);
        public Task<List<Neighbourhood>> GetNeighborhoodsAsync(int districtId);
    }
}
