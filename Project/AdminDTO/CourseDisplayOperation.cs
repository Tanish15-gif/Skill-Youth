namespace Project.DisplayCourseAdminDTO
{
    public class CourseInfo 
    {
        public int CourseId { get; set; }
        public string Url {get;set;} = "";
        public string Title {get;set;} = "";
        public string Description {get;set;} = "";
        public string Duration {get;set;} = "";
        public int TotalCourse {get;set;}
        public string PopularCourse {get;set;} = "";
        public int TotalEnroll {get;set;}
    }
}