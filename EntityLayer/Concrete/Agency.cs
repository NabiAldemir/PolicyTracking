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
    public class Agency:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string LicenseNumber { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
        public virtual Address Address { get; set; }
    }
    public class AgencyUpdateDTO:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string LicenseNumber { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }
    }
}
