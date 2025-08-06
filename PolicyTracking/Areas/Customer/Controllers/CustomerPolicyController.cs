using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTrackingWebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(AuthenticationSchemes = "Customer")]
    public class CustomerPolicyController : Controller
    {

        IPolicyService policyService;
        IUserService userService;

        public CustomerPolicyController(IPolicyService policyService, IUserService userService)
        {
            this.policyService = policyService;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            //Kullanıcını id değerini view e gönderiyoruz
            var username = User.Identity.Name;
            var userId = userService.Get(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            var values = policyService.GetListAllForPolicy(p=>p.CustomerId == userId);
            return View(values);
        }
    }
}
