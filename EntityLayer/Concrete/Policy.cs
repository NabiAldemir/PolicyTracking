using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;

namespace EntityLayer.Concrete
{
    public class Policy:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string PolicyNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPremium { get; set; }  // Brüt Prim

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPremium { get; set; }    // Net Prim

        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; }    // Komisyon
        public string? PdfUrl { get; set; }
        public int PolicyTypeId { get; set; }
        public int CustomerId { get; set; }
        public int AgencyId { get; set; }
        public int? VehicleId { get; set; }
        public int? HousingId { get; set; }
        public int UserId {  get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual PolicyType PolicyType { get; set; }
        public Vehicle Vehicle { get; set; }
        public Housing Housing { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
    public class PolicyUpdateDTO:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string PolicyNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public int Amount { get; set; }
        public int PolicyTypeId { get; set; }
        public int CustomerId { get; set; }
        public int AgencyId { get; set; }
        public int? VehicleId { get; set; }
        public int? HousingId { get; set; }
        public int UserId { get; set; }
    }
}
