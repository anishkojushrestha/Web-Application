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
            var validFrom = vm.ValidFrom.ToString("MM/dd/yyyy");
            var validTo = vm.ValidTo.ToString("MM/dd/yyyy");
            StringBuilder str = new StringBuilder();
            str.Append(" declare @cid bigint \n");
            str.Append(" set @cid = (select isnull(max(companyid), 0) + 1 from CompanyInfo) \n");
            str.Append("declare @pid bigint\n");
            if (vm.Id != null)
            {
                str.Append("UPDATE CompanyInfo SET CompanyName = '" + vm.CompanyName + "', Address = '" + vm.Address + "',Email = '" + vm.Email + "',PanNumber = " + vm.PanNumber + ", City = '" + vm.City + "', Country = '" + vm.Country + "',RegistrationDate = '" + registerDate + "', AchiveFrom = '" + validFrom + "', AchiveTo = '" + validTo + "' WHERE CompanyId = " + vm.Id + "\n");
                foreach (var data in vm.contactPersonVM)
                {
                    if (data.Id != 0)
                    {
                        str.Append(" update contactperson set ContactName = '" + data.ContactName + "',Gender = '" + data.Gender + "', Address = '" + data.Address + "', Email = '"+data.Email+"', PhoneNumber = " + data.phoneNumber + ", MobileNumber = " + data.MobileNumber + ", designation = '" + data.Designation + "' where ContactId =" + data.Id + "  \n");
                        
                    }
                    else
                    {
                        str.Append("set @pid = (select isnull(max(ContactId),0)+1 from ContactPerson)\n");

                        str.Append("INSERT INTO ContactPerson(ContactId, ContactName, Gender,Address,Email, PhoneNumber, MobileNumber,Designation, CompanyId ) VALUES(@pid, '" + data.ContactName + "','" + data.Gender + "','" + data.Address + "','" + data.Email + "'," + data.phoneNumber + "," + data.MobileNumber + ",'" + data.Designation + "'," + vm.Id + ") \n");
                    }
                }
            }
            else
            {
                str.Append(" INSERT INTO CompanyInfo(CompanyId,CompanyName, Email, PanNumber, Address, City,Country, RegistrationDate, ValidFrom, ValidTo ) VALUES(@cid,'" + vm.CompanyName + "','" + vm.Email + "'," + vm.PanNumber + ",'" + vm.Address + "','" + vm.City + "','" + vm.Country + "','" + registerDate + "','" + validFrom + "','" + validTo + "') \n");
                foreach (var data in vm.contactPersonVM)
                {

                    str.Append("set @pid = (select isnull(max(ContactId),0)+1 from ContactPerson)\n");

                    str.Append("INSERT INTO ContactPerson(ContactId, ContactName, Gender,Address, Email, PhoneNumber, MobileNumber,Designation, CompanyId ) VALUES(@pid, '" + data.ContactName + "','" + data.Gender + "','" + data.Address + "','" + data.Email + "'," + data.phoneNumber + "," + data.MobileNumber + ",'" + data.Designation + "',@cid) \n");

                }
            }
       
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

                        Id = Convert.ToInt32(dr["CompanyId"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["Email"]),
                        PanNumber = Convert.ToInt32(dr["PanNumber"]),
                        //Date = Convert.ToDateTime(dr["Date"]),
                        City = Convert.ToString(dr["City"]),
                        Country = Convert.ToString(dr["Country"]),
                        RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]),
                        ValidFrom = Convert.ToDateTime(dr["ValidFrom"]),
                        ValidTo = Convert.ToDateTime(dr["ValidTo"]),
                        

                    });
            }
            return customerList;


        }

        public List<ContactPersonVM> GetContactDetail()
        {
            connection();
            List<ContactPersonVM> contactList= new List<ContactPersonVM>();
            SqlCommand cmd = new SqlCommand("select * from ContactPerson", con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                contactList.Add(
                    new ContactPersonVM
                    {

                        Id = Convert.ToInt32(dr["ContactId"]),
                        ContactName = Convert.ToString(dr["ContactName"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["Email"]),
                        phoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                        MobileNumber = Convert.ToInt32(dr["MobileNumber"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        CompanyId = Convert.ToInt32(dr["CompanyId"] ),
                            

                    });
            }
            return contactList;
        }
        public bool DeleteUser(int id)
        {
            connection();
            StringBuilder str = new StringBuilder();  
            str.Append(" Delete FROM contactperson WHERE CompanyId =" + id+" \n");
            str.Append(" Delete from companyinfo where CompanyId = " + id + "\n");
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public CompanyMV GetUpdateDetail( string id=null,string contactid=null)
        {
            connection();

            CompanyMV cv=new CompanyMV ();
            cv.contactPersonVM = new List<ContactPersonVM>();


            string sql = @"select * from companyinfo c join contactperson p on c.companyid = p.companyid where 1=1";
            if (!string.IsNullOrEmpty(id))
            {
                sql+= " and c.companyId='"+id+"'\n";
            }
            if (!string.IsNullOrEmpty(contactid))
            {
                sql += " and p.contactid='" + contactid + "'\n";
            }
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i == 0)
                {
                    cv.Id = Convert.ToInt32(dr["CompanyId"]);
                    cv.CompanyName = Convert.ToString(dr["CompanyName"]);
                    cv.Address = Convert.ToString(dr["Address"]);
                    cv.Email = Convert.ToString(dr["Email"]);
                    cv.PanNumber = Convert.ToInt32(dr["PanNumber"]);
                    //Date = Convert.ToDateTime(dr["Date"]),
                    cv.City = Convert.ToString(dr["City"]);
                    cv.Country = Convert.ToString(dr["Country"]);
                    cv.RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]);
                    cv.ValidFrom = Convert.ToDateTime(dr["AchiveFrom"]);
                    cv.ValidTo = Convert.ToDateTime(dr["AchiveTo"]);
                }
                cv.contactPersonVM.Add(new ContactPersonVM
                {
                    Id= Convert.ToInt32(dr["ContactId"]),
                    ContactName = Convert.ToString(dr["ContactName"]),
                    Address = Convert.ToString(dr["Address"]),
                    Email = Convert.ToString(dr["Email"]),
                    phoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                    MobileNumber = Convert.ToInt32(dr["MobileNumber"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Designation = Convert.ToString(dr["Designation"]),

                });

               
            }
            return cv;
        }
        
        public bool DeleteContact(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DELETE FROM ContactPerson WHERE ContactId="+id,con);
            con.Open();
            var i = cmd.ExecuteNonQuery();  
            con.Close();
            if (i >= 0)
                return true;
            else
                return false;

        }
        
    }
}
