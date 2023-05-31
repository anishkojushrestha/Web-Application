using System.Data;
using System.Data.SqlClient;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class CompanyDbHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=WebAppDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

        public bool CreateCompany(CompanyMV vm)
        {
            connection();
            var registerDate = vm.RegistrationDate.ToString("MM/dd/yyyy");
            var AchiveFrom = vm.ValidFrom.ToString("MM/dd/yyyy");
            var AchiveTo = vm.ValidTo.ToString("MM/dd/yyyy");
            StringBuilder str = new StringBuilder();
            str.Append(" declare @cid bigint \n");
            str.Append(" set @cid = (select isnull(max(id), 0) + 1 from CompanyInfo) \n");
            str.Append(" INSERT INTO CompanyInfo(id,CompanyName, Email, PanNumber, Address, City, RegistrationDate, AchiveFrom, AchiveTo ) VALUES(@cid,'" +vm.CompanyName+"','" +vm.Email+"',"+vm.PanNumber+",'"+vm.Address+"','"+vm.City+"','"+ registerDate + "','"+ AchiveFrom + "','"+ AchiveTo + "') \n");
            str.Append("declare @pid bigint\n");

            foreach (var data in vm.contactPersonVM)
            {
                str.Append("set @pid = (select isnull(max(ContactId),0)+1 from ContactPerson)\n");

                str.Append("INSERT INTO ContactPerson(ContactId, ContactName, PhoneNumber, MobileNumber, CompanyId ) VALUES(@pid, '" + data.ContactName + "'," + data.phoneNumber + "," + data.MobileNumber + ",@cid) \n");

            }

           
//insert into[dbo].[ContactPerson] (contactname, phonenumber, mobilenumber, companyid)
//select 'contactperson1','1111','4554555',@cid



            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if(i>=1)
                return true;
            else
                return false;
        }

        public List<CompanyMV> GetCompany() {
            connection();
            List<CompanyMV> customerList = new List<CompanyMV>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CompanyInfo", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);   
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                customerList.Add(
                    new CompanyMV
                    {

                        Id = Convert.ToInt32(dr["Id"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["Email"]),
                        PanNumber = Convert.ToInt32(dr["PanNumber"]),
                        //Date = Convert.ToDateTime(dr["Date"]),
                        City = Convert.ToString(dr["City"]),
                        RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]),
                        ValidFrom = Convert.ToDateTime(dr["AchiveFrom"]),
                        ValidTo = Convert.ToDateTime(dr["AchiveTo"]),

                    });
            }
            return customerList;


        }
        public bool DeleteUser(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Delete FROM CompanyInfo WHERE Id =" + id, con);
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
