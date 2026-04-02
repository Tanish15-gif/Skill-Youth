using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using Project.LogingIn;

namespace Project.Operation
{
    public class LoginUser
    {
        private readonly string? conn;

        public LoginUser(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }

        public int GetUser(LogIn login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return 0;

            string sql = "SELECT UserId,Password FROM Users WHERE Email = @email";
            string? storedPass = null;
            int userid = 0;

            try
            {
                using (var connect = new SqlConnection(conn))
                {
                    connect.Open();

                    using (var cmd = new SqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@email", login.Email);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userid = (int)reader["UserId"];
                                storedPass = reader["Password"].ToString();
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(storedPass))
                    return 0;

                bool isPass = BCrypt.Net.BCrypt.Verify(login.Password, storedPass);

                if (isPass)
                {
                    return userid;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error: " + ex.Message);
                return 0;
            }
        }
    }
}
