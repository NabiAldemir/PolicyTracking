using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace EntityLayer.Models
{
    public class UpdateCustomerModel
    {
        public List<AddressUpdateDTO>? Addresses { get; set; }
        public int Id { get; set; }
        public int CustomerTypeId { get; set; }
        public string TaxNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
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

    public class Addresses
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string District { get; set; }
        [MaxLength(100)]
        public string Neighbourhood { get; set; }
        [MaxLength(100)]
        public string? Street { get; set; }
        [MaxLength(20)]
        public string DoorNumber { get; set; }
        [MaxLength(50)]
        public string PostalCode { get; set; }
        public int CustomerId { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
    }
}
