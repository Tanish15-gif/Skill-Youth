namespace Project.Models
{
    public class AdminLoginResult
    {
        public bool success {get;set;}
        public int AdminId {get;set;}
        public string? Role {get;set;} = "";
        public string Message {get;set;} = "";
    }
}