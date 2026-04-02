using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.EnrollmentDTO;

namespace Project.EnrollMentOperation
{
    public class EnrollUser
    {
        private readonly string? conn;
        public EnrollUser(IConfiguration config)
        {
            conn = config.GetConnectionString("DefaultConnection");
        }
        public int Enroll(Enrollments enrollments)
        {
            try
            {
                using (var connect = new SqlConnection(conn))
                {
                    connect.Open();
                    string checksql = "Select Count(*) from Enrollments where UserId = @uid and CourseId = @cid";
                    using (var checkcmd = new SqlCommand(checksql, connect))
                    {
                        checkcmd.Parameters.AddWithValue("@uid",enrollments.UserId);
                        checkcmd.Parameters.AddWithValue("@cid",enrollments.CourseId);
                        int rows = (int)checkcmd.ExecuteScalar();
                        if(rows > 0)
                        {
                            return -1;
                        }
                    }
                    string sql = "Insert into Enrollments(UserId,CourseID) values(@uid,@cid)";
                    using (var cmd = new SqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@uid", enrollments.UserId);
                        cmd.Parameters.AddWithValue("@cid", enrollments.CourseId);

                        int rows = cmd.ExecuteNonQuery();

                        return rows > 0 ? 1 : 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}