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
                    //HttpContext.Session.SetString("userId", userinfo[0].Id.ToString());
                    //HttpContext.Session.SetString("userName", userinfo[0].UserName.ToString());
                    //HttpContext.Session.SetString("userFirstName", userinfo[0].FirstName.ToString());
                    //HttpContext.Session.SetString("userLastName", userinfo[0].LastName.ToString());
                    //HttpContext.Session.SetString("userProfile", userinfo[0].Profile.ToString());
                    //HttpContext.Session.SetString("userEmail", userinfo[0].Email.ToString());
                    //HttpContext.Session.SetString("companyName", userinfo[0].CompanyName.ToString());
                    //HttpContext.Session.SetString("password", userinfo[0].NewPassword.ToString());
                    //HttpContext.Session.SetString("address", userinfo[0].Address.ToString());
                    //HttpContext.Session.SetString("validFrom", userinfo[0].ValidFrom.ToString());
                    //HttpContext.Session.SetString("validTo", userinfo[0].ValidTo.ToString());
                    //HttpContext.Session.SetString("companyEmail", userinfo[0].CompanyEmail.ToString());
                    //HttpContext.Session.SetString("registrationDate", userinfo[0].RegistrationDate.ToString());
                    //ViewBag._userName = userinfo[0].FirstName.ToString() + "," + userinfo[0].LastName.ToString();
                    SessionHandle session = new SessionHandle();
                    session.Session(userinfo[0].Id.ToString(), userinfo[0].UserName, userinfo[0].FirstName, userinfo[0].LastName, userinfo[0].Profile, userinfo[0].Email, userinfo[0].CompanyName, userinfo[0].NewPassword, userinfo[0].Address, userinfo[0].ValidFrom, userinfo[0].ValidTo, userinfo[0].CompanyEmail, userinfo[0].RegistrationDate);
                    return RedirectToAction("Index", "Company");
                }
                ViewBag.UserError = "InCorrect credentials ";

                
            }
            return View();
        }
    }
}
