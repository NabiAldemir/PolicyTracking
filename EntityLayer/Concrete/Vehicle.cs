using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Signatures;

namespace EntityLayer.Concrete
{
    public class Vehicle:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Plate { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }
        [MaxLength(50)]
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        [MaxLength(20)]
        public string DocSerialNumber { get; set; }
        //[MaxLength(20)]
        //public string Color { get; set; }
        //public int Mileage { get; set; }
        public int CustomerId { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Policy> Policies { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class VehicleUpdateDTO : ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Plate { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }
        [MaxLength(50)]
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        //[MaxLength(20)]
        //public string Color { get; set; }
        //public int Mileage { get; set; }
        public int CustomerId { get; set; }
    }
}
