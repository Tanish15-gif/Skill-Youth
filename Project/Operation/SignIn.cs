using Microsoft.Data.SqlClient;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Project.SigningUp;

namespace Project.Operation
{
    public class GetYouth
    {
        private readonly string? conn;
        public GetYouth(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool AddUsers(SignUp signUp)
        {
            try
            {
                using (var connect = new SqlConnection(conn))
                {
                    connect.Open();
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(signUp.Password);
                    string sql = "Insert into Users(FullName,Email,Password) values(@Name,@email,@pass)";
                    using (var cmd = new SqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", signUp.FullName);
                        cmd.Parameters.AddWithValue("@email", signUp.Email);
                        cmd.Parameters.AddWithValue("@pass", hashPassword);
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