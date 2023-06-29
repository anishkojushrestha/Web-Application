using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class UserController : Controller
    {
        
        private readonly ILogger<UserController> logger;
        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }
        public IActionResult Index()
        {
            //if(HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin")
            //{
                UserDbHandle dbhandle = new UserDbHandle();
                ModelState.Clear();
                return View(dbhandle.GetUser());
            //}
            //else
            //{
              //  return RedirectToAction("AccessDenied", "Error");
            //}
            
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
            //if (HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin")
            //{
                CompanyDbHandle cdh = new CompanyDbHandle();
                ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
                return PartialView("_PartialRegister");
            //}
            //else
            //{
              //  return RedirectToAction("AccessDenied", "Error");
            //}
        }
        
        public IActionResult UserError(string username)
        {
                UserDbHandle userDbHandle = new UserDbHandle();
            var result = userDbHandle.UserExist(username);
            return Json(result);
        }

        [HttpPost]
        
        public IActionResult Register(RegisterVM vm)
        {
            UserDbHandle userDbHandle = new UserDbHandle();
            if (ModelState.IsValid)
            {
                if (userDbHandle.UserExist(vm.UserName) )
                {
                    ViewBag.usererror = "User Already Exis";
                    TempData["error"] = "User Already Exis";
                }
                else
                {
                    if (vm.NewPassword == vm.ConfirmPassword)
                    {
                        if (userDbHandle.RegisteUser(vm))
                        {
                            ViewBag.Message = "Register Details Added Successfully";
                            ModelState.Clear();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("userId") != null)
            {
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]

        public IActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                UserDbHandle userDbHandle = new UserDbHandle();
                var userinfo = userDbHandle.GetUser(vm.UserName, vm.Password);
                if (userinfo.Count > 0)
                {
                    HttpContext.Session.SetString("userId", userinfo[0].Id.ToString());
                    HttpContext.Session.SetString("userFirstName", userinfo[0].FirstName.ToString());
                    HttpContext.Session.SetString("userLastName", userinfo[0].LastName.ToString());
                    HttpContext.Session.SetString("userProfile", userinfo[0].Profile.ToString());
                    ViewBag._userName = userinfo[0].FirstName.ToString() + "," + userinfo[0].LastName.ToString();
                    return RedirectToAction("Index", "Company");
                }
                ViewBag.UserError = "InCorrect credentials ";

                //if (userDbHandle.UserExist(vm.UserName, vm.Password) == true)
                //{
                //    var userinfo = userDbHandle.GetUser(vm.UserName);
                //    if (userinfo.Count > 0)
                //    {

                //    }
                //    HttpContext.Session.SetString("Username", vm.UserName);
                //    HttpContext.Session.SetString("Password", vm.Password);
                //    var username = HttpContext.Session.GetString("Username");
                //    var password = HttpContext.Session.GetString("Password");
                //    logger.LogInformation(username, password);
                   
                //}
            }
            return View();
        }
        public ActionResult EditRegister(int id)
        {
           // if (HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin")
            //{
                UserDbHandle sdb = new UserDbHandle();
            CompanyDbHandle cdh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialEditRegister", sdb.GetUser().Find(vm => vm.Id == id));
            //}
            //else
            //{
              //  return RedirectToAction("AccessDenied", "Error");
            //}

        }

        [HttpPost]
        public ActionResult EditRegister(UpdateRegisterVM vm)
        {
            if (ModelState.IsValid)
            {

                UserDbHandle db = new UserDbHandle();

                if (db.UpdateRegister(vm.Id, vm.FirstName, vm.LastName, vm.UserName, vm.Email, vm.Profile, vm.CompanyId, vm.IsActive))
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }



    }
}
