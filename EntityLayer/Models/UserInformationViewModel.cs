using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Models
{
    public class UserInformationViewModel
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
        [Compare("NewPassword", ErrorMessage = "Yeni Şifreler Eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
