using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PolicyTracking.ViewModels
{
    public class LogViewModel
    {
        public List<SelectListItem> TableNameList { get; set; }
        public List<SelectListItem> UserList { get;set; }
        public List<SelectListItem> ActionList { get; set; }
        public List<Log> LogList {  get; set; }  
    }
}
