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
    public class InstitutionalCustomerValidator : AbstractValidator<CreateCustomerModel>
    {
        //    IInstitutionalCustomerService _institutionalCustomerService;
        //    public InstitutionalCustomerValidator(IInstitutionalCustomerService institutionalCustomerService)
        //    {
        //        _institutionalCustomerService = institutionalCustomerService;
        //        RuleFor(x=>x.CompanyName).NotEmpty().WithMessage("Şirket İsmi Alanı Boş Bırakılamaz!")
        //            .MaximumLength(150).WithMessage("Şirket İsmi Alanı Maximum 150 Karakter Olmalı1");

        //        RuleFor(x => x.TaxNumber).NotEmpty().WithMessage("Vergi Numarası Alanı Boş Geçilemez!");

        //        RuleFor(x => x.TaxOffice).NotEmpty().WithMessage("Vergi Ofisi Alanı Boş Geçilemez!")
        //            .MaximumLength(50).WithMessage("Vergi Ofisi Alanı Maximum 50 Karakter İçermelidir!");

        //        RuleFor(x => x.PhoneNumber1).NotEmpty().WithMessage("Telefon Numarası Alanı Boş Geçilemez!")
        //            .MaximumLength(20).WithMessage("Telefon Numarası Alanı Maximum 20 Karakter İçermelidir!");

        //        RuleFor(x => x.Email).NotEmpty().WithMessage("Mail Alanı Boş Geçilemez!")
        //            .EmailAddress().WithMessage("Geçerli Bir Mail Adresi Giriniz!")
        //            .MaximumLength(50).WithMessage("E-Mail Alanı Maximum 50 Karakter İçermelidir!");

        //        RuleFor(x => x.AuthorizedIdentityNumber).NotEmpty().WithMessage("Yetkili Kişi TC Boş Geçilemez!")
        //            .MaximumLength(11).WithMessage("Yetkili Kişi TC Maximum 11 Karakter Olmalı!")
        //            .Matches("^[1-9][0-9]{10}$").WithMessage("Geçerli bir TC Kimlik Numarası giriniz."); ;

        //        RuleFor(x => x.AuthorizedIdentityNumber)
        //            .MustAsync(BeUniqueAuthorizedIdentityNumber).WithMessage("Yetkili Kişi TC Benzersiz Olmalı!");

        //        RuleFor(x => x.AuthorizedPersonName).NotEmpty().WithMessage("Yetkili Kişi Adı Boş Geçilemez!")
        //            .MaximumLength(11).WithMessage("Yetkili Kişi Adı Maximum 100 Karakter Olmalı!");

        //        RuleFor(x => x.AuthorizedPersonSurname).NotEmpty().WithMessage("Yetkili Kişi Soyadı Boş Geçilemez!")
        //            .MaximumLength(11).WithMessage("Yetkili Kişi Soyadı Maximum 100 Karakter Olmalı!");

        //        RuleFor(x => x.TradeRegistryNumber).NotEmpty().WithMessage("Ticaret Sicil Numarası Boş Geçilemez!")
        //            .MaximumLength(11).WithMessage("Ticaret Sicil Numarası Maximum 20 Karakter Olmalı!");
        //    }
        //    private async Task<bool> BeUniqueAuthorizedIdentityNumber(string identityNumber,CancellationToken cancellationToken)
        //    {
        //        var customer = await _institutionalCustomerService.GetAsync(x=>x.IdentityNumber == identityNumber);
        //        if (customer is not null)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //}
    }
}
