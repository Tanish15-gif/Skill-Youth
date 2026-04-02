using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.ShowAdminDetails;
namespace Project.AdminOperation
{
    public class AdminInfo
    {
        public readonly string? conn;
        public AdminInfo(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public AdminDetail InfoAdmin(int AdminId)
        {
            AdminDetail adminDetail = new AdminDetail();
            using(var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "Select AdminName,Email,JoinDate from Admins where AdminId = @id";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@id",AdminId);
                    using(var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            adminDetail.FullName = reader["AdminName"].ToString()!;
                            adminDetail.Email = reader["Email"].ToString()!;
                            adminDetail.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                        }
                    }
                }
            }
            return adminDetail;
        }
    }
}