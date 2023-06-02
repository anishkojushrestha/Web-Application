using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class IssueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
