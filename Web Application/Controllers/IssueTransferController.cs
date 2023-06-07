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
            
            
            return View(idh.GetIssueTransfer());
        }
        
        
    }
}
