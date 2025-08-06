using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class IndividualInstitutionalViewModel
    {
        public List<SelectListItem> InstitutionalCustomer { get; set; }

        public List<Customer> Customers { get; set; }
    }
}
