using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            string constring = "Data Source=DESKTOP-3P1U2GV\\OMSSERVER;Initial Catalog=WebAppDB;Integrated Security=True;Pooling=False";
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
            //var pass = new PasswordHasher<object>().HashPassword(null, vm.NewPassword);
            
            string pass = hasdPassword(vm.NewPassword);
            SqlCommand cmd = new SqlCommand("INSERT into users(FirstName, LastName, UserName, Email, Password) VALUES('"+vm.FirstName+"','"+vm.LastName+"', '"+vm.UserName+"','"+vm.Email+"','"+ pass + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool UserExist(string username, string password) { 
            //establish the database connection
            connection();
            
            string pass = hasdPassword(password);
            //create sql cmd
            SqlCommand cmd = new SqlCommand("SELECT UserName, Password FROM users WHERE UserName = '"+username+"' And Password = '"+ pass + "'", con);
            
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

        public List<UpdateRegisterVM> GetUser()
        {
            connection();
            List<UpdateRegisterVM> registerList = new List<UpdateRegisterVM>();
            SqlCommand cmd = new SqlCommand("SELECT Id, FirstName, LastName, UserName, Email from users", con);

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

                        Id = Convert.ToInt32(dr["Id"]),
                        FirstName = Convert.ToString(dr["FirstName"]),
                        LastName = Convert.ToString(dr["LastName"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        Email = Convert.ToString(dr["Email"]),
                    });
            }
            return registerList;
        }
        public bool UpdateRegister(int Id, string FirstName, string LastName, string UserName, string Email)
        {
            connection();
            SqlCommand cmd = new SqlCommand("Update users SET FirstName = '" + FirstName + "', LastName = '" + LastName + "', UserName = '" + UserName + "', Email = '" + Email + "' WHERE Id = "+Id+"", con);
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
            SqlCommand cmd = new SqlCommand("Delete FROM users WHERE Id =" + id, con);
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
