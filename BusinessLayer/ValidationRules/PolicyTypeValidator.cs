using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class PolicyTypeValidator:AbstractValidator<PolicyType>
    {
        public PolicyTypeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Poliçe Türü Adı Boş Geçilemez!");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Poliçe Türü Adı Maximum 100 Karakter Olmalıdır!");
        }
    }

    public class PolicyTypeUpdateValidator : AbstractValidator<PolicyTypeUpdateDTO>
    {
        public PolicyTypeUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Poliçe Türü Adı Boş Geçilemez!");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Poliçe Türü Adı Maximum 100 Karakter Olmalıdır!");
        }
    }
}
