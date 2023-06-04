using System.Data;
using System.Data.SqlClient;
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

        public bool CreateIssue(IssueVM vm)
        {
            connection();
            StringBuilder str = new StringBuilder();
            var createdDate = vm.CreatedDate.ToString("MM/dd/yyyy");
            var resolveBy = vm.ResolveBy.ToString("MM/dd/yyyy");
            var deleteBY = vm.DeletedBy.ToString("MM/dd/yyyy");
            var closeBY = vm.CloseBy.ToString("MM/dd/yyyy");
            str.Append("declear @isid bigint \n");
            str.Append("set @isid = (select isnull(max(userid), 0) + 1 from users) \n");
            str.Append("insert into Issue(IssueId, IssueNo, IssueCreatorName, IssueGeneratorSteps, Attachments, CreatedDate, Status, ResolveBy, DeleteBy, CloseBy) VALUES(@isid,'" + vm.IssueCreaterName + "','" + vm.IssueGenerationSteps + "','" + vm.Attachments + "','" + createdDate + "','" + vm.Status + "','" + resolveBy + "','" + deleteBY + "','" + closeBY + "' ) \n");
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open() ;
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if(i>=1)
                return true;
            else
                return false;
        }
    }
}
