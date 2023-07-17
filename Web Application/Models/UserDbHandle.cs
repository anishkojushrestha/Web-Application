using Amazon.SimpleSystemsManagement.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class UserDbHandle
    {
        private SqlConnection con;
        HttpContextAccessor accessor = new HttpContextAccessor();
        ISession session => accessor.HttpContext.Session;
        private void connection()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);
            IConfiguration configuration = builder.Build();
            string constring = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            con = new SqlConnection(constring);
        }

        public string hasdPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
        public bool RegisteUser(RegisterVM vm)
        {
            connection();
            string pass = hasdPassword(vm.NewPassword);
            StringBuilder str = new StringBuilder(); 
            str.Append(" declare @uid bigint \n");
            str.Append(" set @uid = (select isnull(max(userid), 0) + 1 from users) \n");
            if (vm.CompanyId == null)
            {
                str.Append(" INSERT into users(UserId, FirstName, LastName, UserName, Email, Password, Profile, IsActive) VALUES(@uid,'" + vm.FirstName + "','" + vm.LastName + "', '" + vm.UserName + "','" + vm.Email + "','" + pass + "','" + vm.Profile + "','" + vm.IsActive + "')\n");

            }
            else {
                str.Append(" INSERT into users(UserId, FirstName, LastName, UserName, Email, Password, Profile, IsActive,CompanyId) VALUES(@uid,'" + vm.FirstName + "','" + vm.LastName + "', '" + vm.UserName + "','" + vm.Email + "','" + pass + "','" + vm.Profile + "','" + vm.IsActive + "'," + vm.CompanyId + ")\n");

            }

            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public string UserEror(string username)
        {
            //establish the database connection
            connection();

            //create sql cmd
            StringBuilder sb = new StringBuilder();
            sb.Append(" declare @username nvarchar(50)\n");
            sb.Append(" set @username='" + username + "'\n");
            sb.Append(" if(exists(select * from users where username=@username)) begin\n");
            sb.Append(" select username from users where username=@username\n");
            sb.Append(" end\n");
            sb.Append(" else \n");
            sb.Append(" begin\n");
            sb.Append(" select 'notfound'\n");
            sb.Append(" end\n");

            SqlCommand cmd = new SqlCommand("SELECT UserName FROM users WHERE UserName = '" + username + "'", con);
            string user = "";
            //create object and pass sql command
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            //create database table to hold data
            DataTable dt = new DataTable();
            con.Open();
            //fills datatable and with data retrieved by executing the sql cmd 
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                user = Convert.ToString(dr["UserName"]);
            }
            return user;
        }
    
            public bool UserExist(string username)
        {
            //establish the database connection
            connection();

            //create sql cmd
            SqlCommand cmd = new SqlCommand("SELECT UserName FROM users WHERE UserName = '" + username + "'", con);
            
            //create object and pass sql command
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            //create database table to hold data
            DataTable dt = new DataTable();
            con.Open();
            //fills datatable and with data retrieved by executing the sql cmd 
            sd.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditProfile(string? FirstName, string? LastName, string? Email, string? UserName, string Id)
        {
            connection();
            var fname = string.IsNullOrEmpty(FirstName) ? session.GetString("userFirstName") : FirstName;
            var lname = string.IsNullOrEmpty(LastName) ? session.GetString("userLastName") : LastName;
            var uname = string.IsNullOrEmpty(UserName) ? session.GetString("userName") : UserName;
            var email = string.IsNullOrEmpty(Email) ? session.GetString("userEmail") : Email;
            SqlCommand cmd = new SqlCommand("Update users set FirstName = '"+ fname + "', LastName = '"+ lname + "', Email = '"+ email + "', UserName = '"+ uname + "' where UserId = " + Id + "", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool ChangePassword(string? NewPassword, string? Id)
        {
            connection();
            var pass = hasdPassword(NewPassword);
            SqlCommand cmd = new SqlCommand("Update users set Password = '"+ pass + "' where UserId = "+Id+"", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<UserVM> GetUser(string username = null, string password = null, string usercode=null)
        {
            connection();
            List<UserVM> registerList = new List<UserVM>();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select u.UserId, u.FirstName, u.LastName, u.UserName, u.Email, u.Profile,u.Password, u.IsActive ,c.CompanyId, c.CompanyName,Format(c.RegistrationDate,'dd-MM-yyyy') as RegistrationDate, c.Address, c.Email as CompanyEmail,Format(c.ValidFrom,'dd-MM-yyyy') as ValidFrom,Format(c.ValidTo,'dd-MM-yyyy') as ValidTo from users u left join CompanyInfo c on c.CompanyId = u.CompanyId where 1=1");
            if (!string.IsNullOrEmpty(username))
            {
                sb.Append(" and u.username='"+username+"'\n");
            }
            if (!string.IsNullOrEmpty(password))
            {
                sb.Append(" and u.Password='" + hasdPassword(password) + "'\n");
            }
            if (!string.IsNullOrEmpty(usercode))
            {
                sb.Append(" and u.UserId='" + usercode + "'\n");
            }
            SqlCommand cmd = new SqlCommand(sb.ToString(), con);
            //SqlCommand cmd = new SqlCommand("SELECT * from users", con);
            
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                registerList.Add(
                    new UserVM
                    {
                        Id = Convert.ToInt32(dr["UserId"]),
                        CompanyId = string.IsNullOrEmpty(dr["CompanyId"].ToString())? null : Convert.ToInt32(dr["CompanyId"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Email = Convert.ToString(dr["Email"]),
                        Profile = Convert.ToString(dr["Profile"]),
                        NewPassword = Convert.ToString(dr["Password"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),
                        RegistrationDate = Convert.ToString(dr["RegistrationDate"]),
                        ValidFrom = Convert.ToString(dr["ValidFrom"]),
                        ValidTo = Convert.ToString(dr["ValidTo"]),
                        Address = Convert.ToString(dr["Address"]),
                        CompanyEmail = Convert.ToString(dr["CompanyEmail"]),

                    }
                    );
                
            }
            return registerList;
        }
        public bool UpdateRegister(int Id, string FirstName, string LastName, string UserName, string Email, string Profile,string? NewPassword, int? CompanyId,bool IsActive)
        {
            connection();
            StringBuilder str = new StringBuilder();
            var newpass = string.IsNullOrEmpty(NewPassword)?"": hasdPassword(NewPassword);
            if(CompanyId != null)
            {
                if (NewPassword != null)
                {
                    str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "', Password = '" + newpass + "', IsActive='" + IsActive + "', CompanyId=" + CompanyId + " WHERE UserId = " + Id + " \n");
                }
                else
                {
                    str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "', IsActive='" + IsActive + "', CompanyId=" + CompanyId + " WHERE UserId = " + Id + " \n");

                }
            }
            else
            {
                if (NewPassword != null)
                {
                    str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "',Password = '" + newpass + "', IsActive='" + IsActive + "' WHERE UserId = " + Id + " \n");
                }
                else
                {
                    str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "', IsActive='" + IsActive + "' WHERE UserId = " + Id + " \n");

                }
            }
            SqlCommand cmd = new SqlCommand(str.ToString(), con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            connection();
            
            SqlCommand cmd = new SqlCommand("Delete FROM users WHERE UserId =" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public void SeedData()
        {
            connection();
            string pass = hasdPassword("superadmin");
            StringBuilder str = new StringBuilder();
            str.Append(" declare @uid bigint \n");
            str.Append(" set @uid = (select isnull(max(userid), 0) + 1 from users) \n");
            str.Append(" INSERT into users(UserId, FirstName, LastName, UserName, Email, Password, Profile, IsActive) VALUES(@uid,'Super','Admin', 'superadmin','superadmin@gmail.com','" + pass + "','SuperAdmin','True')\n");

            // Check if data has already been seeded
            bool isSeeded = false;
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM users", con);
            con.Open();
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                isSeeded = true;
            }
            con.Close();

            // Seed data only if it hasn't been seeded before
            if (!isSeeded)
            {
                SqlCommand cmd = new SqlCommand(str.ToString(), con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
