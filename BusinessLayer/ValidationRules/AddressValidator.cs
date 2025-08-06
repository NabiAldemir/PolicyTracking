using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
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
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));
            RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!"); ;
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");
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

    public class AddressUpdateValidator : AbstractValidator<AddressUpdateDTO>
    {
        public AddressUpdateValidator()
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
                .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));
            RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!"); ;
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");
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
}
