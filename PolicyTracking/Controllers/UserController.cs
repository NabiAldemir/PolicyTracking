using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PolicyTracking.Models;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class UserController : Controller
    {
        private readonly ICustomerService customerService;
        IUserDal _userDal;
        private readonly UserManager<AppUser> _usermanager;
        IUserService _userService;

        public UserController(ICustomerService customerService,IUserDal userDal, UserManager<AppUser> usermanager, IUserService userService)
        {
            this.customerService = customerService;
            _userDal = userDal;
            _usermanager = usermanager;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var values=_userService.GetList();
            var customers = customerService.GetList();
            var customerList = customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name + " " + c.Surname+c.CompanyName
            }).ToList();
            var viewModel = new AddUserViewModel
            {
                CustomerList = customerList,
                AppUsers = values,
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserSignUpViewModel p)
        {
            try
            {
                var validator = new UserSingUpValidator(_usermanager);
                var results = await validator.ValidateAsync(p);

                if (results.IsValid)
                {
                    await _userService.AddUser(p);
                    return Json(new { success = true, message = "Başarılı" });
                }
                else
                {
                    var errorlist = results.Errors
                        .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                        .ToList();
                    return Json(new { success = false, errors = errorlist });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserUpdateDTO p)
        {
            try
            {
                var validator = new UserUpdateValidator(_usermanager,_userService);
                var results = await validator.ValidateAsync(p);

                if (!User.Identity.IsAuthenticated)
                    return Json(new { success = false, message = "Oturum açılmamış." });

                var username = User.Identity.Name;

                if (results.IsValid)
                {
                    await _userService.UpdateUser(p);
                    return Json(new { success = true, message = "Başarılı" });
                }
                else
                {
                    var errorlist = results.Errors
                        .Select(error => new { field = error.PropertyName, message = error.ErrorMessage })
                        .ToList();

                    return Json(new { success = false, errors = errorlist });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
               await _userService.DeleteUser(id);
                return Json(new { success = true, message = "Kullanıcı silme başarılı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sunucu hatası: " + ex.Message });
            }
        }

    }
}
