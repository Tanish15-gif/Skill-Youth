using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseLessonDTO;
using System.Collections.Generic;

namespace Project.LessonOperation
{
    public class ShowLesson
    {
        private readonly string? conn;
        public ShowLesson(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public List<CourseModules> DisplayLesson(int courseid)
        {
            List<CourseModules> courses = new List<CourseModules>();
            using(var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                        Select 
                        LessonId,
                        Title,
                        Content,
                        HeaderImageUrl,
                        OrderIndex
                        from CourseLesson where CourseId = @cid
                        Order by OrderIndex ASC";
                using (var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@cid",courseid);
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CourseModules modules = new CourseModules();
                            modules.LessonId = (int)reader["LessonId"];
                            modules.Title = reader["Title"].ToString()!;
                            modules.Content = reader["Content"].ToString()!;
                            modules.OrderIndex = (int)reader["OrderIndex"];
                            if(reader["HeaderImageUrl"] != DBNull.Value)
                            {
                                modules.HeaderImageUrl = reader["HeaderImageUrl"].ToString()!;
                            }
                            else
                            {
                                modules.HeaderImageUrl = string.Empty;
                            }
                            
                            courses.Add(modules);
                        }
                    }
                }
            }
            return courses;
        }
    }
}