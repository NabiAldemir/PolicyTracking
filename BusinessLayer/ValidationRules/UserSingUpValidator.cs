using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PolicyTracking.Models;

namespace BusinessLayer.ValidationRules
{
    public class UserSingUpValidator:AbstractValidator<UserSignUpViewModel>
    {
        private readonly UserManager<AppUser> _userManager;
        public UserSingUpValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x=>x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez!");
            RuleFor(x=>x.Surname).NotEmpty().WithMessage("Soyisim Alanı Boş Geçilemez!");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı Adı Alanı Boş Geçilemez!")
                .MustAsync(BeUniqueUserName)
                .When(x => !string.IsNullOrWhiteSpace(x.UserName))
                .WithMessage("Bu kullanıcı adı zaten kullanılıyor.");
            RuleFor(x=>x.Mail).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!");
            RuleFor(x=>x.Password).NotEmpty().WithMessage("Şifre Alanı Boş Geçilemez!");
            RuleFor(x => x.Mail).EmailAddress();
            RuleFor(x => x.ConfirmPassword)
           .Equal(x => x.Password).WithMessage("Şifreler Uyuşmuyor!");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("İsim Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.Surname).MaximumLength(100).WithMessage("Soyadı Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.UserName).MaximumLength(256).WithMessage("Kullanıcı Adı Alanı Maximum 256 Karakter İçermelidir!");
            RuleFor(x => x.Mail).MaximumLength(256).WithMessage("E-Mail Alanı Maximum 256 Karakter İçermelidir!");
        }


        private async Task<bool> BeUniqueUserName(string userName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }
    }
}
