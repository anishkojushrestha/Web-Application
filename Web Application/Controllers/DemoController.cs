using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class DemoController : BaseController
    {

    private readonly IWebHostEnvironment _webHostEnvironment;
        public DemoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            DemoDbHandle ddh = new DemoDbHandle();
            return View(ddh.GetDemoDetail());
        }
        
        public IActionResult CreateDemo()
        {
            CompanyDbHandle idh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialCreateDemo");
        }
        public IActionResult EditDemo(int id)
        {
            DemoDbHandle ddh = new DemoDbHandle();
            CompanyDbHandle idh = new CompanyDbHandle();
            ViewData["Company"] = new SelectList(idh.GetCompany(), "Id", "CompanyName");
            return PartialView("_PartialCreateDemo",ddh.GetDemoDetail().Find(x=>x.Id == id));
        }

        [HttpPost]
        public IActionResult CreateDemo(DemoMV vm)
        {
            DemoDbHandle ddh = new DemoDbHandle();
                if (ModelState.IsValid)
                {
                    if (ddh.CreateDemo(vm, UploadFile(vm.Attechment)))
                    {
                        return RedirectToAction("Index");

                    }
                }
            
            return RedirectToAction("Index");
        }

        public IActionResult GetCompanyDemo(int id)
        {
            CompanyDbHandle idh = new CompanyDbHandle();
            var result = idh.GetAllCompany(id);
            return Json(result);
        }

        public IActionResult Attachment(int id)
        {
            DemoDbHandle idh = new DemoDbHandle();
            return PartialView("_PartialAttachment", idh.GetAttachments(id));
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
