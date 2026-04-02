using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.DashBoard;

namespace Project.Operation
{
    public class DashDetails
    {
        private readonly string? conn;
        public DashDetails(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public Details GetDashDetails(int UserId)
        {
            Details details = new Details();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                            select u.FullName,u.Email,d.StudentLevel,d.JoinDate
                            from Users u
                            Join DashBoard d on u.UserId = d.UserId
                            where u.UserId = @dash
                ";
                using (var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@dash",UserId);
                    using(var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            details.FullName = reader["FullName"].ToString()!;
                            details.Email = reader["Email"].ToString()!;
                            details.StudenLevel = reader["StudentLevel"].ToString()!;
                            details.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                        }
                    }
                }
            }
            return details;
        }
    }
}