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
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=SupportDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

       
        public List<RegisterVM> GetUser()
        {
            connection();
            List<RegisterVM>  list= new List<RegisterVM>();
            SqlCommand cmd = new SqlCommand("select UserId, UserName from users where Profile = 'support'", con);
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
                    }
                    );
            }
            return list;
        }

        public ContactPersonVM Email(int id)
        {
            connection();
            ContactPersonVM c = new ContactPersonVM();
            SqlCommand cmd = new SqlCommand("select email from ContactPerson where ContactId = " + id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            con.Open();
            ad.Fill(dataTable);
            con.Close();
            foreach (DataRow dr in dataTable.Rows)
            {
                c.Email = Convert.ToString(dr["Email"]);
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
            SqlCommand cmd = new SqlCommand("select ContactId, ContactName, Email from ContactPerson where CompanyId = "+id, con);
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

                    }
                    );
            }
            return list;
        }

        public bool CreateIssue(IssueVM vm, List<string> ac)
        {
            connection();
            StringBuilder str = new StringBuilder();
            var createdDate = vm.CreatedDate.ToString("MM/dd/yyyy");
            //var resolveBy = vm.ResolveBy.ToString("MM/dd/yyyy");
            //var deleteBY = vm.DeletedBy.ToString("MM/dd/yyyy");
            //var closeBY = vm.CloseBy.ToString("MM/dd/yyyy");
           
            str.Append("declare @eid bigint \n");
            str.Append("declare @Newno varchar(20) \n");
            str.Append("declare @isno varchar(20) = 'IssNo' \n");
            //str.Append("declare @issupid int \n");
            str.Append("set @eid = (select isnull(max(IssueId), 0) + 1 from Issue) \n");
            str.Append("SELECT @Newno = @isno + RIGHT('0000000' + CAST(@eid AS VARCHAR(7)), 7) \n");
            //str.Append("set @issupid = (select isnull(max(IssueSupportId), 0) + 1 from IssueSupport) \n");
            if (vm.TrasferTo != null)
            {
                str.Append("insert into Issue(IssueId,IssueNo, IssueDescription, IssueGeneratorSteps, CreatedDate, Status,CloseDate, CompanyId,  ContactId,SupportId, TransferId) VALUES(@eid,@Newno,'" + vm.IssueDescription + "','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "','" + vm.CloseBy + "'," + vm.CompanyId + "," + vm.ContactId + "," + vm.AssignTo + "," + vm.TrasferTo + ") \n");
            }
            str.Append("insert into Issue(IssueId,IssueNo, IssueDescription, IssueGeneratorSteps, CreatedDate, Status,CloseDate, CompanyId,  ContactId,SupportId) VALUES(@eid,@Newno,'" + vm.IssueDescription + "','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "','" + vm.CloseBy + "'," + vm.CompanyId + "," + vm.ContactId + "," + vm.AssignTo + ") \n");

            //str.Append("insert into IssueSupport(IssueSupportId, Status, IssueId) VALUES(@issupid,'" + vm.Status + "',@eid) \n");
            str.Append("declare @aid bigint \n");
            foreach (var data in ac)
            {
                str.Append("set @aid = (select isnull(max(AttachmentId), 0) + 1 from Attachments) \n");
                str.Append("insert into Attachments(AttachmentId, AttachmentName, IssueId) values(@aid, '"+data+ "', @eid) \n");
            }
            
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open() ;
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                EmailSetting em = new EmailSetting();
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "", vm.ContactEmail, "Issue Created", "The Issue Hasbeen Created."));
                
               
                return true;
            }
            else
                return false;
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
                str.Append("update Issue set IssueDescription='" + vm.IssueDescription + "', IssueGeneratorSteps='" + vm.IssueGeneratorSteps + "', CreatedDate='" + createdDate + "', Status='" + vm.Status + "',CloseDate='" + vm.CloseBy + "', CompanyId=" + vm.CompanyId + ",  ContactId=" + vm.ContactId + ",SupportId=" + vm.AssignTo + " , TransferId= "+vm.TrasferTo+" where IssueId= " + vm.Id + " \n");

            }
            else
            {
                str.Append("update Issue set IssueDescription='" + vm.IssueDescription + "', IssueGeneratorSteps='" + vm.IssueGeneratorSteps + "', CreatedDate='" + createdDate + "', Status='" + vm.Status + "',CloseDate='" + vm.CloseBy + "', CompanyId=" + vm.CompanyId + ",  ContactId=" + vm.ContactId + ",SupportId=" + vm.AssignTo + " where IssueId= " + vm.Id + " \n");

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
                Task.Factory.StartNew(() => em.SendEmail(GetEmail().First(), "",vm.ContactEmail, "testing", "This is simple test body."));

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
            sb.Append(" Select i.IssueId, i.IssueNo,i.Status,i.IssueDescription,i.CompanyId,i.ContactId, u.UserName as support,u.UserId, i.TransferId, t.UserName as TrasferName , i.IssueGeneratorSteps,i.CreatedDate,Format(i.CloseDate,'yyyy-MM-dd')as CloseDate,  c.CompanyName,p.ContactId, p.ContactName,p.Email as ContactEmail, p.PhoneNumber from Issue i  join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId join users u on u.UserId = i.SupportId left join users t on t.UserId = i.TransferId where 1=1 ");

            //if (_httpContextAccessor.HttpContext.Session.GetString("userProfile").ToString().ToLower()!="superadmin" && _httpContextAccessor. HttpContext.Session.GetString("userProfile").ToString().ToLower() != "admin")
            //{
            //if (FromD != null || To != null || Status != null)
            //{
            //    sb.Append(" and i.status = '" + Status + "' or i.createddate between '" + FromD + "' and '" + To + "'\n");
            //}
            //else 
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
            //else if(FromD != null || To != null || Istatus != null)
            //{
            //    sb.Append(" and i.status = '" + Istatus + "' or i.createddate between '" + FromD + "' and '" + To + "'\n");

            //}

            if (session.GetString("userProfile") == "OMSUser")
            {
                sb.Append(" and i.userid='" + session.GetString("userId") + "'\n");
            }
            else if (session.GetString("userProfile") == "Support")
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
                        PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                        //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        ContactId = Convert.ToInt32(dr["ContactId"]),
                        ContactEmail = Convert.ToString(dr["ContactEmail"]),
                        TrasferTo = string.IsNullOrEmpty(dr["TransferId"].ToString()) ? null : Convert.ToInt32(dr["TransferId"].ToString()),
                        TrasferName = Convert.ToString(dr["TrasferName"]),
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
            sb.Append(" Select i.IssueId, i.IssueNo,i.Status,i.IssueDescription,i.CompanyId,i.ContactId, u.UserName as support,u.UserId, i.TransferId, t.UserName as TrasferName , i.IssueGeneratorSteps,i.CreatedDate,Format(i.CloseDate,'yyyy-MM-dd')as CloseDate,  c.CompanyName,p.ContactId, p.ContactName,p.Email as ContactEmail, p.PhoneNumber from Issue i  join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId join users u on u.UserId = i.SupportId left join users t on t.UserId = i.TransferId where 1=1 ");

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
                        PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                        //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        ContactId = Convert.ToInt32(dr["ContactId"]),
                        ContactEmail = Convert.ToString(dr["ContactEmail"]),
                        TrasferTo =string.IsNullOrEmpty( dr["TransferId"].ToString())?null: Convert.ToInt32(dr["TransferId"].ToString()),
                        TrasferName = Convert.ToString(dr["TrasferName"]),
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
