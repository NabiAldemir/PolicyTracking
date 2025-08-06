using Microsoft.AspNetCore.Mvc;

namespace PolicyTracking.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
