using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class InstallationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateInstallation()
        {
            return PartialView("_PartialCreateInstallation");
        }
    }
}
