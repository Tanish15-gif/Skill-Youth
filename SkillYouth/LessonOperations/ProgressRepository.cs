using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.LessonProgressDTO;
using System.Collections.Generic;

namespace Project.LessonOperation
{
    public class Progression
    {
        private readonly string? conn;
        public Progression(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public bool Progress(LessonProgress progress)
        {
            using(var connect = new SqlConnection(conn))
            {
                connect.Open();
                string count = "Select Count(1) From LessonProgress where UserId = @uid and LessonId = @lid";
                using(var countcmd = new SqlCommand(count,connect))
                {
                    countcmd.Parameters.AddWithValue("@uid",progress.Userid);
                    countcmd.Parameters.AddWithValue("@lid",progress.LessonId);
                    int check = (int)countcmd.ExecuteScalar();
                    if(check > 0)
                    {
                        return false;
                    }
                }
                string sql = "Insert into LessonProgress(UserId,LessonId) Values(@uid,@lid)";
                using(var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@uid",progress.Userid);
                    cmd.Parameters.AddWithValue("@lid",progress.LessonId);
                    
                    int rows = cmd.ExecuteNonQuery();

                    return rows > 0;
                }
            }
        }
        public List<int> GetCompletedLessons(int userid)
        {
            List<int> completedIds = new List<int>();
            using (var connect = new SqlConnection(conn))
            {
                connect.Open();
                string sql = "Select LessonId from LessonProgress where UserId = @uid";
                using (var cmd = new SqlCommand(sql,connect))
                {
                    cmd.Parameters.AddWithValue("@uid",userid);
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int lessonid = Convert.ToInt32(reader["LessonId"]);

                            completedIds.Add(lessonid);
                        }
                    }
                }
            }
            return completedIds;
        }
    }
}