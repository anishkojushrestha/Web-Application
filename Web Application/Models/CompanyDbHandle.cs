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
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string constring = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
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
                str.Append("UPDATE CompanyInfo SET CompanyName = '" + vm.CompanyName + "',Category='"+vm.Category+"', Address = '" + vm.Address + "',Email = '" + vm.Email + "',PanNumber = '" + vm.PanNumber + "', District = '" + vm.District + "', Country = '" + vm.Country + "',RegistrationDate = '" + registerDate + "', ValidFrom = '" + validFrom + "', ValidTo = '" + validTo + "' WHERE CompanyId = " + vm.Id + "\n");
                foreach (var data in vm.contactPersonVM)
                {
                    if (data.Id != 0)
                    {
                        str.Append(" update contactperson set ContactName = '" + data.ContactName + "',Gender = '" + data.Gender + "', Address = '" + data.Address + "', Email = '"+data.Email+"', PhoneNumber = '" + data.phoneNumber + "', MobileNumber = '" + data.MobileNumber + "', designation = '" + data.Designation + "' where ContactId =" + data.Id + "  \n");
                        
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
                
                    str.Append(" INSERT INTO CompanyInfo(CompanyId,CompanyName,Category, Email, PanNumber, Address, District ,Country, RegistrationDate, ValidFrom, ValidTo ) VALUES(@cid,'" + vm.CompanyName + "','" + vm.Category + "','" + vm.Email + "','" + vm.PanNumber + "','" + vm.Address + "','" + vm.District + "','" + vm.Country + "','" + registerDate + "','" + validFrom + "','" + validTo + "') \n");

                
                foreach (var data in vm.contactPersonVM)
                {

                    str.Append("set @pid = (select isnull(max(ContactId),0)+1 from ContactPerson)\n");
                    
                        str.Append("INSERT INTO ContactPerson(ContactId, ContactName, Gender,Address, Email, PhoneNumber, MobileNumber,Designation, CompanyId ) VALUES(@pid, '" + data.ContactName + "','" + data.Gender + "','" + data.Address + "','" + data.Email + "','" + data.phoneNumber + "','" + data.MobileNumber + "','" + data.Designation + "',@cid) \n");
                    
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
        public CompanyMV GetAllCompany(int id)
        {
            connection();
            CompanyMV customerList = new CompanyMV();
            customerList.contactPersonVM = new List<ContactPersonVM>();
            SqlCommand cmd = new SqlCommand("SELECT c.CompanyId, c.CompanyName, c.Address as CompanyAddress, c.Email as CompanyEmail, c.PanNumber, c.District, c.Country, Format(c.RegistrationDate,'MM-dd-yyyy') as RegistrationDate ,c.Category,Format(c.ValidFrom,'MM-dd-yyyy') as ValidFrom,Format(c.ValidTo,'MM-dd-yyyy') as ValidTo, p.* FROM CompanyInfo c left join ContactPerson p on p.CompanyId = c.CompanyId where c.CompanyId =" + id, con);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            ad.Fill(dt);
            con.Close();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i == 0) {
                    customerList.Id = Convert.ToInt32(dr["CompanyId"]);
                    customerList.CompanyName = Convert.ToString(dr["CompanyName"]);
                    customerList.Category = Convert.ToString(dr["Category"]);
                    customerList.Address = Convert.ToString(dr["CompanyAddress"]);
                    customerList.Email = Convert.ToString(dr["CompanyEmail"]);
                    customerList.PanNumber = Convert.ToString(dr["PanNumber"]);
                    customerList.District = Convert.ToString(dr["District"]);
                    customerList.Country = Convert.ToString(dr["Country"]);
                    customerList.RDate = Convert.ToString(dr["RegistrationDate"]);
                    customerList.VFrom = Convert.ToString(dr["ValidFrom"]);
                    customerList.VTo = Convert.ToString(dr["ValidTo"]);
                         

                    }
                customerList.contactPersonVM.Add(new ContactPersonVM
                {
                    ContactName = Convert.ToString(dr["ContactName"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Address = Convert.ToString(dr["Address"]),
                    phoneNumber = Convert.ToString(dr["phoneNumber"]),
                    MobileNumber = Convert.ToString(dr["MobileNumber"]),
                    Designation = Convert.ToString(dr["Designation"]),
                    Email = Convert.ToString(dr["Email"]),
                });
            }
            return customerList;
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
                        Category = Convert.ToString(dr["Category"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["Email"]),
                        PanNumber =Convert.ToString(dr["PanNumber"]),
                        //Date = Convert.ToDateTime(dr["Date"]),
                        District = Convert.ToString(dr["District"]),
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
                        phoneNumber =  Convert.ToString(dr["PhoneNumber"]),
                        MobileNumber = Convert.ToString(dr["MobileNumber"]),
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
            string sql = @"select c.CompanyId,c.CompanyName,c.Category,c.Address,c.Email as CompanyEmail, c.PanNumber,c.District,c.Country,c.RegistrationDate,c.ValidFrom,c.ValidTo,p.ContactId,p.ContactName,p.Gender,p.Address as ContactAddress,p.Email as ContactEmail,p.PhoneNumber,p.MobileNumber,p.Designation  from companyinfo c join contactperson p on c.companyid = p.companyid where 1=1 ";
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
                    cv.Category = Convert.ToString(dr["Category"]);
                    cv.Address = Convert.ToString(dr["Address"]);
                    cv.Email = Convert.ToString(dr["CompanyEmail"]);
                    cv.PanNumber = Convert.ToString(dr["PanNumber"]);
                    cv.District = Convert.ToString(dr["District"]);
                    cv.Country = Convert.ToString(dr["Country"]);
                    cv.RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"]);
                    cv.ValidFrom = Convert.ToDateTime(dr["ValidFrom"]);
                    cv.ValidTo = Convert.ToDateTime(dr["ValidTo"]);
                }
                cv.contactPersonVM.Add(new ContactPersonVM
                {
                    Id= Convert.ToInt32(dr["ContactId"]),
                    ContactName = Convert.ToString(dr["ContactName"]),
                    Address = Convert.ToString(dr["ContactAddress"]),
                    Email = Convert.ToString(dr["ContactEmail"]),
                    phoneNumber = Convert.ToString(dr["PhoneNumber"]),
                    MobileNumber = Convert.ToString(dr["MobileNumber"]),
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
