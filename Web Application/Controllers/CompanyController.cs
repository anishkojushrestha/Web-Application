using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            CompanyDbHandle cdh = new CompanyDbHandle();
            return View(cdh.GetCompany());

        }
        public IActionResult AddCompany()
        {
            return PartialView("_PartialAddCompany");
        }

        [HttpPost]
        public IActionResult AddCompany(CompanyMV vm)
        {
            if (ModelState.IsValid)
            {
                
                CompanyDbHandle cdh = new CompanyDbHandle();
                //vm.contactPersonVM = new List<ContactPersonVM>();

                
               //string[] contactName = Request.Form["txtContactName"];

               // StringValues id; 
               // Request.Form.TryGetValue("txtContactName", out id);

                


              //  Request.Form.TryGetValue("txtContactName", out id);
              

                if (cdh.CreateCompany(vm))
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");


        }
        public ActionResult Delete(int id)
        {
            try
            {
                CompanyDbHandle sdb = new CompanyDbHandle();
                if (sdb.DeleteUser(id))
                {
                    ViewBag.AlertMsg = "Company Information Deleted Successfully";
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
