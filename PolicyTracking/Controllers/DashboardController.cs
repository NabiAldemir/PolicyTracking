using System.Runtime.Intrinsics.X86;
using BusinessLayer.Abstract;
using BusinessLayer.Concerete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTracking.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class DashboardController : Controller
    {
        IUserService _userService;
        ICustomerService _customerService;
        IAgencyService _agencyService;
        IPolicyService _policyService;
        IPolicyTypeService _policyTypeService;
        IVehicleService _vehicleService;
        IHousingService _housingService;

        public DashboardController(IUserService userService, ICustomerService customerService, IAgencyService agencyService, IPolicyService policyService, IPolicyTypeService policyTypeService, IVehicleService vehicleService, IHousingService housingService)
        {
            _userService = userService;
            _customerService = customerService;
            _agencyService = agencyService;
            _policyService = policyService;
            _policyTypeService = policyTypeService;
            _vehicleService = vehicleService;
            _housingService = housingService;
        }

        public  IActionResult Index()
        {
            var username = User.Identity.Name;
            var name = _userService.Get(x => x.UserName == username).Select(x => x.Name + " " + x.Surname).FirstOrDefault();
            ViewBag.customerCount=_customerService.GetList().Count();
            ViewBag.agencyCount=_agencyService.GetList().Count();
            ViewBag.policyCount=_policyService.GetList().Count();
            ViewBag.policyTypeCount=_policyTypeService.GetList().Count();
            ViewBag.vehicleCount=_vehicleService.GetList().Count();
            ViewBag.housingCount=_housingService.GetList().Count();
            ViewBag.userCount=_userService.GetList().Count();
            ViewBag.notificationCount = _policyService.GetUpcomingExpiringPolicies(7).Count();
            ViewBag.namesurname = name;
            return View();
        }
    }
}
