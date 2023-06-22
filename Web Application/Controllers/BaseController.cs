using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web_Application.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext FilterContext)
        {
            
            string uFName = HttpContext.Session.GetString("userFirstName");
            string uLName = HttpContext.Session.GetString("userLastName");
            ViewBag._userName = uFName + " " + uLName;
            var result = FilterContext.HttpContext.Session.GetString("userId");
            //if (result == null)
            //{
            //    FilterContext.Result = new RedirectToActionResult("Login", "User", null);
                
            //}

        }

    }
}
