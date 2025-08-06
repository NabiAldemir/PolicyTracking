using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Models
{
    public class AddressCreateViewModel
    {
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
    }
}
