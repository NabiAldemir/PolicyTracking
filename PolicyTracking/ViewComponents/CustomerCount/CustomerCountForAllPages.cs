using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using PolicyTracking.ViewModels;

namespace PolicyTracking.ViewComponents.CustomerCount
{
    public class CustomerCountForAllPages:ViewComponent
    {
        ICustomerService _customerService;

        public CustomerCountForAllPages(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IViewComponentResult Invoke()
        {
            var customerCount = _customerService.GetList().Count();
            ViewBag.customerCount = customerCount;
            return View();
        }
    }
}
