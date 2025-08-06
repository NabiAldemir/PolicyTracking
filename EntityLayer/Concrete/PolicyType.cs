using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;

namespace EntityLayer.Concrete
{
    public class PolicyType:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
    }


    public class PolicyTypeUpdateDTO : ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
