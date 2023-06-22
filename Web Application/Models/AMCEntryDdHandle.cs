using System.Data;
using System.Data.SqlClient;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class AMCEntryDdHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=SupportDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }
        public List<AMCEntryVM> GetAMCDetail()
        {
            connection();
            List<AMCEntryVM> list = new List<AMCEntryVM>();
            SqlCommand cmd = new SqlCommand("select a.*,c.CompanyName from AMCEntry a join CompanyInfo c on c.CompanyId = a.CompanyId", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable st = new DataTable();
            con.Open();
            adapter.Fill(st);
            con.Close();
            foreach (DataRow dr in st.Rows)
            {
                list.Add(
                    new AMCEntryVM
                    {
                        Id = Convert.ToInt32(dr["AMCEntryId"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        Client = Convert.ToString(dr["Client"]),
                        OpenDate = Convert.ToDateTime(dr["OpenDate"]),
                        AMCAmount = Convert.ToInt32(dr["AMCAmount"]),
                        FollowUpDate = Convert.ToDateTime(dr["FollowUpDate"]),
                        CloseDate = Convert.ToDateTime(dr["CloseDate"]),
                    }
                );
            }
            return list;
        }
        public bool CreateAMC(AMCEntryVM vm)
        {
            connection();
            var openDate = vm.OpenDate.ToString("MM/dd/yyyy");
            var closeDate = vm.CloseDate.ToString("MM/dd/yyyy");
            var follow = vm.FollowUpDate.ToString("MM/dd/yyyy");
            StringBuilder str = new StringBuilder();
            str.Append(" declare @amcid bigint \n");
            str.Append(" set @amcid = (select isnull(max(AMCEntryId), 0) + 1 from AMCEntry) \n");
            if (vm.Id != null)
            {
                str.Append("update AMCEntry set  Client='"+vm.Client+"', OpenDate='"+openDate + "', AMCAmount="+vm.AMCAmount+", FollowUpDate='"+ follow + "', CloseDate='"+ closeDate + "' where AMCEntryId =" + vm.Id + " \n");
            }
            else
            {

                str.Append("insert into AMCEntry(AMCEntryId, Client, OpenDate, AMCAmount, FollowUpDate, CloseDate, CompanyId) values(@amcid, '" + vm.Client + "','" + openDate + "'," + vm.AMCAmount + ",'" + follow + "','" + closeDate + "'," + vm.CompanyId + ")\n");
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
    }
}
