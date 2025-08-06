using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Signatures;
using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Concrete
{
    public class AppUser:IdentityUser<int>,ILoggableEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }
        //[MaxLength(500)]
        //public string? Description { get; set; }
        //public bool Status { get; set; }
        //public DateTime CreateDate { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Policy> Policies { get; set; }
    }
    public class UserUpdateDTO:ILoggableEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string NewPassword { get; set; }
        [MaxLength(100)]
        public string OldPassword { get; set; }
        [MaxLength(100)]
        [Compare("NewPassword",ErrorMessage ="Yeni Şifreler Eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

    }
}
