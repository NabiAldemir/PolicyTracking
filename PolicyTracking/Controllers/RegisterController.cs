using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.Models;

namespace PolicyTracking.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(IUserService userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignUpViewModel p)
        {
            try
            {
                var validator = new UserSingUpValidator(_userManager);
                var results = await validator.ValidateAsync(p);

                if (results.IsValid)
                {
                    AppUser user = new AppUser()
                    {
                        Email = p.Mail,
                        UserName = p.UserName,
                        Name = p.Name,
                        Surname = p.Surname,
                        //Status = true,
                    };

                    var result = await _userManager.CreateAsync(user, p.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return View(p);
                    }
                }
                else
                {
                    foreach (var error in results.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(p);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Register");
            }
        }


    }
}
