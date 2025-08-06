using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Models;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class CustomerValidator : AbstractValidator<CreateCustomerModel>
    {
        ICustomerService _customerService;

        public CustomerValidator(ICustomerService customerService)
        {
            _customerService = customerService;


            //Müşteri Türü
            RuleFor(x => x.CustomerTypeId).NotEmpty().WithMessage("Müşteri Türü Alanı Boş Geçilemez!");

            When(x => x.CustomerTypeId == 1, () =>
            {

                //TC KİMLİK
                RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("TC Kimlik Numarası Boş Geçilemez!")
                .Length(11).WithMessage("TC Kimlik Numarası 11 Haneli Olmalı!");

                // Önce sadece sayısal karakter kontrolü:
                RuleFor(x => x.IdentityNumber)
                    .Matches(@"^\d+$").WithMessage("TC Kimlik Numarası Sadece Sayılardan Oluşmalıdır!");

                // Sayı ve uzunluk uygunsa, diğer kontrolleri uygula:
                When(x =>
                    !string.IsNullOrWhiteSpace(x.IdentityNumber) &&
                    x.IdentityNumber.All(char.IsDigit) &&
                    x.IdentityNumber.Length == 11,
                    () =>
                    {
                        RuleFor(x => x.IdentityNumber)
                            .Must(ValidateTCNumber).WithMessage("TC Kimlik Numarası Uygun Değil!")
                            .MustAsync(BeUniqueIdentityNumber).WithMessage("TC Kimlik Numarası Benzersiz Olmalı!");
                    });



                //İSİM
                RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("İsim Alanı Maximum 50 Karakter İçermelidir!");

                When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
                {
                    RuleFor(x => x.Name)
                        .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$")
                        .WithMessage("İsim Alanı Yalnızca Harf ve Boşluk İçerebilir!");
                });


                //SOYİSİM
                RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyisim Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("Soyisim Alanı Maximum 50 Karakter İçermelidir!");

                When(x => x.Surname is not null, () =>
                {
                    RuleFor(x => x.Surname).Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$").WithMessage("Soyisim Alanı Yalnızca Harf ve Boşluk İçerebilir!");
                });

            });

            When(x => x.CustomerTypeId == 2, () =>
            {
                //Şirket İsmi
                RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Şirket İsmi Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Şİrket İsmi Maximum 100 Karakterden Oluşabilir!");

                RuleFor(x => x.CompanyName).Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ0-9\s\.\&\-,()']+$").WithMessage("Şirket İsmi Alanı Yalnızca Harf,Sayı ve Bazı Özel Karakterler İçerebilir!");
            });

            //Vergi Numarası
            RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Vergi Numarası alanı boş geçilemez!")
            .Matches(@"^\d{10}$").WithMessage("Vergi Numarası 10 haneli ve sadece rakamlardan oluşmalıdır!");



            //Vergi Ofisi
            RuleFor(x => x.TaxOffice).NotEmpty().WithMessage("Vergi Ofisi Alanı Boş Geçilemez!")
            .MaximumLength(50).WithMessage("Vergi Ofisi Alanı Maximum 50 Karakter İçermelidir!");


            //Telefon Numarası 1
            RuleFor(x => x.PhoneNumber1).NotEmpty().WithMessage("Telefon Numarası Alanı Boş Geçilemez!")
            .Length(11).WithMessage("Telefon Numarası Alanı 11 Karakter İçermelidir!");

            RuleFor(x => x.PhoneNumber1).Matches(@"[0-9]+$").WithMessage("Telefon Numarası Alanı Yalnızca Sayı İçerebilir!");


            //Telefon Numarası 2
            RuleFor(x => x.PhoneNumber2)
            .Length(11).WithMessage("Telefon Numarası Alanı 11 Karakter İçermelidir!")
            .Matches(@"[0-9]+$").WithMessage("Telefon Numarası Alanı Yalnızca Sayı İçerebilir!");


            //E-Mail
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!")
            .MaximumLength(50).WithMessage("E-Mail Alanı Maximum 50 Karakter İçermelidir!")
            .EmailAddress().WithMessage("Geçerli Bir Mail Adresi Giriniz!");

            When(x => x.Addresses is not null, () =>
            {
                RuleForEach(x => x.Addresses).ChildRules(address =>
                {
                    //Ülke
                    address.RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Ülke Alanı Maximum 100 Karakter İçermelidir!");


                    //Şehir
                    address.RuleFor(x => x.City).NotEmpty().WithMessage("Şehir Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Şehir Alanı Maximum 100 Karakter İçermelidir!");


                    //İlçe
                    address.RuleFor(x => x.District).NotEmpty().WithMessage("İlçe Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("İlçe Alanı Maximum 100 Karakter İçermelidir!");


                    //Mahalle
                    address.RuleFor(x => x.Neighbourhood).NotEmpty().WithMessage("Mahalle/Sokak Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Mahalle/Sokak Alanı Maximum 100 Karakter İçermelidir!");


                    //Kapı Numarası
                    address.RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                   .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                   .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!");


                    //Posta Kodu
                    address.RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Posta Kodu Alanı Boş Geçilemez!")
                    .MaximumLength(5).WithMessage("Posta Kodu Alanı Maximum 5 Karakter İçermelidir!")
                    .Matches(@"[0-9]+$").WithMessage("Posta Kodu Alanı Yalnızca Sayı İçermelidir!");
                });
            });
        }

        private async Task<bool> BeUniqueIdentityNumber(string idNumber, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetAsync(x => x.IdentityNumber == idNumber);
            return customer == null;
        }
        public static bool ValidateTCNumber(string tcNumber)
        {
            if (tcNumber.Length != 11 || !tcNumber.All(char.IsDigit)) return false;

            int[] digits = tcNumber.Select(c => c - '0').ToArray();
            int evenSum = digits.Where((x, i) => i % 2 == 0 && i < 9).Sum();
            int oddSum = digits.Where((x, i) => i % 2 == 1 && i < 9).Sum();

            return (evenSum * 7 - oddSum) % 10 == digits[9] && digits.Take(10).Sum() % 10 == digits[10];
        }

    }
    public class CustomerUpdateValidator : AbstractValidator<UpdateCustomerModel>
    {
        ICustomerService _customerService;
        public CustomerUpdateValidator(ICustomerService customerService)
        {

            _customerService = customerService;

            //Müşteri Türü
            RuleFor(x => x.CustomerTypeId).NotEmpty().WithMessage("Müşteri Türü Alanı Boş Geçilemez!");


            When(x => x.CustomerTypeId == 1, () =>
            {



                //TC KİMLİK
                RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("TC Kimlik Numarası Boş Geçilemez!")
                .Length(11).WithMessage("TC Kimlik Numarası 11 Haneli Olmalı!");

                // Önce sadece sayısal karakter kontrolü:
                RuleFor(x => x.IdentityNumber)
                    .Matches(@"^\d+$").WithMessage("TC Kimlik Numarası Sadece Sayılardan Oluşmalıdır!");

                RuleFor(x => x)
                    .Must(x => ValidateTCNumber(x.IdentityNumber))
                    .WithMessage("TC Kimlik Numarası Uygun Değil!")
                    .OverridePropertyName("IdentityNumber");

                // 2. Benzersizlik kontrolü
                RuleFor(x => x)
                    .MustAsync(BeUniqueIdentityNumber)
                    .WithMessage("TC Kimlik Numarası Benzersiz Olmalıdır !")
                    .OverridePropertyName("IdentityNumber");


                //İSİM
                RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("İsim Alanı Maximum 50 Karakter İçermelidir!");

                When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
                {
                    RuleFor(x => x.Name)
                        .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$")
                        .WithMessage("İsim Alanı Yalnızca Harf ve Boşluk İçerebilir!");
                });


                //SOYİSİM
                RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyisim Alanı Boş Geçilemez!")
                .MaximumLength(50).WithMessage("Soyisim Alanı Maximum 50 Karakter İçermelidir!");

                When(x => x.Surname is not null, () =>
                {
                    RuleFor(x => x.Surname).Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s]+$").WithMessage("Soyisim Alanı Yalnızca Harf ve Boşluk İçerebilir!");
                });

            });

            When(x => x.CustomerTypeId == 2, () =>
            {
                //Şirket İsmi
                RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Şirket İsmi Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Şİrket İsmi Maximum 100 Karakterden Oluşabilir!");

                RuleFor(x => x.CompanyName).Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ0-9\s\.\&\-,()']+$").WithMessage("Şirket İsmi Alanı Yalnızca Harf İçerebilir!");
            });

            //Vergi Numarası
            RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Vergi Numarası alanı boş geçilemez!")
            .Matches(@"^\d{10}$").WithMessage("Vergi Numarası 10 haneli ve sadece rakamlardan oluşmalıdır!");


            //Vergi Ofisi
            RuleFor(x => x.TaxOffice).NotEmpty().WithMessage("Vergi Ofisi Alanı Boş Geçilemez!")
            .MaximumLength(50).WithMessage("Vergi Ofisi Alanı Maximum 50 Karakter İçermelidir!");


            //Telefon Numarası 1
            RuleFor(x => x.PhoneNumber1).NotEmpty().WithMessage("Telefon Numarası Alanı Boş Geçilemez!")
            .MaximumLength(11).WithMessage("Telefon Numarası Alanı Maximum 11 Karakter İçermelidir!");

            RuleFor(x => x.PhoneNumber1).Matches(@"[0-9]+$").WithMessage("Telefon Numarası Alanı Yalnızca Sayı İçerebilir!");


            //Telefon Numarası 2
            RuleFor(x => x.PhoneNumber2)
            .MaximumLength(11).WithMessage("Telefon Numarası Alanı Maximum 11 Karakter İçermelidir!")
            .Matches(@"[0-9 -]+$").WithMessage("Telefon Numarası Alanı Yalnızca Sayı İçerebilir!");


            //E-Mail
            RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!")
            .MaximumLength(50).WithMessage("E-Mail Alanı Maximum 50 Karakter İçermelidir!")
            .EmailAddress().WithMessage("Geçerli Bir Mail Adresi Giriniz!");

            When(x => x.Addresses is not null, () =>
            {
                RuleForEach(x => x.Addresses).ChildRules(address =>
                {
                    //Ülke
                    address.RuleFor(x => x.Country).NotEmpty().WithMessage("Ülke Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Ülke Alanı Maximum 100 Karakter İçermelidir!");


                    //Şehir
                    address.RuleFor(x => x.City).NotEmpty().WithMessage("Şehir Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Şehir Alanı Maximum 100 Karakter İçermelidir!");


                    //İlçe
                    address.RuleFor(x => x.District).NotEmpty().WithMessage("İlçe Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("İlçe Alanı Maximum 100 Karakter İçermelidir!");


                    //Mahalle
                    address.RuleFor(x => x.Neighbourhood).NotEmpty().WithMessage("Mahalle/Sokak Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Mahalle/Sokak Alanı Maximum 100 Karakter İçermelidir!");


                    //Kapı Numarası
                    address.RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                   .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                   .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!");


                    //Posta Kodu
                    address.RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Posta Kodu Alanı Boş Geçilemez!")
                    .MaximumLength(5).WithMessage("Posta Kodu Alanı Maximum 5 Karakter İçermelidir!")
                    .Matches(@"[0-9]+$").WithMessage("Posta Kodu Alanı Yalnızca Sayı İçermelidir!");
                });
            });
        }

        private async Task<bool> BeUniqueIdentityNumber(UpdateCustomerModel model, CancellationToken token)
        {
            return !await _customerService.AnyAsync(x =>
            x.IdentityNumber == model.IdentityNumber &&
            x.Id != model.Id,
            token);
        }
        public static bool ValidateTCNumber(string tcNumber)
        {
            if (tcNumber.Length != 11 || !tcNumber.All(char.IsDigit)) return false;

            int[] digits = tcNumber.Select(c => c - '0').ToArray();
            int evenSum = digits.Where((x, i) => i % 2 == 0 && i < 9).Sum();
            int oddSum = digits.Where((x, i) => i % 2 == 1 && i < 9).Sum();

            return (evenSum * 7 - oddSum) % 10 == digits[9] && digits.Take(10).Sum() % 10 == digits[10];
        }
    }

}
