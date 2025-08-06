using Microsoft.AspNetCore.Http;

namespace PolicyTracking.ViewModels
{
    public class AddCompanyInformationViewModel
    {
        public string companyName { get; set; }
        public IFormFile imageUrl { get; set; }
    }
}
