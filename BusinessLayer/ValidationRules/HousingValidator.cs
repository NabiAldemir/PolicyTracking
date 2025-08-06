using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class HousingValidator : AbstractValidator<Housing>
    {
        public HousingValidator()
        {
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
            RuleFor(x => x.PostalCode).MustAsync(BeLogicalPostalCode).WithMessage("Doğru Posta Kodunu Giriniz!")
                .When(x=> !string.IsNullOrWhiteSpace(x.PostalCode));
            RuleFor(x => x.TotalArea).NotEmpty().WithMessage("Toplam Alan Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("Toplam Alan Alanı Maximum 50 Karakter İçermelidir!");
            RuleFor(x => x.TotalArea)
                 .MustAsync(BeLogicalTotalArea).WithMessage("Doğru Toplam Alanı Giriniz!")
                 .When(x => !string.IsNullOrWhiteSpace(x.TotalArea));
            RuleFor(x => x.ConstructionYear).NotEmpty().WithMessage("Yapım Yılı Alanı Boş Geçilemez!")
                .MaximumLength(5).WithMessage("Yapım Yılı Alanı Maximum 5 Karakterden Oluşmalı");
            RuleFor(x => x.ConstructionYear).MustAsync(BeLogicalConstYear).WithMessage("Doğru Yapım Yılını Giriniz!")
                .When(x => !string.IsNullOrWhiteSpace(x.ConstructionYear));
            RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!"); ;
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");

        }

        private async Task<bool> BeLogicalConstYear(string constructionYear, CancellationToken cancellationToken)
        {
            if (!int.TryParse(constructionYear, out int parsedYear))
            {
                return false;
            }

            if (parsedYear <= 1950 || parsedYear > DateTime.Now.Year)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> BeLogicalTotalArea(string totalArea, CancellationToken cancellationToken)
        {
            if (!int.TryParse(totalArea, out int parsedTotalArea))
            {
                return false;
            }

            if (parsedTotalArea <= 10 || parsedTotalArea >= 2000)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> BeLogicalPostalCode(string postalCode, CancellationToken cancellationToken)
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
    public class HousingUpdateValidator : AbstractValidator<HousingUpdateDTO>
    {
        public HousingUpdateValidator()
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke Alanı Boş Geçilemez!")
               .MaximumLength(100).WithMessage("Ülke Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.City).NotEmpty().WithMessage("Şehir Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Şehir Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.Neighbourhood).NotEmpty().WithMessage("Mahalle/Sokak Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Mahalle/Sokak Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.District).NotEmpty().WithMessage("İlçe Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("İlçe Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Posta Kodu Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("Posta Kodu Alanı Maximum 50 Karakter İçermelidir!");
            RuleFor(x => x.TotalArea)
                .NotEmpty().WithMessage("Toplam Alan Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("Toplam Alan Alanı Maximum 50 Karakter İçermelidir!");
            RuleFor(x => x.TotalArea)
                 .MustAsync(BeLogicalTotalArea).WithMessage("Toplam Alan İçin Doğru Alanı Giriniz!")
                 .When(x => !string.IsNullOrWhiteSpace(x.TotalArea));
            RuleFor(x => x.ConstructionYear).NotEmpty().WithMessage("Yapım Yılı Alanı Boş Geçilemez!")
                .MaximumLength(5).WithMessage("Yapım Yılı Alanı Maximum 5 Karakterden Oluşmalı");
            RuleFor(x => x.ConstructionYear).MustAsync(BeLogicalConstYear).WithMessage("Lütfen Doğru Yapım Yılını Giriniz!")
                .When(x => !string.IsNullOrWhiteSpace(x.ConstructionYear));
            RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");

        }

        private async Task<bool> BeLogicalConstYear(string constructionYear, CancellationToken cancellationToken)
        {
            if (!int.TryParse(constructionYear, out int parsedYear))
            {
                return false;
            }

            if (parsedYear <= 1950 || parsedYear > DateTime.Now.Year)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> BeLogicalTotalArea(string totalArea, CancellationToken cancellationToken)
        {
            if (!int.TryParse(totalArea, out int parsedTotalArea))
            {
                return false;
            }

            if (parsedTotalArea <= 10 || parsedTotalArea >= 2000)
            {
                return false;
            }

            return true;
        }
    }
}
