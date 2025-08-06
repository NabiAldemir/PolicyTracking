using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class VehicleValidator:AbstractValidator<Vehicle>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.Plate).NotEmpty().WithMessage("Plaka Alanı Boş Geçilemez!");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Marka Alanı Boş Geçilemez!");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model Alanı Boş Geçilemez!");
            RuleFor(x => x.ProductionYear).NotEmpty().WithMessage("Üretim Yılı Alanı Boş Geçilemez!");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
            RuleFor(x => x.Plate).MaximumLength(20).WithMessage("Plaka Alanı Maximum 20 Karakter İçermelidir!");
            RuleFor(x => x.Brand).MaximumLength(50).WithMessage("Marka Alanı Maximum 50 Karakter İçermelidir!");
            RuleFor(x => x.Model).MaximumLength(50).WithMessage("Model Alanı Maximum 50 Karakter İçermelidir!");
        }

        public class VehicleUpdateValidator : AbstractValidator<VehicleUpdateDTO>
        {
            public VehicleUpdateValidator()
            {
                RuleFor(x => x.Plate).NotEmpty().WithMessage("Plaka Alanı Boş Geçilemez!");
                RuleFor(x => x.Brand).NotEmpty().WithMessage("Marka Alanı Boş Geçilemez!");
                RuleFor(x => x.Model).NotEmpty().WithMessage("Model Alanı Boş Geçilemez!");
                RuleFor(x => x.ProductionYear).NotEmpty().WithMessage("Üretim Yılı Alanı Boş Geçilemez!");
                RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri Alanı Boş Geçilemez!");
                RuleFor(x => x.Plate).MaximumLength(20).WithMessage("Plaka Alanı Maximum 20 Karakter İçermelidir!");
                RuleFor(x => x.Brand).MaximumLength(50).WithMessage("Marka Alanı Maximum 50 Karakter İçermelidir!");
                RuleFor(x => x.Model).MaximumLength(50).WithMessage("Model Alanı Maximum 50 Karakter İçermelidir!");
            }
        }
    }
}
