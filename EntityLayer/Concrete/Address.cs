using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;

namespace EntityLayer.Concrete
{
    public class Address:ILoggableEntity
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
        public int? CustomerId { get; set; }
        public int? AgencyId { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Agency Agency { get; set; }
    }
    public class AddressUpdateDTO:ILoggableEntity
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

        public int CustomerId { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
    }
}
