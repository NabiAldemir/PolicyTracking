using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.ValidationRules
{
    public class UserValidator : AbstractValidator<UserUpdateDTO>
    {
        IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public UserValidator()
        {


            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez!");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyisim Alanı Boş Geçilemez!");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Alanı Boş Geçilemez!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Şifre Alanı Boş Geçilemez!")
                .Equal(x=>x.ConfirmPassword).WithMessage("Girdiğiniz Şifreler Uyuşmuyor!");


        }

    }
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        private readonly UserManager<AppUser> _userManager;
        IUserService _userService;
        public UserUpdateValidator(UserManager<AppUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;

            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez!");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyisim Alanı Boş Geçilemez!");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Alanı Boş Geçilemez!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!");
            RuleFor(x => x.Email).EmailAddress();

            //When(x => x.OldPassword is not null, () =>
            //{
            //    RuleFor(x => x)
            //   .MustAsync(MatchOldPassword)
            //   .WithMessage("Eski şifre hatalı.")
            //   .OverridePropertyName("OldPassword");

            //});

            When(x => x.UserName is not null, () =>
            {
                RuleFor(x => x)
                .MustAsync(BeUniqueUsername)
                .WithMessage("Bu kullanıcı adı zaten alınmış.")
                .OverridePropertyName("UserName");
            });

            //RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Eski Şifre Alanı Boş Geçilemez!")
            //    .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!");



            //RuleFor(x => x.NewPassword)
            //    .NotEmpty().WithMessage("Yeni Şifre Alanı Boş Geçilemez!")
            //    .Equal(x => x.ConfirmPassword).WithMessage("Yeni şifreler eşleşmiyor.")
            //    .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!"); ;


            //RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Yeni Şifre Tekrar Alanı Boş Geçilemez!")
            //    .MinimumLength(6).WithMessage("Şifre Alanı Minimum 6 Karakter Olmalı!");
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

        private async Task<bool> BeUniqueUsername(UserUpdateDTO dto, CancellationToken cancellationToken)
        {
            return !await _userService.AnyAsync(x =>
          x.UserName == dto.UserName &&
          x.Id != dto.Id,
          cancellationToken);
        }
    }
}
