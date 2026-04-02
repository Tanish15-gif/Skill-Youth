using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseLessonDTO;

namespace Project.LessonOperation
{
    public class Modules
    {
        private readonly string? conn;
        public Modules(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool ReceiveLessons(int courseid,CourseModules modules)
        {
            using(var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                    Insert into CourseLesson(CourseId,Title,Content,HeaderImageUrl,OrderIndex) 
                    Values(@cid,@title,@content,@imgurl,@order)
                    ";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@cid",courseid);
                    cmd.Parameters.AddWithValue("@title",modules.Title);
                    cmd.Parameters.AddWithValue("@content",modules.Content);
                    cmd.Parameters.AddWithValue("@order",modules.OrderIndex);

                    if(string.IsNullOrEmpty(modules.HeaderImageUrl))
                    {
                        cmd.Parameters.AddWithValue("@imgurl",DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imgurl",modules.HeaderImageUrl);
                    }
                    
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0;
                }
            }
        }
    }
}