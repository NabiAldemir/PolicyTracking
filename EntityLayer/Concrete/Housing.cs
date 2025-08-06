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
    public class Housing:ILoggableEntity
    {
        [Key]
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
        [MaxLength(5)]
        public string ConstructionYear { get; set; }
        [MaxLength(50)]
        public string TotalArea { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BuildingCoverageAmount { get; set; } // Bina Teminatı

        [Column(TypeName = "decimal(18,2)")]
        public decimal FurnitureCoverageAmount { get; set; } // Eşya Teminatı
        public int CustomerId { get; set; } 

        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Policy> Policies { get; set; }
        public virtual Customer Customer { get; set; }

    }
    public class HousingUpdateDTO : ILoggableEntity
    {
        [Key]
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

        [MaxLength(10)]
        public string ConstructionYear { get; set; }
        [MaxLength(50)]
        public string TotalArea { get; set; }
        public int CustomerId { get; set; }
    }
}
