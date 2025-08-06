using System.ComponentModel.DataAnnotations;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class UpdateCustomerViewModel
    {
        public List<Address> Addresses { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int CustomerTypeId { get; set; }
        public string TaxNumber { get; set; }
        [MaxLength(50)]
        public string TaxOffice { get; set; }
        [MaxLength(20)]
        public string PhoneNumber1 { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber2 { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(150)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(11)]
        public string IdentityNumber { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }

    }
}
