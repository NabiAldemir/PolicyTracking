using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Models
{
    public class CreatePolicyModel
    {
        public int SelectedType { get; set; }  // "Vehicle" veya "Housing"
        public string PolicyNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public decimal GrossPremium { get; set; }  // Brüt Prim
        public decimal NetPremium { get; set; }    // Net Prim
        public decimal Commission { get; set; }
        public int PolicyTypeId { get; set; }
        public int CustomerId { get; set; }
        public int AgencyId { get; set; }
        public int VehicleId { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }

        // Vehicle 
        [MaxLength(20)]
        public string Plate { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }
        [MaxLength(50)]
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public string DocSerialNumber { get; set; }


        //housing

        
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string District { get; set; }
        [MaxLength(20)]
        public string DoorNumber { get; set; }
        [MaxLength(100)]
        public string Street { get; set; }
        [MaxLength(100)]
        public string Neighbourhood { get; set; }
        [MaxLength(50)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(5)]
        public string ConstructionYear { get; set; }
        [MaxLength(50)]
        public string TotalArea { get; set; }
        public decimal BuildingCoverageAmount { get; set; } // Bina Teminatı
        public decimal FurnitureCoverageAmount { get; set; } // Eşya Teminatı

    }
}
