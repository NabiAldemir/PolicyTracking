using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace PolicyTracking.ViewComponents.NameSurname
{
    public class NameSurnameForAllPages:ViewComponent
    {
        IUserService _userService;
        public NameSurnameForAllPages(IUserService userService)
        {
            _userService = userService;
        }
        public IViewComponentResult Invoke()
        {
            var username = User.Identity.Name;
            var name = _userService.Get(x => x.UserName == username).Select(x => x.Name + " " + x.Surname).FirstOrDefault();
            ViewBag.nameSurname = name;
            return View();
        }
    }
}
