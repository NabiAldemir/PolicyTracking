using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class CompanyInformation
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(200)]
        public string CompanyLogoUrl { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
