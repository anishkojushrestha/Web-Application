using Intersoft.Crosslight;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class IssueController : BaseController
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
        public IActionResult GetValue()
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result= idh.GetIssue();
            return Json(result);
        }

        public IActionResult Issue(int id)
        {
            CompanyDbHandle cdh = new CompanyDbHandle();
            IssueDbHandle idh = new IssueDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompanyUser(), "Id", "CompanyName");
            ViewData["Support"] = new SelectList(idh.GetUser(), "Id", "UserName");
            return PartialView("_PartialIssue", idh.GetIssue().Find(x => x.Id == id));
        }

        [HttpPost]
        public IActionResult Issue(IssueVM vm)
        {
            if (vm.Id == null)
            {
                if (ModelState.IsValid)
                {
                    IssueDbHandle idh = new IssueDbHandle();
                    if (idh.CreateIssue(vm, UploadFile(vm.Attachments)))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    IssueDbHandle idh = new IssueDbHandle();
                    if (idh.UpdateIssue(vm, UploadFile(vm.Attachments)))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetEmail(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result= idh.Email(id);
            return Json(result);
        }
        public IActionResult GetUserEmail(int id, string username)
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result = idh.UserEmail(id, username);
            return Json(result);
        }

        public IActionResult GetTransfer(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result= idh.Transfer(id);
            return Json(result);
        }

        public IActionResult Contact(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            var result = idh.GetContact(id);
            return Json(result);
        }
      
       

        [HttpPost]
        public IActionResult Assign(IssueVM vm)
        {
            if (ModelState.IsValid)
            {
                IssueDbHandle ish = new IssueDbHandle();
                if (ish.Assigned(vm))
                {
                    return RedirectToAction("Index");
                }

            };
            return RedirectToAction("Index");
        }

        public IActionResult Attachment(int id)
        {
            IssueDbHandle idh = new IssueDbHandle();
            return PartialView("_PartialAttachment", idh.GetAttachments(id));
        }

        [HttpPost]
        public IActionResult Transfer(IssueTransferVM vm)
        {
            if (ModelState.IsValid)
            {
                IssueDbHandle idh = new IssueDbHandle();
                if (idh.CreateIssueTransfer(vm))
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        private List<string> UploadFile(List<IFormFile> file)
        {
            List<string> files = new List<string>();
            if (file != null)
            {

                string uniqueFileName = "";
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                foreach (var data in file)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + data.FileName;
                    var filePath = Path.Combine(folderPath, uniqueFileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        data.CopyTo(fileStream);
                    }

                    files.Add(uniqueFileName);
                }

            }
            return files;
        }
    }
}
