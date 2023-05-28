using System.Data;
using System.Data.SqlClient;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class CustomerDbHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=.;Initial Catalog=WebAppDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }
        
        public List<CustomerVM> GetCustomer()
        {
            connection();
            List<CustomerVM> CustomerList = new List<CustomerVM>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM customers", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            adapter.Fill(dt);
            con.Close();
            foreach(DataRow dr in dt.Rows)
            {
                CustomerList.Add(new CustomerVM
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    ContactPerson = Convert.ToString(dr["ContactPerson"]),
                    phoneNumber = Convert.ToInt32(dr["phoneNo"]),
                    MobileNumber = Convert.ToInt32(dr["MobileNo"]),
                    Email = Convert.ToString(dr["Email"]),
                    PanNumber = Convert.ToInt32(dr["PanNo"]),
                    Address = Convert.ToString(dr["Address"]),
                    City = Convert.ToString(dr["City"]),
                    Country = Convert.ToString(dr["Country"]),

                });
            }
            return CustomerList;

        }
        public bool AddCustomer(CustomerVM vm)
        {
            connection();
            SqlCommand cmd = new SqlCommand("INSERT INTO customers(CompanyName, ContactPerson, PhoneNo, MobileNo, Email, PanNo, Address, City, Country) VALUES ('"+vm.CompanyName+"','"+vm.ContactPerson + "','"+vm.phoneNumber+"','"+vm.MobileNumber+"','"+vm.Email+"','"+vm.PanNumber+"','"+vm.Address+"','"+vm.City+"','"+vm.Country+"')",con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }
        public bool UpdateCustomer(CustomerVM vm)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UPDATE customers SET CompanyName = '"+vm.CompanyName+"', ContactPerson = '"+vm.ContactPerson + "',PhoneNo = '"+vm.phoneNumber+"',MobileNo = '"+vm.MobileNumber+"',Email = '"+vm.Email+"',PanNo = '"+vm.PanNumber+"',Address = '"+vm.Address+"',City = '"+vm.City+"',Country = '"+vm.Country+"' ", con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }

        public bool DeleteCustomer(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DELETE FROM customer WHERE Id = "+id,con);
            con.Open();
            var i = cmd.ExecuteNonQuery();
            con.Close();
            if (i>=1)
                return true;
            else
                return false;   

        }

    }
}
