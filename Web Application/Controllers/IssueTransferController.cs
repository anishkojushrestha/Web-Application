using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class IssueTransferController : BaseController
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin" || HttpContext.Session.GetString("userProfile") == "Support")
            {
                IssueDbHandle idh = new IssueDbHandle();
                return View(idh.GetIssueTransfer());
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            
        }
        
        
    }
}
