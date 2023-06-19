using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;

namespace Web_Application.Controllers
{
    public class IssueSupportController : BaseController
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin" || HttpContext.Session.GetString("userProfile") == "Support")
            {
                IssueDbHandle idh = new IssueDbHandle();
                return View(idh.GetIssueSupport());
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            
        }
    }
}
