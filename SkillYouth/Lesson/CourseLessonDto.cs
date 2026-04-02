namespace Project.CourseLessonDTO
{
    public class CourseModules
    {
        public int LessonId {get;set;}
        public string Title {get;set;} = "";
        public string Content {get;set;} = "";
        public string? HeaderImageUrl {get;set;}
        public int OrderIndex {get;set;}
    }
}