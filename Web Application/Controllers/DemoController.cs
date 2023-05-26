using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateDemo()
        {
            return PartialView("_PartialCreateDemo");
        }
    }
}
