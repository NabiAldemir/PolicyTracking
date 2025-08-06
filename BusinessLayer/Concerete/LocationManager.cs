using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.LocationSelect;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PolicyTracking.Models;

namespace BusinessLayer.Concerete
{
    public class LocationManager:ILocationService
    {
        private readonly string _cityJsonPath;
        private readonly string _districtJsonPath;
        private readonly string _neighbourhoodJsonPath;

        public LocationManager(IWebHostEnvironment env)
        {
            _cityJsonPath = Path.Combine(env.ContentRootPath, "wwwroot/citydata/sehirler.json");
            _districtJsonPath = Path.Combine(env.ContentRootPath, "wwwroot/citydata/ilceler.json");
            _neighbourhoodJsonPath = Path.Combine(env.ContentRootPath, "wwwroot/citydata/mahalleler");
        }

        public async Task<List<City>> GetProvincesAsync()
        {
            var json = await File.ReadAllTextAsync(_cityJsonPath);
            var cities = JsonConvert.DeserializeObject<List<City>>(json);
            return cities;
        }

        public async Task<List<District>> GetDistrictsAsync(int provinceId)
        {
            var json = await File.ReadAllTextAsync(_districtJsonPath);
            var districts = JsonConvert.DeserializeObject<List<District>>(json);
            return districts.Where(d => d.sehir_id == provinceId).ToList();
        }


        public async Task<List<Neighbourhood>> GetNeighborhoodsAsync(int districtId)
        {
            var allNeighbourhoods = new List<Neighbourhood>();

            
            var files = Directory.GetFiles(_neighbourhoodJsonPath, "mahalleler-*.json");

            foreach (var file in files)
            {
                var json = await File.ReadAllTextAsync(file);
                var list = JsonConvert.DeserializeObject<List<Neighbourhood>>(json);
                allNeighbourhoods.AddRange(list);
            }

            // districtId ile eşleşenleri filtrele
            return allNeighbourhoods
                .Where(n => n.ilce_id == districtId)
                .ToList();
        }


    }
}
