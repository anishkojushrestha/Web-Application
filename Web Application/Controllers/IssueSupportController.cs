using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;

namespace Web_Application.Controllers
{
    public class IssueSupportController : Controller
    {
        public IActionResult Index()
        {
            IssueDbHandle idh = new IssueDbHandle();
            return View(idh.GetIssueSupport());
        }
    }
}
