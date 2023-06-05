using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class IssueDbHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=WebAppDB;Integrated Security=True;Pooling=False";
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
        public List<ContactPersonVM> GetContact(int id)
        {
            connection();
            List<ContactPersonVM> list = new List<ContactPersonVM>();
            SqlCommand cmd = new SqlCommand("select ContactId, ContactName from ContactPerson where CompanyId = "+id, con);
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
            str.Append("set @eid = (select isnull(max(IssueId), 0) + 1 from Issue) \n");
            str.Append("insert into Issue(IssueId,IssueDescription, IssueGeneratorSteps, CreatedDate, Status, CompanyId, UserId, ContactId) VALUES(@eid,'"+vm.IssueDescription +"','" + vm.IssueGeneratorSteps + "','" + createdDate + "','" + vm.Status + "',"+vm.CompanyId+","+vm.AssignTo+","+vm.ContactId+" ) \n");
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
            if(i>=1)
                return true;
            else
                return false;
        }
        public bool Resolve(IssueVM vm)
        {
            connection();
            SqlCommand cmd = new SqlCommand("update Issue set ResolveBy = '"+vm.ResolveBy+"' where IssueId="+vm.Id, con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool DeletedBy(int id)
        {
            connection();
            string now = DateTime.Now.ToShortDateString();
            SqlCommand cmd = new SqlCommand("update Issue set DeletedBy = '" + now + "' where IssueId=" + id, con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<IssueVM> GetIssue()
        {
            connection();
            List<IssueVM> list = new List<IssueVM>();
            SqlCommand cmd = new SqlCommand("Select i.*, U.UserName, c.CompanyName, p.ContactName, p.PhoneNumber from Issue i join users u on u.UserId = i.UserId join CompanyInfo c on c.CompanyId = i.CompanyId join ContactPerson p on p.ContactId = i.ContactId", con);
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
                        //IssueGeneratorSteps = Convert.ToString(dr["IssueGeneratorSteps"]),
                        IssueNo = Convert.ToString(dr["IssueNo"]),
                        CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                        Status = Convert.ToString(dr["Status"]),
                        ResolveBy = Convert.ToString(dr["ResolveBy"]),
                        DeletedBy = Convert.ToString(dr["DeletedBy"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),


                    });
            }
            return list;
        }

    }
}
