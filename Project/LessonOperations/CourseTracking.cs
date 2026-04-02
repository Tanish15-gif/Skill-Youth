using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.CourseTrackDto;

namespace Project.LessonOperation
{
    public class CourseTrack
    {
        private readonly string? conn;
        public CourseTrack(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public async Task<CourseProgress?> GetCourseProgress(int courseid,int userid)
        {
            try
            {
                CourseProgress progress = new CourseProgress();
                using(var connect = new SqlConnection(conn))
                {
                    await connect.OpenAsync();
                    string sql = "select count(*) from CourseLesson where CourseId = @cid";
                    using(var cmd = new SqlCommand(sql,connect))
                    {
                        cmd.Parameters.AddWithValue("@cid",courseid);
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        progress.TotalLesson = count;
                    }
                    string CheckSql = @"
                                select Count(lp.LessonId) from LessonProgress lp 
                                join CourseLesson cl on lp.LessonId = cl.LessonId 
                                where lp.UserId = @uid and cl.CourseId = @cid";
                    using (var cmd = new SqlCommand(CheckSql,connect))
                    {
                        cmd.Parameters.AddWithValue("@uid",userid);
                        cmd.Parameters.AddWithValue("@cid",courseid);

                        int checkRow = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        progress.CompletedLesson = checkRow;
                    }
                }
                return progress;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}