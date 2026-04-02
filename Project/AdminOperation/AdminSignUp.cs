using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using Project.AdminSignUp;

namespace Project.AdminOperation
{
    public class SignUpAdmin
    {
        private readonly string? conn;
        public SignUpAdmin(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool AddAdmin(AddAdmin addAdmin)
        {
            try
            {
                using(var connect = new SqlConnection(conn))
                {
                    connect.Open();
                    string hashPass = BCrypt.Net.BCrypt.HashPassword(addAdmin.Password);
                    string sql = "Insert into Admins(AdminName,Password,Email) values(@name,@pass,@email)";
                    using(var cmd = new SqlCommand(sql,connect))
                    {
                        cmd.Parameters.AddWithValue("@name",addAdmin.FullName);
                        cmd.Parameters.AddWithValue("@pass",hashPass);
                        cmd.Parameters.AddWithValue("@email",addAdmin.Email);
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}