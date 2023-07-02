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
        private void connection()
        {
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=SupportDB;Integrated Security=True;Pooling=False";
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
            if(vm.CompanyId == null)
            {
                str.Append(" INSERT into users(UserId, FirstName, LastName, UserName, Email, Password, Profile, IsActive) VALUES(@uid,'" + vm.FirstName + "','" + vm.LastName + "', '" + vm.UserName + "','" + vm.Email + "','" + pass + "','" + vm.Profile + "','" + vm.IsActive + "')\n");

            }
            else{
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

        public List<UpdateRegisterVM> GetUser(string username = null, string password = null, string usercode=null)
        {
            connection();
            List<UpdateRegisterVM> registerList = new List<UpdateRegisterVM>();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select u.UserId, u.FirstName, u.LastName, u.UserName, u.Email, u.Profile, u.IsActive ,c.CompanyId, c.CompanyName from users u left join CompanyInfo c on c.CompanyId = u.CompanyId where 1=1");
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
                    new UpdateRegisterVM
                    {

                        Id = Convert.ToInt32(dr["UserId"]),
                        CompanyId = string.IsNullOrEmpty(dr["CompanyId"].ToString())? null : Convert.ToInt32(dr["CompanyId"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Email = Convert.ToString(dr["Email"]),
                        Profile = Convert.ToString(dr["Profile"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        CompanyName = Convert.ToString(dr["CompanyName"]),

                    });
            }
            return registerList;
        }
        public bool UpdateRegister(int Id, string FirstName, string LastName, string UserName, string Email, string Profile, int? CompanyId,bool IsActive)
        {
            connection();
            StringBuilder str = new StringBuilder();
            if(CompanyId != null)
            {
                str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "', IsActive='"+ IsActive + "', CompanyId="+CompanyId+" WHERE UserId = " + Id +" \n");

            }
            else
            {
                str.Append("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "',Profile = '" + Profile + "', IsActive='" + IsActive + "' WHERE UserId = " + Id + " \n");
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
