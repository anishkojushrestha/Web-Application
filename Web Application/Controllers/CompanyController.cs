﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly INotyfService _toastNotification;
        public CompanyController(INotyfService toastNotification)
        {
            _toastNotification = toastNotification;
        }

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

                if (vm.Id == null)
                {
                    if (cdh.CreateCompany(vm))
                    {
                        _toastNotification.Success("Company hasbeen added Successfully");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    if (cdh.CreateCompany(vm))
                    {
                        _toastNotification.Success("Company hasbeen Edited Successfully");
                        return RedirectToAction("Index");
                    }
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
                return RedirectToAction("Index");
            }
        }

        public IActionResult GetContact(int id)
        {
            CompanyDbHandle cdh = new CompanyDbHandle();
            return PartialView("_PartialGetContact", cdh.GetContactDetail().Where(x => x.CompanyId == id));
        }

        public IActionResult EditCompany(int id) { 
            CompanyDbHandle cdh = new CompanyDbHandle();
            return PartialView("_PartialAddCompany", cdh.GetUpdateDetail(id.ToString(),""));
        }

        
        public IActionResult Details(int id, string name)
        {
            var data = new CompanyMV
            {
                Id = id,
                CompanyName=name,
            };
            return View(data);
        }
        public IActionResult GetDemos(int id)
        {
            DemoDbHandle ddh = new DemoDbHandle();
            var result = ddh.GetDemoDetail().Where(x => x.CompanyId == id).ToList();
            return Json(new {data = result });
        }
        public IActionResult GetAMC(int id)
        {
            AMCEntryDdHandle ddh = new AMCEntryDdHandle();
            var result = ddh.GetAMCDetail().Where(x => x.CompanyId == id).ToList();
            return Json(new { data = result });
        }
        public IActionResult GetUsers(int id)
        {
            UserDbHandle data = new UserDbHandle();
            var result = data.GetUser().Where(x=>x.CompanyId == id).ToList();
            return Json(new { data = result});
        }
        public IActionResult GetIssues(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result = idh.FilterDate().Where(x=>x.CompanyId == id).ToList();
            return Json(new { data = result });
        }
        public IActionResult DeleteContact(int id)
        {
            CompanyDbHandle cdh = new CompanyDbHandle();
            if (cdh.DeleteContact(id))
            {
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        

    }
}
