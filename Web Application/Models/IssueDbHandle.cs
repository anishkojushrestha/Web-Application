using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using Web_Application.ModelViews;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Paddings;
using Microsoft.Extensions.Primitives;

namespace Web_Application.Models
{
    public class IssueDbHandle
    {
        private SqlConnection con;

        HttpContextAccessor accessor = new HttpContextAccessor();
        ISession session => accessor.HttpContext.Session;

        private void connection()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string constring = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            con = new SqlConnection(constring);
        }

       
        public List<RegisterVM> GetUser()
        {
            connection();
            List<RegisterVM>  list= new List<RegisterVM>();
            StringBuilder str = new StringBuilder();
            str.Append("select UserId, UserName from users where Profile = 'support' \n"); 
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);  
            DataTable dt = new DataTable();
            con.Open();
            adapter.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows) {
                list.Add(
                    new RegisterVM()
                    {
                        Id = Convert.ToInt32(dr["UserId"]),
                        UserName = Convert.ToString(dr["Username"]),
                    });
            }
            return list;
        }
        public int maxId()
        {
            connection();
            int ticket=0;
            SqlCommand cmd = new SqlCommand("select max(IssueId) as ticket from Issue", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach(DataRow dr in dataTable.Rows)
            {
                ticket = string.IsNullOrEmpty(dr["ticket"].ToString()) ? 0 : Convert.ToInt32(dr["ticket"]);
            }
            return ticket;
        }

        public ContactPersonVM Email(int id)
        {
            connection();
            ContactPersonVM c = new ContactPersonVM();
            SqlCommand cmd = new SqlCommand("select email,Designation from ContactPerson where ContactId = " + id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach (DataRow dr in dataTable.Rows)
            {
                c.Email = Convert.ToString(dr["Email"]);
                c.Designation = Convert.ToString(dr["Designation"]);
            }
            return c;

        }
        public IssueVM UserEmail(int id,string username)
        {
            connection();
            IssueVM c = new IssueVM();
            StringBuilder str = new StringBuilder();
            str.Append("select email from users where 1=1\n");
            if(username == null)
            {
                str.Append("and UserId = " + id + "\n");
            }
            else { 
                str.Append("and Username = '" + username+"'\n");
            }

            SqlCommand cmd = new SqlCommand(str.ToString() , con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach (DataRow dr in dataTable.Rows)
            {
                c.AssignedEmail = Convert.ToString(dr["Email"]);
            }

            return c;
        }

        public string AssignedEmail(int id)
        {
            connection();
            string email = "";
            SqlCommand cmd = new SqlCommand("select Email from users where UserId="+id+"", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach (DataRow dr in dataTable.Rows)
            {
                email = Convert.ToString(dr["Email"]);
            }
            return email;
        }
        public RegisterVM Transfer(int id)
        {
            connection();
            RegisterVM c = new RegisterVM();
            SqlCommand cmd = new SqlCommand("select u.UserName from Issue i join users u on u.UserId = i.UserId where IssueID = " + id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach (DataRow dr in dataTable.Rows)
            {
                c.UserName = Convert.ToString(dr["UserName"]);
            }

            return c;

        }

        public List<ContactPersonVM> GetContact(int id)
        {
            connection();
            List<ContactPersonVM> list = new List<ContactPersonVM>();
            //SqlCommand cmd = new SqlCommand("select ContactId, ContactName, Email from ContactPerson where CompanyId = "+id, con);
            SqlCommand cmd = new SqlCommand("select p.ContactId, p.ContactName, p.Email,c.CompanyName from ContactPerson p join CompanyInfo c on c.CompanyId = p.CompanyId  where p.CompanyId = " + id, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            adapter.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new ContactPersonVM()
                    {
                        Id = Convert.ToInt32(dr["ContactId"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        Email = Convert.ToString(dr["Email"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),  
                    });
            }
            return list;
        }

        public bool CreateIssue(IssueVM vm, List<string> ac)
        {
            connection();
            StringBuilder str = new StringBuilder();
            var createdDate = vm.CreatedDate.ToString("MM/dd/yyyy");
            str.Append("declare @eid bigint \n");
            str.Append("declare @Newno varchar(20) \n");
            str.Append("declare @isno varchar(20) = 'IssNo' \n");
            //str.Append("declare @issupid int \n");
            str.Append("set @eid = (select isnull(max(IssueId), 0) + 1 from Issue) \n");
            str.Append("SELECT @Newno = @isno + RIGHT( CAST(@eid AS VARCHAR(7)), 7) \n");
            //str.Append("set @issupid = (select isnull(max(IssueSupportId), 0) + 1 from IssueSupport) \n");

            if (vm.TrasferTo != null)
            {
                str.Append("insert into Issue(IssueId,IssueNo,SupportType, IssueDescription, IssueGeneratorSteps, CreatedDate, Status,CloseDate, CompanyId,  ContactId,SupportId, TransferId) VALUES(@eid,'" + vm.IssueNo + "','" + vm.SupportType + "','" + vm.IssueDescription + "','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "','" + vm.CloseBy + "'," + vm.CompanyId + "," + vm.ContactId + "," + vm.AssignTo + "," + vm.TrasferTo + ") \n");
            }
            else if (vm.CloseBy != null)
            {
                str.Append("insert into Issue(IssueId,IssueNo,SupportType, IssueDescription, IssueGeneratorSteps, CreatedDate, Status,CloseDate, CompanyId,  ContactId,SupportId) VALUES(@eid,'" + vm.IssueNo + "','" + vm.SupportType + "','" + vm.IssueDescription + "','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "','" + vm.CloseBy + "'," + vm.CompanyId + "," + vm.ContactId + "," + vm.AssignTo + ") \n");
            }
            else
            {
                str.Append("insert into Issue(IssueId,IssueNo,SupportType, IssueDescription, IssueGeneratorSteps, CreatedDate, Status, CompanyId,  ContactId,SupportId) VALUES(@eid,'" + vm.IssueNo+"','"+vm.SupportType+"','" + vm.IssueDescription + "','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "'," + vm.CompanyId + "," + vm.ContactId + "," + vm.AssignTo + ") \n");
            }
            //str.Append("insert into IssueSupport(IssueSupportId, Status, IssueId) VALUES(@issupid,'" + vm.Status + "',@eid) \n");
            str.Append("declare @aid bigint \n");
            foreach (var data in ac)
            {
                str.Append("set @aid = (select isnull(max(AttachmentId), 0) + 1 from Attachments) \n");
                str.Append("insert into Attachments(AttachmentId, AttachmentName, IssueId) values(@aid, '"+data+ "', @eid) \n");
            }
            
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                EmailSetting em = new EmailSetting();
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.ContactEmail, "Issue Generated", "Ticket NO. "+vm.IssueNo+ "\n\nIssue Date: " + vm.CreatedDate.ToString("MM/dd/yyyy")+ " \n\n Client Name: " + vm.CompanyName+ " \n\nContact Person: "+vm.ContactName+"\n\n Designation: "+vm.Designation+" \n\nSupport Type:"+vm.SupportType+" \n\nIssue: Reinstall."));
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.AssignedEmail, "Issue Assigend", "Ticket NO. "+vm.IssueNo+ "\n\nIssue Date: " + vm.CreatedDate.ToString("MM/dd/yyyy")+ " \n\n Client Name: " + vm.CompanyName+ " \n\nContact Person: "+vm.ContactName+"\n\n Designation: "+vm.Designation+" \n\nSupport Type:"+vm.SupportType+" \n\nIssue: Reinstall."));
                return true;
            }
            else
                return false;
        }

        public string Min()
        {
            connection();
            string minimum ="";
            SqlCommand cmd = new SqlCommand("select min(CreatedDate) as minimum from issue", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();    
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                minimum = Convert.ToString(dr["minimum"]);
                //to = Convert.ToString(dr["maximum"]);

            }
            return minimum;
        }
        public string Max()
        {
            connection();
            string to = "";
            SqlCommand cmd = new SqlCommand("select max(CreatedDate) as maximum from issue", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                //from = Convert.ToString(dr["minimum"]);
                to = Convert.ToString(dr["maximum"]);

            }
            return to;
        }

        public bool UpdateIssue(IssueVM vm, List<string> ac)
        {
            connection();
            StringBuilder str = new StringBuilder();
            var createdDate = vm.CreatedDate.ToString("MM/dd/yyyy");
            str.Append("declare @eid bigint \n");
            //str.Append("declare @isno int \n");
            str.Append("declare @issupid int \n");
            str.Append("set @eid = (select isnull(max(IssueId), 0) + 1 from Issue) \n");
            //str.Append("set @isno = (select isnull(max(IssueNo), 0) + 1 from Issue) \n");
            //str.Append("set @issupid = (select isnull(max(IssueSupportId), 0) + 1 from IssueSupport) \n");
            if(vm.TrasferTo != null )
            {
                str.Append("update Issue set IssueDescription='" + vm.IssueDescription + "', SupportType='"+vm.SupportType+"', IssueGeneratorSteps='" + vm.IssueGeneratorSteps + "', CreatedDate='" + createdDate + "', Status='" + vm.Status + "',CloseDate='" + vm.CloseBy + "', CompanyId=" + vm.CompanyId + ",  ContactId=" + vm.ContactId + ",SupportId=" + vm.AssignTo + " , TransferId= "+vm.TrasferTo+" where IssueId= " + vm.Id + " \n");
            }
            else
            {
                str.Append("update Issue set IssueDescription='" + vm.IssueDescription + "',SupportType='"+vm.SupportType+"', IssueGeneratorSteps='" + vm.IssueGeneratorSteps + "', CreatedDate='" + createdDate + "', Status='" + vm.Status + "', CompanyId=" + vm.CompanyId + ",  ContactId=" + vm.ContactId + ",SupportId=" + vm.AssignTo + " where IssueId= " + vm.Id + " \n");
            }
            if(vm.CloseBy != null)
            {
                str.Append("update Issue set IssueDescription='" + vm.IssueDescription + "',SupportType='"+vm.SupportType+"', IssueGeneratorSteps='" + vm.IssueGeneratorSteps + "', CreatedDate='" + createdDate + "', Status='" + vm.Status + "',CloseDate='" + vm.CloseBy + "', CompanyId=" + vm.CompanyId + ",  ContactId=" + vm.ContactId + ",SupportId=" + vm.AssignTo + " where IssueId= " + vm.Id + " \n");

            }
            //str.Append("insert into IssueSupport(IssueSupportId, Status, IssueId) VALUES(@issupid,'" + vm.Status + "',"+vm.Id+") \n");
            str.Append("declare @aid bigint \n");
            foreach (var data in ac)
            {
                str.Append("set @aid = (select isnull(max(AttachmentId), 0) + 1 from Attachments) \n");
                str.Append("insert into Attachments(AttachmentId, AttachmentName, IssueId) values(@aid, '" + data + "', " + vm.Id + ") \n");
            }
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                EmailSetting em = new EmailSetting();
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.ContactEmail, "Issue Updated", "Ticket NO. " + vm.IssueNo + "\n\nIssue Date: " + vm.CreatedDate.ToString("MM/dd/yyyy") + " \n\n Client Name: " + vm.CompanyName + " \n\nContact Person: " + vm.ContactName + "\n\n Designation: "+vm.Designation+" \n\nSupport Type:" + vm.SupportType + " \n\nIssue: Reinstall."));
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.AssignedEmail, "Issue Assigend", "Ticket NO. " + vm.IssueNo + "\n\nIssue Date: " + vm.CreatedDate.ToString("MM/dd/yyyy") + " \n\n Client Name: " + vm.CompanyName + " \n\nContact Person: " + vm.ContactName + "\n\n Designation: "+vm.Designation+" \n\nSupport Type:" + vm.SupportType + " \n\nIssue: Reinstall."));

                return true;
            }
            else
                return false;
        }
        
        public bool Assigned(IssueVM vm)
        {
            connection();
            StringBuilder str= new StringBuilder();
            str.Append("update Issue set AssignedDate = '" + vm.AssignedDate + "', AssignedTo = "+vm.AssignTo+" where IssueId=" + vm.Id+" \n");
            str.Append("update IssueSupport set AssignedDate = '" + vm.AssignedDate + "', AssignedTo = " + vm.AssignTo + " where IssueId=" + vm.Id + " \n");
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                EmailSetting em = new EmailSetting();
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.AssignedEmail, "testing", "Task hasbeen assigned to you"));
                return true;
            }
            else
            {
                return false;
            }
        }
        

        public List<AttachmentVM> GetAttachments(int id)
        {
            connection();
            List<AttachmentVM> list = new List<AttachmentVM>();
            SqlCommand cmd = new SqlCommand("select * from Attachments where IssueId="+id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new AttachmentVM()
                    {
                        Id = Convert.ToInt32(dr["AttachmentId"]),
                        AttachmentName = Convert.ToString(dr["AttachmentName"]),
                    });
            }
            return list;
        }

        public List<IssueSupportVM> GetIssueSupport()
        {
            connection();
            List<IssueSupportVM> list = new List<IssueSupportVM>();
            SqlCommand cmd = new SqlCommand("select s.IssueSupportId,s.IssueId,Format(s.AssignedDate,'dd-MM-yyyy')as AssignedDate,s.Status,s.AssignedTo, i.IssueNo, u.UserName, c.ContactName from IssueSupport s left join users u on u.UserId = s.AssignedTo join Issue i on i.IssueId = s.IssueId join ContactPerson c on c.ContactId = i.ContactId", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new IssueSupportVM() 
                    {
                        Id = Convert.ToInt32(dr["IssueSupportId"]),
                        Status = Convert.ToString(dr["Status"]),
                        AssignedTo = Convert.ToString(dr["UserName"]),
                        IssueNo = Convert.ToString(dr["IssueNo"]),
                        AssignedDate = Convert.ToString(dr["AssignedDate"]),
                        AssignedBy = Convert.ToString(dr["ContactName"]),
                    });
            }
            return list;
        }
        public List<IssueVM> FilterDate(string Istatus=null, string FromD=null, string To=null)
        {
            connection();
            List<IssueVM> list = new List<IssueVM>();
            StringBuilder sb = new StringBuilder();
            //SessionHandler sd = new SessionHandler();
            sb.Append(" Select i.IssueId,i.SupportType,DATEDIFF(day, i.CreatedDate, ISNULL(i.CloseDate, GETDATE())  ) AS count, i.IssueNo,i.Status,i.IssueDescription,i.CompanyId,i.ContactId, u.UserName as support,u.UserId, i.TransferId, t.UserName as TrasferName , i.IssueGeneratorSteps,Format(i.CreatedDate,'yyyy-MM-dd') as CreatedDate,Format(i.CloseDate,'yyyy-MM-dd')as CloseDate,  c.CompanyName,p.ContactId, p.ContactName,p.Email as ContactEmail, p.PhoneNumber from Issue i  join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId join users u on u.UserId = i.SupportId left join users t on t.UserId = i.TransferId where 1=1 ");

            //if (_httpContextAccessor.HttpContext.Session.GetString("userProfile").ToString().ToLower()!="superadmin" && _httpContextAccessor. HttpContext.Session.GetString("userProfile").ToString().ToLower() != "admin")
            //{
            
            if (!string.IsNullOrEmpty(FromD))
            {
                sb.Append(" and  i.createddate >= '" + FromD + "'\n");  
            }

            if (!string.IsNullOrEmpty(To))
            {
                sb.Append(" and  i.createddate <= '" + To + "'\n");
            }

            if (!string.IsNullOrEmpty(Istatus))
            {
                if (Istatus != "All")
                {
                    sb.Append(" and  i.status = '" + Istatus + "'\n");
                }
            }
            

            //if (session.GetString("userProfile") == "OMSUser")
            //{
            //    sb.Append(" and i.userid='" + session.GetString("userId") + "'\n");
            //}
            //else if (session.GetString("userProfile") == "Support")
            //{
            //    sb.Append(" and i.Support='" + session.GetString("userId") + "'\n");
            //}
            //}

            SqlCommand cmd = new SqlCommand(sb.ToString(), con);
            //SqlCommand cmd = new SqlCommand("Select i.*,  c.CompanyName, p.ContactName, p.PhoneNumber from Issue i join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new IssueVM()
                    {
                        Id = Convert.ToInt32(dr["IssueId"]),                                    
                        IssueGeneratorSteps = Convert.ToString(dr["IssueGeneratorSteps"]),
                        IssueDescription = Convert.ToString(dr["IssueDescription"]),
                        IssueNo = Convert.ToString(dr["IssueNo"]),
                        Created = Convert.ToString(dr["CreatedDate"]),
                        Status = Convert.ToString(dr["Status"]),
                        //ResolveBy = Convert.ToString(dr["ResolveBy"]),
                        CloseBy = Convert.ToString(dr["CloseDate"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        Support = Convert.ToString(dr["support"]),
                        AssignTo = Convert.ToInt32(dr["UserId"]),
                        PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                        //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        ContactId = Convert.ToInt32(dr["ContactId"]),
                        ContactEmail = Convert.ToString(dr["ContactEmail"]),
                        TrasferTo = string.IsNullOrEmpty(dr["TransferId"].ToString()) ? null : Convert.ToInt32(dr["TransferId"]),
                        TrasferName = Convert.ToString(dr["TrasferName"]),
                        Count = Convert.ToInt32(dr["count"]),
                        SupportType = Convert.ToString(dr["SupportType"]),
                        //AssignedEmail = Convert.ToString(dr["AssignedEmail"]),
                    });
            }
            return list;
        }
        public List<IssueVM> GetIssue()
        {
            connection();
            List<IssueVM> list = new List<IssueVM>();
            StringBuilder sb = new StringBuilder();
            //SessionHandler sd = new SessionHandler();
            sb.Append(" Select i.IssueId, i.IssueNo,i.Status,i.IssueDescription,i.SupportType,i.CompanyId,i.ContactId, u.UserName as support,u.UserId, i.TransferId, t.UserName as TrasferName , i.IssueGeneratorSteps,i.CreatedDate,Format(i.CloseDate,'yyyy-MM-dd')as CloseDate,  c.CompanyName,p.ContactId, p.ContactName,p.Email as ContactEmail, p.PhoneNumber from Issue i  join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId join users u on u.UserId = i.SupportId left join users t on t.UserId = i.TransferId where 1=1 ");

            //if (_httpContextAccessor.HttpContext.Session.GetString("userProfile").ToString().ToLower()!="superadmin" && _httpContextAccessor. HttpContext.Session.GetString("userProfile").ToString().ToLower() != "admin")
            //{
            
            if (session.GetString("userProfile") == "OMSUser") {
                sb.Append(" and i.userid='" + session.GetString("userId") + "'\n");
            }else if(session.GetString("userProfile") == "Support")
            {
                sb.Append(" and i.Support='" + session.GetString("userId") + "'\n");
            }
            //}

            SqlCommand cmd = new SqlCommand(sb.ToString(), con);
            //SqlCommand cmd = new SqlCommand("Select i.*,  c.CompanyName, p.ContactName, p.PhoneNumber from Issue i join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new IssueVM()
                    {
                        Id = Convert.ToInt32(dr["IssueId"]),
                        IssueGeneratorSteps = Convert.ToString(dr["IssueGeneratorSteps"]),
                        IssueDescription = Convert.ToString(dr["IssueDescription"]),
                        IssueNo = Convert.ToString(dr["IssueNo"]),
                        CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                        Status = Convert.ToString(dr["Status"]),
                        //ResolveBy = Convert.ToString(dr["ResolveBy"]),
                        CloseBy = Convert.ToString(dr["CloseDate"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        Support = Convert.ToString(dr["support"]),
                        AssignTo = Convert.ToInt32(dr["UserId"]),
                        PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                        //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        ContactId = Convert.ToInt32(dr["ContactId"]),
                        ContactEmail = Convert.ToString(dr["ContactEmail"]),
                        TrasferTo =string.IsNullOrEmpty( dr["TransferId"].ToString())?null: Convert.ToInt32(dr["TransferId"].ToString()),
                        TrasferName = Convert.ToString(dr["TrasferName"]),
                        SupportType = Convert.ToString(dr["SupportType"]),
                        //AssignedEmail = Convert.ToString(dr["AssignedEmail"]),
                    });
            }
            return list;
        }
        public bool DeleteIssue(int id)
        {
            connection();
            StringBuilder str = new StringBuilder();
            str.Append(" Delete FROM Attachments WHERE IssueId =" + id + " \n");
            str.Append(" Delete from Issue where IssueId = " + id + "\n");
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool CreateIssueTransfer(IssueTransferVM vm)
        {
            connection();
            var date = vm.TransferDate.ToString("MM/dd/yyyy");
            StringBuilder str = new StringBuilder();
            str.Append("declare @itid int \n");
            str.Append("set @itid = (select isnull(max(IssueTransferId), 0) + 1 from IssueTransfer) \n");
            str.Append("insert into IssueTransfer(IssueTransferId, TransferedFrom, TransferedTo, TransferedDate, CurrentStage, IssueId) values(@itid,'"+vm.TransferFrom+"','"+vm.TransferTo+"','"+ date + "','"+vm.CurrentStage+"',"+vm.Id+")\n");
            SqlCommand cmd = new SqlCommand(str.ToString(),con);
            con.Open();
            var i = cmd.ExecuteNonQuery();  
            con.Close();

            if (i >= 1)
            {
                EmailSetting em = new EmailSetting();
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.AssignedEmail, "testing", "Task hasbeen trasfer to you"));
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<IssueTransferVM> GetIssueTransfer()
        {
            connection();
            List<IssueTransferVM> list = new List<IssueTransferVM>();
            SqlCommand cmd = new SqlCommand("select t.*, i.IssueNo from IssueTransfer t join Issue i on i.IssueId = t.IssueId", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new IssueTransferVM()
                    {
                        Id = Convert.ToInt32(dr["IssueTransferId"]),
                        TransferFrom = Convert.ToString(dr["TransferedFrom"]),
                        TransferTo = Convert.ToString(dr["TransferedTo"]),
                        TransferDate = Convert.ToDateTime(dr["TransferedDate"]),
                        CurrentStage = Convert.ToString(dr["CurrentStage"]),
                        IssueId = Convert.ToString(dr["issueNo"]),
                    });
            }
            return list;
        }

        public List<EmailSetupVM> GetEmail()
        {
            connection();
            List<EmailSetupVM> list = new List<EmailSetupVM>();               
            SqlCommand cmd = new SqlCommand("select * from EmailSetting",con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new EmailSetupVM()
                    {
                        EmailId = Convert.ToString(dr["EmailId"]),
                        EmailPws = Convert.ToString(dr["EmailPws"]),
                        PORT = Convert.ToString(dr["PORT"]),
                        SMTP = Convert.ToString(dr["SMTP"]),
                    });
            }
            return list;
        }

        public List<CompanyMV> GetCompanyUser()
        {
            connection();
            List<CompanyMV> com = new List<CompanyMV>();
            StringBuilder str = new StringBuilder();
            //str.append("select c.companyid,c.companyname from users u join companyinfo c on c.companyid =u.companyid where 1=1\n");
            str.Append("SELECT c.CompanyId,c.CompanyName FROM CompanyInfo c\n");
            if (session.GetString("userProfile") == "OMSUser")
            {
                if (session.GetString("userId") != null)
                {
                    str.Append(" and u.UserId = " + session.GetString("userId") + "  \n");
                }
            }
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach(DataRow dr in dt.Rows)
            {
                com.Add(
                    new CompanyMV
                    {
                      Id = Convert.ToInt32(dr["CompanyId"]),
                      CompanyName = Convert.ToString(dr["CompanyName"]),
            });  
            }
            return com;
        }
    }
}
