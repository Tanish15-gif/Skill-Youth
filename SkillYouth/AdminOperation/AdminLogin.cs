using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using Project.AdminLogin;
using Project.Models;

namespace Project.AdminOperation
{
    public class AdminLog
    {
        private readonly string? conn;
        public AdminLog(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public AdminLoginResult Login(GetInAdmin admin)
        {
            var result = new AdminLoginResult();
            if (admin == null || string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
            {
                result.success = false;
                result.Message = "Invalid Input";
                return result;
            }
            string sql = "Select AdminId,Password,Role from Admins where Email = @email";
            string? storedPass = null;
            int adminid = 0;
            string? role = null;
            try
            {
                using (var connect = new SqlConnection(conn))
                {
                    connect.Open();
                    Console.WriteLine($"[DEBUG] Trying to login admin: {admin.Email}");
                    using (var cmd = new SqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@email", admin.Email);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                storedPass = reader["Password"].ToString();
                                adminid = (int)reader["AdminId"];
                                role = reader["Role"].ToString();

                                // Console.WriteLine($"[DEBUG] Found Admin ID: {adminid}");
                                //Console.WriteLine($"[DEBUG] DB Hash: {storedPass}");
                                //Console.WriteLine($"[DEBUG] Role: {role}");
                            }
                            else
                            {
                                result.success = false;
                                result.Message = "Email Not Found";
                                return result;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(storedPass))
                {
                    result.success = false;
                    result.Message = "Password not found";
                    return result;
                }
                bool isPass = BCrypt.Net.BCrypt.Verify(admin.Password, storedPass);
                if (isPass)
                {
                    result.success = true;
                    result.AdminId = adminid;
                    result.Role = role;
                    result.Message = "Login Successful";
                }
                else
                {
                    result.success = false;
                    result.Message = "Wrong Password";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.Message = "Server Error: " + ex.Message;
                return result;
            }
        }
    }
}