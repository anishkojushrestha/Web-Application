using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            SupportDbHandle sdh = new SupportDbHandle();

            return View(sdh.GetSupport());
        }

        public IActionResult AddSupport()
        {
            return PartialView("_PartialAddSupport");
        }

        [HttpPost]
        public IActionResult AddSupport(SupportMV vm)
        {
            if (ModelState.IsValid)
            {
                SupportDbHandle sdh = new SupportDbHandle();
                if (sdh.AddSupport(vm))
                {
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            return View(vm);

        }
        public IActionResult EditSupport(int id)
        {
            SupportDbHandle adh= new SupportDbHandle();
            return PartialView("_PartialEditSupport", adh.GetSupport().Find(x=>x.Id == id));
        }

        [HttpPost]
        public IActionResult EditSupport(SupportMV vm)
        {
            if (ModelState.IsValid)
            {
                SupportDbHandle sdh = new SupportDbHandle();
                if (sdh.UpdateSupport(vm))
                {
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            return View(vm);

        }
        public ActionResult Delete(int id)
        {
            try
            {
                SupportDbHandle sdb = new SupportDbHandle();
                if (sdb.DeleteUser(id))
                {
                    ViewBag.AlertMsg = "Student Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
