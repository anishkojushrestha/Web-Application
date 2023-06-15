using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;

namespace Web_Application.Controllers
{
    public class IssueSupportController : BaseController
    {
        public IActionResult Index()
        {
            IssueDbHandle idh = new IssueDbHandle();
            return View(idh.GetIssueSupport());
        }
    }
}
