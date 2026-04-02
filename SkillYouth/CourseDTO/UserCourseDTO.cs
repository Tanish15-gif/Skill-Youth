namespace Project.UserCourseDTO
{
    public class UserCourseDisplay
    {
        public int CourseId {get;set;}
        public string Title {get;set;} = "";
        public string Description {get;set;} = "";
        public string Url {get;set;} = "";
        public DateTime EnrollmentDate {get;set;}
    }
}