using Intersoft.Crosslight;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class IssueController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IssueController(IWebHostEnvironment _webHostEnvironment)
        {
            this._webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            IssueDbHandle idh = new IssueDbHandle();
            return View(idh.GetIssue());
        }

        public IActionResult Issue(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            ViewData["Assign"] = new SelectList(idh.GetUser(), "Id", "UserName");
            CompanyDbHandle cdh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
            

            return PartialView("_PartialIssue", idh.GetIssue().Find(x=>x.Id ==id));
        }

        [HttpPost]
        public IActionResult Issue(IssueVM vm)
        {
            
            if (ModelState.IsValid){
                IssueDbHandle idh = new IssueDbHandle();
                if (idh.CreateIssue(vm, UploadFile(vm.Attachments)))
                {
                    
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        

        public IActionResult Contact(int id) {
            IssueDbHandle idh = new IssueDbHandle();
            var result = idh.GetContact(id);
            return Json(result);
        }
        public IActionResult DeletedBY(int id)
        {
            IssueDbHandle ish = new IssueDbHandle();
            ish.DeletedBy(id);
            return RedirectToAction("Index");
        }
        public IActionResult Resolve(int id) {
            return View("_PartialResolve");
        }
        [HttpPost]
        public IActionResult Resolve(IssueVM vm)
        {
            if(ModelState.IsValid) {
                IssueDbHandle ish = new IssueDbHandle();
                if (ish.Resolve(vm))
                {
                    return RedirectToAction("Index");
                }
            
            };
            return RedirectToAction("Index");
        }

        public IActionResult Attachment(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            return PartialView("_PartialAttachment",idh.GetAttachments(id));
        }
        public ActionResult Delete(int id)
        {
            try
            {
                IssueDbHandle sdb = new IssueDbHandle();
                if (sdb.DeleteIssue(id))
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
        private List<string> UploadFile(List<IFormFile> file)
        {
            List<string> files = new List<string>();
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
            foreach(var data in file)
            {
                
                uniqueFileName = Guid.NewGuid().ToString() + "_" + data.FileName;
                var filePath = Path.Combine(folderPath, uniqueFileName);
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    data.CopyTo(fileStream);
                }
                
                files.Add(uniqueFileName);
            }
            return files;
        }
        
        
    }
}
