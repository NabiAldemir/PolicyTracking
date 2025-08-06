using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.ViewModels;

namespace PolicyTracking.ViewComponents.Notifications
{
    public class NotificationsViewComponents:ViewComponent
    {
        IPolicyService _policyService;

        public NotificationsViewComponents(IPolicyService policyService)
        {
            _policyService = policyService;
        }
        public IViewComponentResult Invoke()
        {
            var upcomingPolicies = _policyService.GetUpcomingExpiringPolicies(7);

            var notifications = upcomingPolicies.Select(p => new NotificationViewModel
            {
                PolicyId = p.Id,
                PolicyType = p.PolicyType.Name,
                CustomerName = p.Customer.Name,
                CustomerSurname=p.Customer.Surname,
                EndDate = p.EndDate,
                DaysLeft = (p.EndDate - DateTime.Today).Days
            }).ToList();

            return View(notifications);
        }
    }
}
