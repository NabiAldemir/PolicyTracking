using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class AddressViewModel
    {
        public List<Address> Addresses { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public List<SelectListItem> Neighbourhoods { get; set; }
    }
}
