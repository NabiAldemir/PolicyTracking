using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class VehicleViewModel
    {
        public List<Vehicle> Vehicles { get; set; }
        public List<SelectListItem> CustomerSelect { get; set; }
    }
}
