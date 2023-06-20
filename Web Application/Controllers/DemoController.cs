using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;

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
            CompanyDbHandle idh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialCreateDemo");
        }

        public IActionResult GetCompanyDemo(int id)
        {
            CompanyDbHandle idh = new CompanyDbHandle();
            var result = idh.GetAllCompany(id);
            return Json(result);
        }
    }
}
