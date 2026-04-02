using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseDTo;
using System.Collections.Generic;
namespace Project.CourseOperation
{
    public class DisplayCourse
    {
        private readonly string? conn;
        public DisplayCourse(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public List<Courses> Display()
        {
            List<Courses> coursesList = new List<Courses>();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "select * from Courses";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Courses courses = new Courses
                            {
                                CourseId = (int)reader["CourseId"],
                                Title = reader["Title"].ToString()!,
                                Description = reader["Description"].ToString()!,
                                Duration = reader["Duration"].ToString()!
                            };
                            if (reader["Url"] != DBNull.Value)
                            {
                                courses.Url = reader["Url"].ToString()!;
                            }
                            else
                            {
                                courses.Url = "/images/default.jpg";
                            }

                            coursesList.Add(courses);
                        }
                    }
                }
            }
            return coursesList;
        }
    }
}