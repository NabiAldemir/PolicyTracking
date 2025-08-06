using Microsoft.AspNetCore.Mvc;

namespace PolicyTrackingWebUI.Areas.Customer.Controllers
{
    public class CustomerUserInformationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
