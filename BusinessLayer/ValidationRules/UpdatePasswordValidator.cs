using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Concerete;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.ValidationRules
{
    public class UpdatePasswordValidator : AbstractValidator<UserUpdateDTO>
    {
        private readonly UserManager<AppUser> _userManager;
        public UpdatePasswordValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            When(x => x.OldPassword is not null, () =>
            {
                RuleFor(x => x)
               .MustAsync(MatchOldPassword)
               .WithMessage("Eski şifre hatalı.")
               .OverridePropertyName("OldPassword");

            });

            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Eski Şifre Alanı Boş Geçilemez!")
                .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!");



            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni Şifre Alanı Boş Geçilemez!")
                .Equal(x => x.ConfirmPassword).WithMessage("Yeni şifreler eşleşmiyor.")
                .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!"); ;


            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Yeni Şifre Tekrar Alanı Boş Geçilemez!")
                .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!");
        }

        private async Task<bool> MatchOldPassword(UserUpdateDTO dto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null)
                return false;
            var PasswordTrue = await _userManager.CheckPasswordAsync(user, dto.OldPassword);
            if (!PasswordTrue)
            {
                return false;
            }
            return true;
        }
    }
}
