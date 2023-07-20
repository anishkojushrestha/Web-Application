using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class AMCEntryController : BaseController
    {
        private readonly INotyfService _toastNotification;
        public AMCEntryController(INotyfService toastNotification)
        {
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userProfile") != "OMSUser")
            {
                AMCEntryDdHandle ddh = new AMCEntryDdHandle();
                return View(ddh.GetAMCDetail());
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        public IActionResult CreateAMCEntry()
        {
            if (HttpContext.Session.GetString("userProfile") != "OMSUser")
            {
                CompanyDbHandle idh = new CompanyDbHandle();
                ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
                return PartialView("_PartialCreateAMCEntry");
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        public IActionResult EditAMCEntry(int id)
        {
            if (HttpContext.Session.GetString("userProfile") != "OMSUser")
            {
                AMCEntryDdHandle ddh = new AMCEntryDdHandle();
                return PartialView("_PartialCreateAMCEntry", ddh.GetAMCDetail().Find(x => x.Id == id));
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        [HttpPost]
        public IActionResult CreateAMCEntry(AMCEntryVM vm)
        {
            AMCEntryDdHandle ddh = new AMCEntryDdHandle();
            if (ModelState.IsValid)
            {
                if (vm.Id == null)
                {
                    if (ddh.CreateAMC(vm))
                    {
                        _toastNotification.Success("AMCEntry hasbeen added Successfully");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    if (ddh.CreateAMC(vm))
                    {
                        _toastNotification.Success("AMCEntry hasbeen edited Successfully");
                        return RedirectToAction("Index");
                    }
                }

            }
            return RedirectToAction("Index");
        }
    }
}
