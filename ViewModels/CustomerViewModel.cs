using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<Address> Addresses { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public List<SelectListItem> Neighbourhoods { get; set; }
    }
}
