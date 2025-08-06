using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EntityLayer.Concrete
{
    public class Customer:ILoggableEntity
    {
        [Key]
        [ForeignKey("AppUser")]
        public int Id { get; set; }
        [MaxLength(11)]
        public string? IdentityNumber { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Surname { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(10)]
        public string? Gender { get; set; }
        [MaxLength(10)]
        public string TaxNumber { get; set; }
        [MaxLength(50)]
        public string TaxOffice { get; set; }
        [MaxLength(20)]
        public string PhoneNumber1 { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber2 { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(100)]
        public string? CompanyName { get; set; }
        public int CustomerTypeId { get; set; }
        public int? UserId { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection< Address>? Address { get; set; }
        public virtual ICollection<Policy>? Policies { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; }
        public virtual ICollection<Housing>? Housings { get; set; }
    }
    public class CustomerUpdateDTO:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(11)]
        public string IdentityNumber { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        public int TaxNumber { get; set; }
        [MaxLength(50)]
        public string TaxOffice { get; set; }
        [MaxLength(20)]
        public string PhoneNumber1 { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber2 { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string? CompanyName { get; set; }
        public int CustomerTypeId { get; set; }
    }
}
