using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.DisplayCourseAdminDTO;
using System.Collections.Generic;
using System;

namespace Project.AdminOperation
{
    public class AdminDisplayCourse
    {
        private readonly string? conn;
        public AdminDisplayCourse(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public List<CourseInfo> GetCourse()
        {
            List<CourseInfo> courses = new List<CourseInfo>();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "select CourseId,Url,Title,Description,Duration,Count(*) over() as TotalCourse from Courses";
                using (var cmd = new SqlCommand(sql, connect))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new CourseInfo
                            {
                                CourseId = (int)reader["CourseId"],
                                Url = reader["Url"].ToString()!,
                                Title = reader["Title"].ToString()!,
                                Description = reader["Description"].ToString()!,
                                Duration = reader["Duration"].ToString()!,
                                TotalCourse = (int)reader["TotalCourse"]
                            });
                        }
                    }
                }
            }
            return courses;
        }
        public CourseInfo? GetPopularCourse()
        {
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = @"
                    SELECT TOP 1 c.Title as PopularCourse, COUNT(e.CourseID) AS EnrollmentCount
                    FROM Enrollments e
                    join Courses c on e.CourseId = c.CourseId
                    GROUP BY c.Title
                    ORDER BY EnrollmentCount DESC
                ";
                using (var cmd = new SqlCommand(sql, connect))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var PopularCourseName = reader["PopularCourse"].ToString()!;
                            var Total = (int)reader["EnrollmentCount"];
                            
                            return new CourseInfo
                            {
                                PopularCourse = PopularCourseName,
                                TotalEnroll = Total
                            };
                            
                        }
                    }
                }
            }
            return null;
        }
    }
}