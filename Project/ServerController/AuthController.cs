using Microsoft.AspNetCore.Mvc;
using Project.SigningUp;
using Project.LogingIn;
using Project.Operation;
using Microsoft.Extensions.Configuration;
using Project.DashBoard;
using Project.AdminLogin;
using Project.AdminOperation;
using Project.AdminSignUp;
using Project.ShowAdminDetails;
using Project.CourseOperation;
using Project.CourseDTo;
using Project.EnrollMentOperation;
using Project.EnrollmentDTO;
using Project.UserCourseDTO;
using Project.DisplayCourseAdminDTO;
using Project.LessonOperation;
using Project.CourseLessonDTO;
using Project.LessonProgressDTO;

namespace AuthController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly GetYouth _getYouth;
        private readonly LoginUser _loginUser;
        private readonly DashDetails _dashDetails;
        private readonly AdminLog _adminLog;
        private readonly SignUpAdmin _signUpAdmin;
        private readonly AdminInfo _adminInfo;
        private readonly ShowUsers _showUsers;
        private readonly CourseAdd _courseAdd;
        private readonly DisplayCourse _displayCourse;
        private readonly EnrollUser _enrollUser;
        private readonly DisplayUserCourse _displayUserCourse;
        private readonly AdminDisplayCourse _adminDisplayCourse;
        private readonly EditCourseAdmin _editCourseAdmin;
        private readonly CourseDeletion _courseDeletion;
        private readonly Modules _modules;
        private readonly ShowLesson _showLesson;
        private readonly Progression _progression;
        private readonly CourseTrack _courseTrack;
        public UsersController(
        GetYouth get,
         LoginUser login,
         DashDetails dash,
         AdminLog adminLog,
         SignUpAdmin signUpAdmin,
         AdminInfo adminInfo,
         ShowUsers showUsers,
         CourseAdd course,
         DisplayCourse displayCourse,
         EnrollUser enrollUser,
         DisplayUserCourse displayUserCourse,
         AdminDisplayCourse adminDisplayCourse,
         EditCourseAdmin editCourseAdmin,
         CourseDeletion courseDeletion,
         Modules modules,
         ShowLesson showLesson,
         Progression progression,
         CourseTrack courseTrack
         )
        {
            _getYouth = get;
            _loginUser = login;
            _dashDetails = dash;
            _adminLog = adminLog;
            _signUpAdmin = signUpAdmin;
            _adminInfo = adminInfo;
            _showUsers = showUsers;
            _courseAdd = course;
            _displayCourse = displayCourse;
            _enrollUser = enrollUser;
            _displayUserCourse = displayUserCourse;
            _adminDisplayCourse = adminDisplayCourse;
            _editCourseAdmin = editCourseAdmin;
            _courseDeletion = courseDeletion;
            _modules = modules;
            _showLesson = showLesson;
            _progression = progression;
            _courseTrack = courseTrack;
        }
        [HttpPost("signup")]
        public IActionResult SignIn([FromBody] SignUp signUp)
        {
            if (signUp == null)
            {
                return BadRequest("No Data Received");
            }

            if (string.IsNullOrEmpty(signUp.Email))
            {
                return BadRequest("Email Empty");
            }

            bool success = _getYouth.AddUsers(signUp);

            if (success)
                return Ok("User Added Successfully");
            else
                return BadRequest("Insert Failed");
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LogIn logIn)
        {
            int userId = _loginUser.GetUser(logIn);
            if (userId > 0)
            {
                return Ok(new { success = true, userid = userId });
            }
            else
            {
                return Ok(new { success = false, message = "Invalid Email and Password" });
            }
        }
        [HttpGet("dashboard/{userid}")]
        public IActionResult GetUserDashBoard(int userid)
        {
            try
            {
                Details result = _dashDetails.GetDashDetails(userid);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("User details not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpPost("AdminLogin")]
        public IActionResult LogInAdmin([FromBody] GetInAdmin admin)
        {
            var result = _adminLog.Login(admin);

            return Ok(result);

        }
        [HttpPost("AdminSignUp")]
        public IActionResult RegisterAdmin([FromBody] AddAdmin inAdmin, string role)
        {
            if (inAdmin == null)
            {
                return BadRequest("No Data Received");
            }

            if (string.IsNullOrEmpty(inAdmin.Email))
            {
                return BadRequest("Email Empty");
            }

            if (role != "SuperAdmin")
                return Unauthorized("Only Super Admin Can Add Admins");

            bool success = _signUpAdmin.AddAdmin(inAdmin);

            if (success)
                return Ok("User Added Successfully");
            else
                return BadRequest("Insert Failed");
        }
        [HttpGet("AdminDashBoard/{AdminId}")]
        public IActionResult AdminDash(int AdminId)
        {
            try
            {
                AdminDetail adminDetail = _adminInfo.InfoAdmin(AdminId);
                if (adminDetail != null)
                {
                    return Ok(adminDetail);
                }
                return NotFound("Admin details not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        [HttpGet("AdminDashBoard/ShowUser")]
        public IActionResult GetUser()
        {
            var list = _showUsers.ShowUserDetail();
            return Ok(list);
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No File Uploaded");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var dbPath = $"/images/courses/{fileName}";

            return Ok(new { url = dbPath });
        }
        [HttpPost("add-course")]
        public IActionResult AddCourse([FromBody] Courses courses)
        {
            if (courses == null) return BadRequest("No Course Data");

            bool success = _courseAdd.AddCourse(courses);
            if (success)
            {
                return Ok(new { message = "Course Added SuccessFully" });
            }
            return StatusCode(500, "Failed to create course");
        }
        [HttpGet("get-all-courses")]
        public IActionResult Display()
        {
            List<Courses> courses = _displayCourse.Display();
            if (courses.Count > 0)
            {
                return Ok(courses);
            }
            else
            {
                return NotFound("No Courses Found");
            }
        }
        [HttpPost("enroll-course")]
        public IActionResult UserEnroll([FromBody] Enrollments enrollments)
        {
            int success = _enrollUser.Enroll(enrollments);
            if (success == -1)
            {
                return Conflict(new { message = "Already Enrolled" });
            }
            else if (success == 1)
            {
                return Ok(new { message = "Enrollment successFull" });
            }
            else
            {
                return StatusCode(500, new { message = "DataBase error" });
            }
        }
        [HttpGet("my-courses/{userid}")]
        public IActionResult DisplayUserCourse(int userid)
        {
            List<UserCourseDisplay> courses = _displayUserCourse.DisplayCourse(userid);
            return Ok(courses);
        }
        [HttpGet("AdminDashBoard/showCourse")]
        public IActionResult GetCourse()
        {
            var list = _adminDisplayCourse.GetCourse();
            return Ok(list);
        }
        [HttpGet("AdminDashBoard/PopularCourse")]
        public IActionResult Popular()
        {
            var Popular = _adminDisplayCourse.GetPopularCourse();
            if(Popular != null)
            {
                return Ok(Popular);
            }
            else
            {
                return NotFound("Error Finding the Popular Course");
            }
        }
        [HttpPut("update-course")]
        public IActionResult UpdateCourse([FromBody] Courses courses)
        {
            int result = _editCourseAdmin.EditCourse(courses);
            if (result == 1)
            {
                return Ok(new { message = "Updated SuccessFully" });
            }
            else if (result == 0)
            {
                return NotFound(new { message = "Course Not found" });
            }
            else
            {
                return StatusCode(500, new { message = "Database Error" });
            }
        }
        [HttpDelete("delete-course/{CourseId}")]
        public IActionResult DeleteCourses(int CourseId)
        {
            bool success = _courseDeletion.DeleteCourse(CourseId);
            if(success)
            {
                return Ok(new {message = "Course Deleted SuccessFully"});
            }
            else
            {
                return NotFound(new {message = "Course Not Found"});
            }
        }
        [HttpPost("{courseid}/lessons")]
        public IActionResult AddLessons([FromRoute]int courseid,[FromBody]CourseModules modules)
        {
            bool success = _modules.ReceiveLessons(courseid,modules);
            if(success)
            {
                return Ok(new {message = "Lesson Added SuccessFully"});
            }
            else
            {
                return StatusCode(500,new {message = "DataBase Error"});
            }
        }
        [HttpGet("{courseid}/lessons")]
        public IActionResult ShowLesson([FromRoute]int courseid)
        {
            var list = _showLesson.DisplayLesson(courseid);
            return Ok(list);
        }
        [HttpPost("lessonprogress")]
        public IActionResult MarkProgress(LessonProgress lesson)
        {
            bool success = _progression.Progress(lesson);
            if(success)
            {
                return Ok("Completed");
            }
            else
            {
                return Conflict(new {message ="Already Completed"});
            }
        }
        [HttpGet("completed-lesson/{userid}")]
        public IActionResult CompletedLesson(int userid)
        {
            var list = _progression.GetCompletedLessons(userid);
            return Ok(list);
        }
        [HttpGet("{courseid}/progress/{userid}")]
        public async Task<IActionResult> TrackProgress(int courseid,int userid)
        {
            var progress = await _courseTrack.GetCourseProgress(courseid,userid);
            if(progress == null)
            {
                return StatusCode(500,"Error Calculating the Progress");
            }
            else
            {
                return Ok(progress);
            }
        }
    }
}