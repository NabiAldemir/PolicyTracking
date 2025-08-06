using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using EntityLayer.Models;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AgencyValidator : AbstractValidator<AddAgencyWithAddress>
    {
        public AgencyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez")
                .MaximumLength(50).WithMessage("İsim Alanı Maximum 50 Karakter İçermelidir!");

            RuleFor(x => x.Name)
                .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$")
                .WithMessage("Acente Adı Alanı Yalnızca Harf ve Boşluk İçerebilir!");

            RuleFor(x => x.LicenseNumber).NotEmpty().WithMessage("Lisans Numarası Alanı Boş Geçilemez")
                .Matches(@"^\d+$").WithMessage("Lisans Numarası Sadece Sayılardan Oluşmalıdır!");

            RuleFor(x => x.LicenseNumber)
                .Matches(@"[0-9]+$").WithMessage("Lisans Numarası Alanı Sadece Sayı İçerebilir!");


            RuleFor(x => x.Email).MaximumLength(50).WithMessage("E-Mail Alanı Maximum 50 Karakter İçermelidir!");

            When(x => x.Email is not null, () =>
            {
                RuleFor(x => x.Email).EmailAddress().WithMessage("Lütfen Uygun Bir Mail Adresi Giriniz!");
            });


            RuleFor(x => x.PhoneNumber)
                .Length(11).WithMessage("Telefon Numarası Alanı Maximum 11 Karakter İçermelidir!");

            When(x => x.PhoneNumber is not null, () =>
            {
                RuleFor(x => x.PhoneNumber).Matches(@"[0-9]+$").WithMessage("Telefon Numarası Alanı Sadece Sayı İçerebilir!");
            });

                RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Ülke Alanı Maximum 100 Karakter İçermelidir!");
                RuleFor(x => x.City).NotEmpty().WithMessage("Şehir Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Şehir Alanı Maximum 100 Karakter İçermelidir!");
                RuleFor(x => x.Neighbourhood).NotEmpty().WithMessage("Mahalle/Sokak Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Mahalle/Sokak Alanı Maximum 100 Karakter İçermelidir!");
                RuleFor(x => x.District).NotEmpty().WithMessage("İlçe Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("İlçe Alanı Maximum 100 Karakter İçermelidir!");
                RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Posta Kodu Alanı Boş Geçilemez!")
                    .MaximumLength(5).WithMessage("Posta Kodu Alanı Maximum 5 Karakter İçermelidir!");
                RuleFor(x => x.PostalCode).Must(BeLogicalPostalCode).WithMessage("Doğru Posta Kodunu Giriniz!")
                    .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));
                RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                    .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!"); ;
                RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");
        }

        private static bool BeLogicalPostalCode(string postalCode)
        {
            if (!int.TryParse(postalCode, out int parsedPostalCode))
            {
                return false;
            }

            if (parsedPostalCode <= 1000 || parsedPostalCode >= 81999)
            {
                return false;
            }

            return true;
        }
    }
    public class AgencyUpdateValidator : AbstractValidator<AgencyUpdateDTO>
    {
        public AgencyUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez")
                 .MaximumLength(50).WithMessage("İsim Alanı Maximum 50 Karakter İçermelidir!");

            RuleFor(x => x.Name)
                .Matches(@"^[A-Za-z]+$").WithMessage("Acente İsim Alanı Yalnızca Harf İçerebilir!");

            RuleFor(x => x.LicenseNumber).NotEmpty().WithMessage("Lisans Numarası Alanı Boş Geçilemez")
                .MaximumLength(20).WithMessage("Lisans Numarası Alanı Maximum 20 Karakter İçermelidir!");

            RuleFor(x => x.LicenseNumber)
                .Matches(@"[0-9]+$").WithMessage("Lisans Numarası Alanı Sadece Sayı İçerebilir!");


            RuleFor(x => x.Email).MaximumLength(50).WithMessage("E-Mail Alanı Maximum 50 Karakter İçermelidir!");

            When(x => x.Email is not null, () =>
            {
                RuleFor(x => x.Email).EmailAddress().WithMessage("Lütfen Uygun Bir Mail Adresi Giriniz!");
            });


            RuleFor(x => x.PhoneNumber).MaximumLength(11).WithMessage("Telefon Numarası Alanı Maximum 11 Karakter İçermelidir!");

            When(x => x.PhoneNumber is not null, () =>
            {
                RuleFor(x => x.PhoneNumber).Matches(@"[0-9]+$").WithMessage("Telefon Numarası Alanı Sadece Sayı İçerebilir!");
            });


            RuleFor(x => x.Address).MaximumLength(100).WithMessage("Adres Alanı Maximum 100 Karakter İçermelidir!");
        }
    }
}

