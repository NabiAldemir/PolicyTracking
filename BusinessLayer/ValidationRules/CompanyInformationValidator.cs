using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;
using PolicyTracking.ViewModels;

namespace BusinessLayer.ValidationRules
{
    public class CompanyInformationValidator:AbstractValidator<AddCompanyInformationViewModel>
    {
        public CompanyInformationValidator()
        { 
            RuleFor(x=>x.companyName).NotEmpty().WithMessage("Şirket İsmi Alanı Boş Geçilemez!");
            RuleFor(x=>x.imageUrl).NotEmpty().WithMessage("Şirket Logosu Alanı Boş Geçilemez!");
            RuleFor(x=>x.companyName).MaximumLength(100).WithMessage("Şiret İsmi Alanı Maximum 100 Karakter Olmalıdır!");
        }
    }
}
