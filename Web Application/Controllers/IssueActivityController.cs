using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class IssueActivityController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IssueActivityController(IWebHostEnvironment _webHostEnvironment)
        {
            this._webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userProfile") == "SuperAdmin" || HttpContext.Session.GetString("userProfile") == "Admin" || HttpContext.Session.GetString("userProfile") == "Support")
            {
                IssueActivityDbHandle iadh = new IssueActivityDbHandle();
                return View(iadh.GetIssuesActivity());
            }
            else
            {
                return RedirectToAction("AccessDenied", "Error");
            }
        }
        public IActionResult IssueActivity(int id)
        {
            IssueDbHandle ish = new IssueDbHandle();
            ViewData["IssueActivity"] = new SelectList(ish.GetIssue(), "Id", "IssueNo");
            IssueActivityDbHandle iadh = new IssueActivityDbHandle();
            return PartialView("_PartialIssueActivity",iadh.GetIssuesActivity().Find(x => x.Id == id));
        }
        [HttpPost]
        public IActionResult IssueActivity(IssueActivityVM vm)
        { 
            IssueActivityDbHandle iad= new IssueActivityDbHandle();
                if (ModelState.IsValid)
                {
                    if (iad.CreateIssueActivity(vm, UploadFile(vm.Attachment)))
                    {
                        return RedirectToAction("Index");
                    }
                }
            
            
            return RedirectToAction("Index");
        }


        public IActionResult Attachment(int id)
        {
            IssueActivityDbHandle idh = new IssueActivityDbHandle();
            return PartialView("_PartialAttachment", idh.GetAttachmentsActivity(id));
        }

        private List<string> UploadFile(List<IFormFile> file)
        {
            List<string> files = new List<string>();
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
            return files;
        }
    }
}
