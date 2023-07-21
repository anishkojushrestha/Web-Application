using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Application.Models;

namespace Web_Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        DateTime date = DateTime.Now;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            
            
            IssueDbHandle idh = new IssueDbHandle();
            var total = idh.FilterDate().Count();
            var totalAssgin = idh.FilterDate().Where(x=>x.Support != "").Count();
            var totalTransfer = idh.FilterDate().Where(x=>x.TrasferName != "").Count();
            var remainning = idh.FilterDate().Where(x=>x.Status != "Close").Count();
            var todayDue = idh.FilterDate().Where(x => x.CloseBy == date.ToString("yyyy-MM-dd")).Count();
            ViewBag.TotalIssue = total;
            ViewBag.TotalAssign = totalAssgin;
            ViewBag.TotalTransfer = totalTransfer;
            ViewBag.Remain = remainning;
            ViewBag.TodayDue = todayDue;
            return View();
        }

        public IActionResult TotalAssign()
        {
            IssueDbHandle idh = new IssueDbHandle();
            var totalAssgin = idh.FilterDate().Where(x => x.Support != "").ToList();
            return  Json(new { data= totalAssgin });
        }
        public IActionResult TotalTransfer()
        {
            IssueDbHandle idh = new IssueDbHandle();
            var totalTran = idh.FilterDate().Where(x => x.TrasferName != "").ToList();
            return  Json(new { data= totalTran });
        }
        public IActionResult TotalClose()
        {
            IssueDbHandle idh = new IssueDbHandle();
            var totalClose = idh.FilterDate().Where(x => x.Status != "Close").ToList();
            return  Json(new { data= totalClose });
        }
        public IActionResult TodayDue()
        {
            IssueDbHandle idh = new IssueDbHandle();
            var todayDue = idh.FilterDate().Where(x => x.CloseBy == date.ToString("yyyy-MM-dd")).ToList();
            return Json(new { data = todayDue });
        }


    }
}