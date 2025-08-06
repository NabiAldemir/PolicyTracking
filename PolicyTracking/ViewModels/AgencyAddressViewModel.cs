using EntityLayer.Concrete;
using EntityLayer.LocationSelect;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTrackingWebUI.ViewModels
{
    public class AgencyAddressViewModel
    {
        public List<Agency> Agencies { get; set; }

        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public List<SelectListItem> Neighbourhoods { get; set; }

        public List<Address> Addresses { get; set; }

    }
}
