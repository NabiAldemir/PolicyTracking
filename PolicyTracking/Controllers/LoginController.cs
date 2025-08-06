using System.Security.Claims;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PolicyTracking.Models;

namespace PolicyTracking.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [Route("/Account/Login")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/Account/Login")]
        public async Task<IActionResult> Index(UserSignInViewModel p)
        {
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = _configuration["GoogleReCaptcha:SecretKey"];

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}",
                    null);

                var json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json);

                if (result.success != true)
                {
                    TempData["ErrorMessage"] = "Lütfen reCAPTCHA doğrulamasını tamamlayın.";
                    return View(p);
                }
            }
            var validator = new LoginValidator();
            var validationResult = validator.Validate(p);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(p);
            }
            var user = await _userManager.FindByNameAsync(p.Username);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı adı.";
                return View(p);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, p.Password, false, true);

            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                string scheme = userRoles.Contains("Admin") ? "Admin" : "Customer";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, userRoles.FirstOrDefault())
                };

                var claimsIdentity = new ClaimsIdentity(claims, scheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(scheme, claimsPrincipal);

                if (scheme == "Admin")
                    return RedirectToAction("Index", "Dashboard");
                else if(scheme == "Customer")
                    return Redirect("/Customer/CustomerDashboard/Index");

            }
            else if (signInResult.IsLockedOut)
            {
                TempData["ErrorMessage"] = "Hesabınız kilitlenmiştir. Lütfen daha sonra tekrar deneyin.";
            }
            else
            {
                TempData["ErrorMessage"] = "Geçersiz şifre.";
            }

            return View(p);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        }
    }

