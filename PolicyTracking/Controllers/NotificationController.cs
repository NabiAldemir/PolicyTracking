using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class NotificationController : Controller
    {
        IPolicyService _policyService;

        public NotificationController(IPolicyService policyService)
        {
            _policyService = policyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetFilteredNotifications(int daysLeft)
        {
            var today = DateTime.Now;
            var upcomingday = today.AddDays(daysLeft);

            var query = _policyService.QueryablePolicy();

            if (daysLeft != 0)
            {
                query = query.Where(x => x.EndDate >= today && x.EndDate <= upcomingday);
            }

            var result = query.Select(p => new
            {
                PolicyId = p.Id,
                PolicyType = p.PolicyType.Name,
                CustomerName = p.Customer.Name+" "+ p.Customer.Surname + p.Customer.CompanyName,
                EndDate = p.EndDate.ToString("dd-MM-yyyy"),
                DaysLeft = (p.EndDate - DateTime.Today).Days,
            }).ToList();

            return Json(result);
        }
        public JsonResult GetCalendarNotifications()
        {
            try
            {
                var notifications = _policyService.GetListAllForPolicy().Select(x => new
                {
                    policyNumber = x.PolicyNumber,
                    policyType = x.PolicyType.Name,
                    customer = x.Customer.Name + " " + x.Customer.Surname + x.Customer.CompanyName,
                    title = x.PolicyNumber + " - " + x.Customer.Name + " " + x.Customer.Surname + x.Customer.CompanyName,
                    start = x.EndDate.ToString("yyyy-MM-dd"),
                    daysLeft = (x.EndDate - DateTime.Today).Days,

                }).ToList();
                return Json(notifications);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Başarısız" });
            }
            
        }
    }
}