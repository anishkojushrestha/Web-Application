using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;

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
            CompanyDbHandle idh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialCreateAMCEntry");
        }
    }
}
