using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class AMCEntryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAMCEntry()
        {
            return PartialView("_PartialCreateAMCEntry");
        }
    }
}
