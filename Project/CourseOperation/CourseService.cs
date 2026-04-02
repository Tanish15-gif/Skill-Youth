using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseDTo;

namespace Project.CourseOperation
{
    public class CourseAdd
    {
        private readonly string? conn;
        public CourseAdd(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool AddCourse(Courses course)
        {
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "Insert into Courses(Title,Description,Duration,Url) values(@t,@d,@duration,@url)";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@t",course.Title);
                    cmd.Parameters.AddWithValue("@d",course.Description);
                    cmd.Parameters.AddWithValue("@duration",course.Duration);
                    cmd.Parameters.AddWithValue("@url",course.Url);
                    
                    return cmd.ExecuteNonQuery() > 0; 
                }
            }
        }
    }
}