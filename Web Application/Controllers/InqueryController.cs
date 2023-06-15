using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers
{
    public class InqueryController : BaseController
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
