using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            CustomerDbHandle cdh = new CustomerDbHandle();
            return View(cdh.GetCustomer());
        }

        public IActionResult Customer()
        {
            return PartialView("_PartialAddCustomer");
        }

        [HttpPost]
        public IActionResult Customer(CustomerVM vm)
        {
            if(ModelState.IsValid)
            {
                CustomerDbHandle cdh = new CustomerDbHandle();
                if(cdh.AddCustomer(vm))
                {
                    ModelState.Clear(); 
                    return RedirectToAction("Index");   
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult EditCustomer(int id)
        {
            CustomerDbHandle cdh = new CustomerDbHandle();
            return PartialView("_PartialEditCustomer", cdh.GetCustomer().Find(x=>x.Id ==id));
        }

        [HttpPost]
        public IActionResult EditCustomer(CustomerVM vm)
        {
            if (ModelState.IsValid)
            {
                CustomerDbHandle cdh = new CustomerDbHandle();
                if (cdh.UpdateCustomer(vm))
                {
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                CustomerDbHandle sdb = new CustomerDbHandle();
                if (sdb.DeleteCustomer(id))
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
