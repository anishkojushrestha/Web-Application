using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Application.Models;
using Web_Application.ModelViews;

namespace Web_Application.Controllers
{
    public class IssueTransferController : Controller
    {
        public IActionResult Index()
        {
            IssueDbHandle idh = new IssueDbHandle();
            ViewData["Assign"] = new SelectList(idh.GetUser(), "UserName", "UserName");
            ViewData["Issue"] = new SelectList(idh.GetIssue(), "Id", "IssueNo");
            return View(idh.GetIssueTransfer());
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
    }
}
