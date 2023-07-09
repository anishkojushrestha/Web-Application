using System.Data;
using System.Data.SqlClient;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class IssueActivityDbHandle
    {
         
        private SqlConnection con;
        private void connection()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string constring = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            con = new SqlConnection(constring);
        }
        HttpContextAccessor accessor = new HttpContextAccessor();
        ISession session => accessor.HttpContext.Session;
        
        public bool CreateIssueActivity(IssueActivityVM vm, List<string> ac)
        {
            connection();
            StringBuilder str = new StringBuilder();
            
            str.Append("declare @asid bigint \n");
            str.Append("set @asid = (select isnull(max(IssueActivityId), 0) + 1 from IssueActivity) \n");
            str.Append(" insert into IssueActivity(IssueActivityId, IssueDescription, IssueId) values(@asid,'"+vm.ActivityDescription+"',"+vm.Id+") \n");
            str.Append("declare @aid bigint \n");
            foreach (var data in ac)
            {
                str.Append("set @aid = (select isnull(max(AttachmentId), 0) + 1 from Attachments) \n");
                str.Append("insert into Attachments(AttachmentId, AttachmentName, IssueActivityId) values(@aid, '" + data + "', @asid) \n");
            }

            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool UpdateIssueActivity(IssueActivityVM vm)
        {
            connection();
            StringBuilder str = new StringBuilder();
            str.Append(" update IssueActivity set  IssueDescription = '"+vm.ActivityDescription+"' where IssueActivityId="+vm.Id+"  \n");
           
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public List<AttachmentVM> GetAttachmentsActivity(int id)
        {
            connection();
            List<AttachmentVM> list = new List<AttachmentVM>();
            SqlCommand cmd = new SqlCommand("select * from Attachments where IssueActivityId=" + id, con);
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
        public List<IssueActivityVM> GetIssuesActivity() {
            connection();
            List<IssueActivityVM> list = new List<IssueActivityVM>();
            StringBuilder str = new StringBuilder();
            str.Append("select ia.*,a.AttachmentName, i.IssueId, i.IssueNo,i.AssignedTo from IssueActivity ia join Attachments a on a.IssueActivityId = ia.IssueActivityId join Issue i on i.IssueId = ia.IssueId where 1=1 \n");
            if(session.GetString("userProfile") == "Support")
            {
                str.Append("and i.AssignedTo = " + session.GetString("userId") + " \n");
            }
            
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(
                    new IssueActivityVM()
                    {
                        Id = Convert.ToInt32(dr["IssueActivityId"]),
                        ActivityDescription = Convert.ToString(dr["IssueDescription"]),
                        AttachmentName = Convert.ToString(dr["AttachmentName"]),
                        IsseId = Convert.ToString(dr["IssueNo"]),
                    });
            }
            return list;
        }
    }
}
