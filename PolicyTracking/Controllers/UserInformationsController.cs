using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using EntityLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTrackingWebUI.Controllers
{
    public class UserInformationsController : Controller
    {
        private readonly IMapper _mappper;
        private readonly UserManager<AppUser> _userManager;
        IUserService _userService;

        public UserInformationsController(IMapper mappper, UserManager<AppUser> userManager, IUserService userService)
        {
            _mappper = mappper;
            _userManager = userManager;
            _userService = userService;
        }

        public IActionResult Index(UserInformationViewModel model)
        {
            var username = User.Identity.Name;
            var user = _userService.Get(x => x.UserName == username).FirstOrDefault();

            var viewModel = new UserInformationViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
            };
            return View(viewModel);
        }

        public async Task<IActionResult> UpdatePassword(UserUpdateDTO model)
        {
            try
            {
                var validator = new UpdatePasswordValidator(_userManager);
                var result =await validator.ValidateAsync(model);

                if (result.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.Id.ToString());

                    var oldData = ExtractPrimitiveProperties(user) as Dictionary<string, object>;

                    var checkOldPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                    if (!checkOldPassword)
                        throw new Exception("Eski Şifre Hatalı!");

                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

                    _mappper.Map(model, user);


                    var result1 = await _userManager.UpdateAsync(user);

                    var newData = ExtractPrimitiveProperties(user) as Dictionary<string, object>;

                    await _userService.UpdateUserPassword(model,oldData,newData);

                    return Json(new { success = true, message = "Şifre Güncelleme Başarılı" });
                }
                else
                {
                    var errorList = result.Errors
                        .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                        .ToList();
                    return Json(new { success = false, errors = errorList });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private object ExtractPrimitiveProperties(object entity)
        {
            var properties = entity.GetType()
                .GetProperties()
                .Where(p =>
                {
                    var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    return type.IsPrimitive ||
                           type == typeof(string) ||
                           type == typeof(DateTime) ||
                           type == typeof(decimal) ||
                           type == typeof(Guid) ||
                           type.IsEnum;
                })
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            return properties;
        }

        private (Dictionary<string, object> original, Dictionary<string, object> changed)
    GetDifferences(Dictionary<string, object> originalData, Dictionary<string, object> currentData)
        {
            var originalDiff = new Dictionary<string, object>();
            var changedDiff = new Dictionary<string, object>();

            foreach (var key in originalData.Keys)
            {
                if (!currentData.ContainsKey(key)) continue;

                var originalValue = originalData[key];
                var currentValue = currentData[key];

                if ((originalValue == null && currentValue != null) ||
                    (originalValue != null && !originalValue.Equals(currentValue)))
                {
                    originalDiff[key] = originalValue;
                    changedDiff[key] = currentValue;
                }
            }

            return (originalDiff, changedDiff);
        }
    }
}
