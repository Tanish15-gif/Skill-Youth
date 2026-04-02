using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.UserCourseDTO;
using System.Collections.Generic;

namespace Project.CourseOperation
{
    public class DisplayUserCourse
    {
        private readonly string? conn;
        public DisplayUserCourse(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public List<UserCourseDisplay> DisplayCourse(int userId)
        {
            List<UserCourseDisplay> userCourses = new List<UserCourseDisplay>();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                        select c.CourseId,c.Title,c.Description,c.Url,e.EnrollDate
                        from Courses c
                        join Enrollments e on c.CourseId = e.CourseID
                        where e.UserId = @uid
                ";
                using (var cmd = new SqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserCourseDisplay userCourseDisplay = new UserCourseDisplay();

                            userCourseDisplay.CourseId = (int)reader["CourseId"];
                            userCourseDisplay.Title = reader["Title"].ToString()!;
                            userCourseDisplay.Description = reader["Description"].ToString()!;
                            userCourseDisplay.Url = reader["Url"].ToString()!;
                            if (reader["EnrollDate"] != DBNull.Value)
                            {
                                userCourseDisplay.EnrollmentDate = Convert.ToDateTime(reader["EnrollDate"]);
                            }
                            userCourses.Add(userCourseDisplay);
                        }
                    }
                }
            }
            return userCourses;
        }
    }
}