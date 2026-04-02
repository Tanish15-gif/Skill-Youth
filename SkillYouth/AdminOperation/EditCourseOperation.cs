using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseDTo;

namespace Project.AdminOperation
{
    public class EditCourseAdmin
    {
        private readonly string? conn;
        public EditCourseAdmin(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public int EditCourse(Courses courses)
        {
            try
            {
                using (var connect = new SqlConnection(conn))
                {
                    connect.Open();
                    string sql = @"Update Courses Set 
                            Title = @title,
                            Description = @desc,
                            Duration = @duration,
                            Url = @url
                            where CourseId = @cid";
                    using (var cmd = new SqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@cid", courses.CourseId);
                        cmd.Parameters.AddWithValue("@title", courses.Title);
                        cmd.Parameters.AddWithValue("@desc", courses.Description);
                        cmd.Parameters.AddWithValue("@duration", courses.Duration);
                        cmd.Parameters.AddWithValue("@url", courses.Url);

                        int rows = cmd.ExecuteNonQuery();
                        
                        return rows > 0 ? 1 : 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}