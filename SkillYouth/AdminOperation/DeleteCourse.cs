using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Project.AdminOperation
{
    public class CourseDeletion
    {
        private readonly string? conn;
        public CourseDeletion(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool DeleteCourse(int CourseId)
        {
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "Delete from Courses where CourseId = @cid";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@cid",CourseId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}