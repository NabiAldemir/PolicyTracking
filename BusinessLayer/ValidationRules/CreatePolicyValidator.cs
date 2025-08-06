using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.Models;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class CreatePolicyValidator : AbstractValidator<CreatePolicyModel>
    {
        IPolicyService _policyService;
        IPolicyTypeService _policyTypeService;
        public CreatePolicyValidator(IPolicyTypeService policyTypeService, IPolicyService policyService)
        {
            _policyService = policyService;
            _policyTypeService = policyTypeService;
            RuleFor(x => x.SelectedType).NotEmpty().WithMessage("Seçim Alanı Boş Geçilemez!");


            RuleFor(x => x.PolicyNumber).NotEmpty().WithMessage("Poliçe Numarası Alanı Boş Geçilemez!")
                .Matches(@"[0-9]+$").WithMessage("Poliçe Numarası Alanı Yalnızca Sayı İçerebilir!")
                .MaximumLength(50).WithMessage("Poliçe Numarası Alanı Maximum 50 Karakter İçermelidir!");



            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Başlama Tarihi Alanı Boş Geçilemez!");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Bitiş Tarihi Alanı Boş Geçilemez!");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.AgencyId).NotEmpty().WithMessage("Acente Alanı Boş Geçilemez!");

            var selectedVehicleId = _policyTypeService.GetList().Where(x => x.Name == "Araç Sigortası").Select(x => x.Id).FirstOrDefault();
            var selectedHousingId = _policyTypeService.GetList().Where(x => x.Name == "Konut Sigortası").Select(x => x.Id).FirstOrDefault();

            When(x => x.SelectedType == 1 ||x.SelectedType == 9, () =>
            {
                RuleFor(x => x.Plate).NotEmpty().WithMessage("Plaka Alanı Boş Geçilemez!")
                    .Matches(@"^[A-Za-z0-9 ]+$").WithMessage("Plaka sadece harf ve rakam içerebilir!");
                RuleFor(x => x.Brand).NotEmpty().WithMessage("Marka Alanı Boş Geçilemez!");
                RuleFor(x => x.Model).NotEmpty().WithMessage("Model Alanı Boş Geçilemez!");
                RuleFor(x => x.ProductionYear).NotEmpty().WithMessage("Üretim Yılı Alanı Boş Geçilemez!")
                .Must(BeLogicalProductionYear).WithMessage("Doğru Üretim Yılını Giriniz!");
                RuleFor(x => x.Plate).MaximumLength(20).WithMessage("Plaka Alanı Maximum 20 Karakter İçermelidir!");
                RuleFor(x => x.Brand).MaximumLength(50).WithMessage("Marka Alanı Maximum 50 Karakter İçermelidir!");
                RuleFor(x => x.Model).MaximumLength(50).WithMessage("Model Alanı Maximum 50 Karakter İçermelidir!");
            });

            When(x => x.SelectedType == 2 || x.SelectedType == 10 || x.SelectedType == 11, () =>
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
                RuleFor(x => x.PostalCode).Must(BeLogicalPostalCode).WithMessage("Doğru Posta Kodunu Giriniz!")
                    .When(x => !string.IsNullOrWhiteSpace(x.PostalCode));
                RuleFor(x => x.TotalArea).NotEmpty().WithMessage("Toplam Alan Alanı Boş Geçilemez!")
                    .MaximumLength(50).WithMessage("Toplam Alan Alanı Maximum 50 Karakter İçermelidir!");
                RuleFor(x => x.TotalArea)
                     .Must(BeLogicalTotalArea).WithMessage("Doğru Toplam Alanı Giriniz!")
                     .When(x => !string.IsNullOrWhiteSpace(x.TotalArea));
                RuleFor(x => x.ConstructionYear).NotEmpty().WithMessage("Yapım Yılı Alanı Boş Geçilemez!")
                    .MaximumLength(5).WithMessage("Yapım Yılı Alanı Maximum 5 Karakterden Oluşmalı");
                RuleFor(x => x.ConstructionYear).Must(BeLogicalConstYear).WithMessage("Doğru Yapım Yılını Giriniz!")
                    .When(x => !string.IsNullOrWhiteSpace(x.ConstructionYear));
                RuleFor(x => x.DoorNumber).NotEmpty().WithMessage("Kapı Numarası Alanı Boş Geçilemez!")
                    .MaximumLength(100).WithMessage("Kapı Numarası Alanı Maximum 100 Karakter İçermelidir!")
                    .Matches(@"^[A-Za-z0-9/ -]+$").WithMessage("Kapı Numarası sadece harf, rakam, boşluk ve '/' içerebilir!"); ;
                RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
                RuleFor(x => x.Street).MaximumLength(100).WithMessage("Cadde Alanı Maximum 100 Karakter İçermelidir!");
            });
        }
        private static bool BeLogicalConstYear(string constructionYear)
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

        private static bool BeLogicalTotalArea(string totalArea)
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
        private  static bool BeLogicalPostalCode(string postalCode)
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
        private static bool BeLogicalProductionYear(int productionYear)
        {

            if (productionYear <= 1950 || productionYear > DateTime.Now.Year)
            {
                return false;
            }

            return true;
        }
    }
}
