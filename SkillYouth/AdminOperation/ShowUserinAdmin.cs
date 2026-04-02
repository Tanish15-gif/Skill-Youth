using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.UserOperation;
using System.Collections.Generic;

namespace Project.AdminOperation
{
    public class ShowUsers
    {
        private readonly string? conn;
        public ShowUsers(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public List<UserInfo> ShowUserDetail()
        {
            List<UserInfo> show = new List<UserInfo>();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                        select u.FullName,d.StudentLevel,d.JoinDate,Count(*) over() as TotalUsers
                        from Users u
                        Join DashBoard d on d.UserId = u.UserId
                        ";
                using (var cmd = new SqlCommand(sql,connect))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            show.Add(new UserInfo
                            {
                                FullName = reader["FullName"].ToString()!,
                                Level = reader["StudentLevel"].ToString()!,
                                JoinDate = Convert.ToDateTime(reader["JoinDate"]), 
                                TotalUsers = (int)reader["TotalUsers"]
                            });
                        }
                    }
                }
            }
            return show;
        }
    }
}