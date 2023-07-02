using Microsoft.AspNetCore.Mvc;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

                
            }
            return View();
        }
    }
}
