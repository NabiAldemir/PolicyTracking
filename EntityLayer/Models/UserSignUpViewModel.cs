using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PolicyTracking.Models
{
    public class UserSignUpViewModel
    {
        [Required(ErrorMessage = "Lütfen Ad Giriniz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen Soyad Giriniz!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Lütfen Şifre Giriniz!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lütfen Şifre'yi Tekrar Giriniz!")]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Lütfen Mail Giriniz!")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Lütfen Kullanıcı Adını Giriniz!")]
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public int CustomerId { get; set; }
    }
}
