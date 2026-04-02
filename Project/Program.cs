var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<Project.Operation.GetYouth>();
builder.Services.AddScoped<Project.Operation.LoginUser>();
builder.Services.AddScoped<Project.Operation.DashDetails>();
builder.Services.AddScoped<Project.AdminOperation.AdminLog>();
builder.Services.AddScoped<Project.AdminOperation.SignUpAdmin>();
builder.Services.AddScoped<Project.AdminOperation.AdminInfo>();
builder.Services.AddScoped<Project.AdminOperation.ShowUsers>();
builder.Services.AddScoped<Project.CourseOperation.CourseAdd>();
builder.Services.AddScoped<Project.CourseOperation.DisplayCourse>();
builder.Services.AddScoped<Project.EnrollMentOperation.EnrollUser>();
builder.Services.AddScoped<Project.CourseOperation.DisplayUserCourse>();
builder.Services.AddScoped<Project.AdminOperation.AdminDisplayCourse>();
builder.Services.AddScoped<Project.AdminOperation.EditCourseAdmin>();
builder.Services.AddScoped<Project.AdminOperation.CourseDeletion>();
builder.Services.AddScoped<Project.LessonOperation.Modules>();
builder.Services.AddScoped<Project.LessonOperation.ShowLesson>();
builder.Services.AddScoped<Project.LessonOperation.Progression>();
builder.Services.AddScoped<Project.LessonOperation.CourseTrack>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.UseDefaultFiles();
app.MapControllers();

app.MapGet("/",() => Results.Redirect("/index.html"));
/*string myPassword = "Tanish@2006"; 
string hash = BCrypt.Net.BCrypt.HashPassword(myPassword);

Console.WriteLine(">>> COPY THIS HASH: " + hash);*/
app.Run();