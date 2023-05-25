using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class InqueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInquery()
        {
            return PartialView("_partialCreateInquery");
        }
    }
}
