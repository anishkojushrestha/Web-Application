using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class AMCEntryController : Controller
    {
        public IActionResult Index()
        {
            AMCEntryDdHandle ddh = new AMCEntryDdHandle();
            return View(ddh.GetAMCDetail());
        }
        public IActionResult CreateAMCEntry()
        {
            CompanyDbHandle idh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialCreateAMCEntry");
        }
        public IActionResult EditAMCEntry(int id)
        {
            AMCEntryDdHandle ddh = new AMCEntryDdHandle();
            return PartialView("_PartialCreateAMCEntry",ddh.GetAMCDetail().Find(x=>x.Id == id));
        }
        [HttpPost]
        public IActionResult CreateAMCEntry(AMCEntryVM vm)
        {
            AMCEntryDdHandle ddh = new AMCEntryDdHandle();
            if (ModelState.IsValid)
            {
                if (ddh.CreateAMC(vm))
                {
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }
    }
}
