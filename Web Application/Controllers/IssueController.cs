using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using Intersoft.Crosslight;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Web_Application.Models;
using Web_Application.ModelViews;
using Array = System.Array;

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
        public JsonResult GetReportDate(string DateType)
        {
            string sql = "";
            DateTime Startdate = DateTime.Now;
            DateTime Enddate = DateTime.Now;
            string FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            string ToDate = DateTime.Now.ToString("dd/MM/yyyy");

            if (DateType == "Today")
            {
                Startdate = DateTime.Now;
                Enddate = DateTime.Now;
            }
            else if (DateType == "Yesterday")
            {
                Startdate = DateTime.Now.AddDays(-1);
                Enddate = DateTime.Now.AddDays(-1);
            }
            else if (DateType == "Current Week")
            {
                Startdate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                Enddate = DateTime.Now.AddDays(6 - (int)DateTime.Now.DayOfWeek);
            }
            else if (DateType == "Last Week")
            {
                Enddate = DateTime.Now.AddDays(-1 - (int)DateTime.Now.DayOfWeek);
                Startdate = DateTime.Now.AddDays(-7 - (int)DateTime.Now.DayOfWeek);
            }
            else if (DateType == "Next Week")
            {
                Enddate = DateTime.Now.AddDays(13 - (int)DateTime.Now.DayOfWeek);
                Startdate = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek);
            }
            else if (DateType == "Current Month")//Current Month
            {
                //    if (DAL.GlobalSessionCls._DateMiti == 'D')
                //    {
                //        Startdate = Convert.ToDateTime(DAL.Database.GetSqlData("SELECT CONVERT(nvarchar(11),DATEADD(Month, DATEDIFF(Month, 0, GETDATE()), 0),23) "));
                //        Enddate = Convert.ToDateTime(DAL.Database.GetSqlData("Select CONVERT(nvarchar(11), DATEADD(month,1, DATEADD(Month, DATEDIFF(Month, 0, GETDATE()), -1)),23) "));  //DateTime.Now;
                //    }
                //    else
                //    {
                //        sql = " Select Top 1 M_Date From DateMiti Where Month_Name = (Select Distinct Month_Name from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'  ) ";
                //        sql = sql + " And Substring(M_Miti,7,4)= (Select Distinct Substring(M_Miti,7,4) MYear from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                //        sql = sql + " Order By M_Date asc ";
                //        Startdate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));
                //        sql = " Select Top 1 M_Date From DateMiti Where Month_Name = (Select Distinct Month_Name from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'  ) ";
                //        sql = sql + " And Substring(M_Miti,7,4)= (Select Distinct Substring(M_Miti,7,4) MYear from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                //        sql = sql + " Order By M_Date desc ";
                //        Enddate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));
                //    }
                //}
                //else if (DateType == "Last Month")//Last Month
                //{
                //    if (DAL.GlobalSessionCls._DateMiti == 'D')
                //    {
                //        string st = DAL.Database.GetSqlData("Select CONVERT(nvarchar(11), DATEADD(month,-1, DATEADD(Month, DATEDIFF(Month, 0, GETDATE()), 0)),23)");
                //        //Startdate = Convert.ToDateTime(DAL.Database.GetSqlData("Select CONVERT(nvarchar(11), DATEADD(month,-1, DATEADD(Month, DATEDIFF(Month, 0, GETDATE()), 0)),105)").ToString());
                //        string ED = DAL.Database.GetSqlData("Select CONVERT(nvarchar(11),DATEADD(Month, DATEDIFF(Month, 0, GETDATE()), -1),23) ");
                //        Enddate = Convert.ToDateTime(ED);
                //        Startdate = Convert.ToDateTime(st);
                //    }
                //    else
                //    {
                //        sql = "Select Top 1 M_Date from DateMiti Where Substring(M_Miti, 7, 4) = (Select Distinct Case When Convert(nvarchar(5),Convert(Numeric, Substring(M_Miti, 4, 2)) - 1)= 0 Then Convert(Numeric, Substring(M_Miti,7,4))-1 Else Substring(M_Miti,7,4) End MYear from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "') \n";
                //        sql = sql + " and(Len(Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) = 1 and Convert(Numeric, Substring(M_Miti, 4, 2)) = (Select Distinct Case When Convert(nvarchar(5), Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) = 0 Then 12 Else '0' + Convert(nvarchar(5), Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) End from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "') \n";
                //        sql = sql + " or Len(Convert(Numeric, Substring(M_Miti,4,2))-1)<> 1 and Convert(Numeric, Substring(M_Miti,4,2))= (Select Distinct Case When Convert(nvarchar(5),Convert(Numeric, Substring(M_Miti, 4, 2)) - 1)= 0 Then 12 Else Convert(nvarchar(5),Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) End from DateMiti Where M_Date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ) \n";
                //        sql = sql + " Order By M_Date asc \n";
                //        Startdate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));
                //        sql = "Select Top 1 M_Date from DateMiti Where Substring(M_Miti,7,4)=(Select Distinct Case When Convert(nvarchar(5),Convert(Numeric, Substring(M_Miti, 4, 2)) - 1)= 0 Then Convert(Numeric, Substring(M_Miti,7,4))-1 Else Substring(M_Miti,7,4) End MYear from DateMiti Where M_Date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') \n";
                //        sql = sql + " and (Len(Convert(Numeric,Substring(M_Miti,4,2))-1)=1 and Convert(Numeric,Substring(M_Miti,4,2))=(Select Distinct Case When Convert(nvarchar(5), Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) = 0 Then 12 Else '0'+Convert(nvarchar(5),Convert(Numeric,Substring(M_Miti,4,2))-1) End from DateMiti Where M_Date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') \n";
                //        sql = sql + " or Len(Convert(Numeric,Substring(M_Miti,4,2))-1)<>1 and Convert(Numeric,Substring(M_Miti,4,2))=(Select Distinct Case When Convert(nvarchar(5), Convert(Numeric, Substring(M_Miti, 4, 2)) - 1) = 0 Then 12 Else Convert(nvarchar(5),Convert(Numeric,Substring(M_Miti,4,2))-1) End from DateMiti Where M_Date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') ) \n";
                //        sql = sql + " Order By M_Date desc \n";
                //        Enddate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));

                //        //commented not return true value of deduct -30 days
                //        //sql = "Select Top(1) M_Date from DateMiti where Substring(M_Miti,7,4) =";
                //        //sql = sql + " (Select substring(M_Miti, 7, 4) from  DateMiti where  Convert(Varchar(10), M_Date, 103) = Convert(Varchar(10), DATEADD(day, -30, getdate()), 103))";
                //        //sql = sql + " and Month_Name = (Select Month_Name from DateMiti where  Convert(Varchar(10),M_Date,103)= Convert(Varchar(10), DATEADD(day, -30, getdate()), 103) ) ";
                //        //sql = sql + " Order by M_Date ";
                //        //Startdate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));
                //        //sql = "Select Top(1) M_Date from DateMiti where Substring(M_Miti,7,4) =";
                //        //sql = sql + " (Select substring(M_Miti, 7, 4) from  DateMiti where  Convert(Varchar(10), M_Date, 103) = Convert(Varchar(10), DATEADD(day, -30, getdate()), 103))";
                //        //sql = sql + " and Month_Name = (Select Month_Name from DateMiti where  Convert(Varchar(10),M_Date,103)= Convert(Varchar(10), DATEADD(day, -30, getdate()), 103) ) ";
                //        //sql = sql + " Order by M_Date Desc ";
                //        //Enddate = Convert.ToDateTime(DAL.Database.GetSqlData(sql));
                //    }
                //}
                //else if (DateType == "Upto Date")
                //{
                //    Startdate = Convert.ToDateTime(DateTime.ParseExact(DAL.GlobalSessionCls._CompanyStartDate, "dd/MM/yyyy", null));// Convert.ToDateTime(DAL.GlobalSessionCls._CompanyStartDate);
                //    Enddate = DateTime.Now;
                //}
                //else if (DateType == "Accounting Period")
                //{
                //    Startdate = Convert.ToDateTime(DateTime.ParseExact(DAL.GlobalSessionCls._CompanyStartDate, "dd/MM/yyyy", null));// Convert.ToDateTime(DAL.GlobalSessionCls._CompanyStartDate);
                //    Enddate = Convert.ToDateTime(DateTime.ParseExact(DAL.GlobalSessionCls._CompanyEnddate, "dd/MM/yyyy", null));// Convert.ToDateTime(DAL.GlobalSessionCls._CompanyEnddate);
                //}
                //else if (DateType == "Accounting Period")
                //{
                //    Startdate = Convert.ToDateTime(DateTime.ParseExact(DAL.GlobalSessionCls._CompanyStartDate, "dd/MM/yyyy", null));// Convert.ToDateTime(DAL.GlobalSessionCls._CompanyStartDate);
                //    Enddate = Convert.ToDateTime(DateTime.ParseExact(DAL.GlobalSessionCls._CompanyEnddate, "dd/MM/yyyy", null));// Convert.ToDateTime(DAL.GlobalSessionCls._CompanyEnddate);

                //}

                //if (DAL.GlobalSessionCls._DateMiti == 'D')
                //{
               
                //}
                //else
                //{
                //    FromDate = DAL.Database.GetSqlData("Select m_miti from datemiti where m_date='" + Startdate.ToString("yyyy/MM/dd") + "'");
                //    ToDate = DAL.Database.GetSqlData("Select m_miti from datemiti where m_date='" + Enddate.ToString("yyyy/MM/dd") + "'");
                //}


            }
            FromDate = Startdate.ToString("yyyy-MM-dd");
            ToDate = Enddate.ToString("yyyy-MM-dd");
            List<string> arrayData = new List<string>();
            arrayData.Add(FromDate);
            arrayData.Add(ToDate);

            Array convertedDate = arrayData.ToArray();
            return Json(convertedDate);
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
