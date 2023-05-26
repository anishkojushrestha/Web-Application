using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class SupportDbHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=.;Initial Catalog=WebAppDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

        public List<SupportMV> GetSupport() { 
            connection();
            List<SupportMV> SupportList = new List<SupportMV>();
            SqlCommand cmd = new SqlCommand("SELECT * from supports", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                SupportList.Add(
                    new SupportMV
                    {

                        Id = Convert.ToInt32(dr["Id"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        ClientName = Convert.ToString(dr["ClientName"]),
                        CallBy = Convert.ToString(dr["CallBy"]),
                        SupportStaff = Convert.ToString(dr["SupportStaff"]),
                        //Date = Convert.ToDateTime(dr["Date"]),
                        Issue = Convert.ToString(dr["Issue"]),
                        FeedBack = Convert.ToString(dr["FeedBack"]),
                        Remote = Convert.ToString(dr["Remote"]),
                        Status = Convert.ToString(dr["Status"]),
                    });
            }
            return SupportList;

        }

        public bool AddSupport(SupportMV vm)
        {
            connection();
            SqlCommand cmd = new SqlCommand("INSERT into supports(CompanyName, ClientName, CallBy, SupportStaff, Date, Issue, FeedBack, Remote, Status) VALUES ('" + vm.CompanyName + "','" + vm.ClientName + "', '" + vm.CallBy + "','" + vm.SupportStaff + "','" + vm.Date + "','" + vm.Issue + "','" + vm.FeedBack + "','" + vm.Remote + "','" + vm.Status + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool UpdateSupport(SupportMV vm)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UPDATE supports SET CompanyName = '" + vm.CompanyName + "',ClientName = '" + vm.ClientName + "', CallBy = '" + vm.CallBy + "',SupportStaff = '" + vm.SupportStaff + "',Date = '" + vm.Date + "',Issue = '" + vm.Issue + "',FeedBack = '" + vm.FeedBack + "',Remote = '" + vm.Remote + "',Status = '" + vm.Status + "' WHERE ID = "+vm.Id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1) 
                return true;
            else
                return false;

        } 
        public bool DeleteUser(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Delete FROM supports WHERE Id =" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}
