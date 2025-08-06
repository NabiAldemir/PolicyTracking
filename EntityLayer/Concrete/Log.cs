using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string TableName { get; set; }
        public int ActionId { get; set; }
        [MaxLength]
        public string? OriginalData { get; set; }
        [MaxLength]
        public string? ChangedData { get; set; }
        public int UserId { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Actions Actions { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
