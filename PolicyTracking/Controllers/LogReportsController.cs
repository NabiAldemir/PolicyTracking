using System.Globalization;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PolicyTracking.ViewModels;

namespace PolicyTracking.Controllers
{
    public class LogReportsController : Controller
    {
       ILogService _logService;
       IActionsService _actionsService;
       IUserService _userService;

        public LogReportsController(ILogService logService, IActionsService actionsService, IUserService userService)
        {
            _logService = logService;
            _actionsService = actionsService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var tableName = _logService.GetTableNames();
            var tableNameList = tableName.Select(x=>  new SelectListItem
            {
                Text = x,
                Value = x
            }).ToList();

            var actionValues = _actionsService.GetList();
            var actionList = actionValues.Select(x=> new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var userValues = _userService.GetList();
            var userList = userValues.Select(x => new SelectListItem
            {
                Text = x.Name+" "+x.Surname,
                Value = x.Id.ToString()
            }).ToList();

            var logValues = _logService.GetListAllLogs();

            var viewModel = new LogViewModel
            {
                ActionList = actionList,
                UserList = userList,
                TableNameList = tableNameList,
                LogList = logValues
            };
            return View(viewModel);
        }
        public IActionResult FilterLogs(int? userId,string tableName,int? actionId,string date)
        {
            var query = _logService.QueryableLog();

            if (actionId.HasValue)
            {
                query = query.Where(x=>x.ActionId == actionId);
            }
            if (!string.IsNullOrEmpty(tableName)) 
            {
                query = query.Where(x => x.TableName == tableName);
            }
            if (!string.IsNullOrEmpty(date))
            {
                if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
                {
                    query = query.Where(p => p.CreateDate.Date == parsedEndDate.Date);
                }
            }
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var result = query.Select(x => new
            {
                tableName = x.TableName,
                actionName = x.Actions.Name,
                userName = x.AppUser.Name+" "+x.AppUser.Surname,
                originalData = x.OriginalData,
                changedData = x.ChangedData,
                date = x.CreateDate.ToString("dd-MM-yyyy H:mm"),
                description = x.Description,
                id = x.Id,
            }).ToList();

            return Json(result);
        }
    }
}
