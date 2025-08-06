using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTrackingWebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class CustomerDashboardController : Controller
    {
        private readonly IPolicyService policyService;
        private readonly IUserService userService;
        ICustomerService customerService;

        public CustomerDashboardController(IPolicyService policyService,IUserService userService,ICustomerService customerService)
        {
            this.policyService = policyService;
            this.userService = userService;
            this.customerService = customerService;
        }

        public IActionResult Index()
        {
            //Kullanıcını id değerini view e gönderiyoruz
            var username = User.Identity.Name;
            var userId = userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            ViewBag.policyCount = policyService.Get(x => x.CustomerId == userId).Count();
            return View();
        }
    }
}
