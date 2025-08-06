using EntityLayer.Concrete;
using EntityLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class PolicyViewModel
    {
        public List<Policy> Policies { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> AgnncyList { get; set; }
        public List<SelectListItem> PolicyTypeList { get; set; }
        public List<SelectListItem> ProvinceList { get; set; }
        public List<SelectListItem> DistrictList { get; set; }
        public List<SelectListItem> NeighborhoodList { get; set; }

        public CreatePolicyModel CreateModel { get; set; }
        public int policyTypeVehicleId;
        public int policyTypeHousingId;
        public string? policyNumber;
        public int UserId { get; set; }
    }
}
