using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            UserDbHandle dbhandle = new UserDbHandle();
            ModelState.Clear();
            return View(dbhandle.GetUser());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            UserDbHandle data = new UserDbHandle();
            var result = data.GetUser();
            return Json(result);
        }

        public IActionResult Register()
        {
            CompanyDbHandle cdh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialRegister");
        }
        public IActionResult NewRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.NewPassword == vm.ConfirmPassword)
                {
                    UserDbHandle userDbHandle = new UserDbHandle();
                    if (userDbHandle.RegisteUser(vm))
                    {
                        ViewBag.Message = "Register Details Added Successfully";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                
                UserDbHandle userDbHandle = new UserDbHandle();
                if (userDbHandle.UserExist(vm.UserName, vm.Password) == true)
                {
                    ModelState.Clear();
                    return RedirectToAction("Index", "Company");
                }
            }
            return View();
        }
        public ActionResult EditRegister(int id)
        {
            UserDbHandle sdb = new UserDbHandle();
            CompanyDbHandle cdh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialEditRegister", sdb.GetUser().Find(vm => vm.Id == id));

        }

        [HttpPost]
        public ActionResult EditRegister(UpdateRegisterVM vm)
        {
            if (ModelState.IsValid)
            {

                UserDbHandle db = new UserDbHandle();

                if (db.UpdateRegister(vm.Id, vm.FirstName, vm.LastName, vm.UserName, vm.Email, vm.Profile, vm.CompanyId))
                {
                    ViewBag.Message = "Register Details Edited Successfully";
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
                UserDbHandle sdb = new UserDbHandle();
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
