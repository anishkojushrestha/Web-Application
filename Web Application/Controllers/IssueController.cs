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
            return View();
        }

        public IActionResult Issue()
        {
            IssueDbHandle idh = new IssueDbHandle();
            ViewData["Assign"] = new SelectList(idh.GetUser(), "Id", "UserName");
            CompanyDbHandle cdh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(cdh.GetCompany(), "Id", "CompanyName");
            

            return PartialView("_PartialIssue");
        }

        [HttpPost]
        public IActionResult Issue(IssueVM vm)
        {
            UploadFile(vm.Attachments);
            return View();
        }

        public IActionResult Contact(int id) {
            IssueDbHandle idh = new IssueDbHandle();
            var result = idh.GetContact(id);
            return Json(result);
        }
        private string UploadFile(List<IFormFile> file)
        {
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

            }
            return uniqueFileName;
        }
    }
}
