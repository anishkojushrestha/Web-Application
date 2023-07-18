using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Application.Models;

namespace Web_Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

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
            ViewBag.TotalIssue = total;
            ViewBag.TotalAssign = totalAssgin;
            ViewBag.TotalTransfer = totalTransfer;
            ViewBag.Remain = remainning;
            return View();
        }
    }
}