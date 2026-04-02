window.addEventListener("pageshow", function () {
    loadUserCourses();
});
async function loadUserCourses() {
    const container = document.getElementById("course-container");
    const userid = sessionStorage.getItem("currentuserId");

    if (!userid) {
        container.innerHTML = "<p>Log in view your courses<p/>";
        return;
    }
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/my-courses/${userid}`,{ cache: "no-store" });

        if (!response.ok) {
            container.innerHTML = "<p>You haven't Enrolled in any courses yet!<p/>";
            return;
        }

        const courses = await response.json();
        if (courses.length === 0) {
            container.innerHTML = "<p>You haven't Enrolled in any course yet!</p>";
            return;
        }
        container.innerHTML = "";
        for (const course of courses) {
            let percentage = await fetchCourseProgress(course.courseId)
            const CourseCard = `
                <div class="course">
                    <div class="course-img">
                        <img src="${course.url}" class="course-img" alt="${course.title}">
                    </div>
                    <h3>${course.title}</h3>
                    <p>${course.description}</p>
                    <p><small>Enrolled on: ${new Date(course.enrollmentDate).toLocaleDateString()}</small></p>
                    <a href="LearningModules.html?courseId=${course.courseId}" class="btn" style="display: inline-block; text-align: center; text-decoration: none;">Start Learning</a>
                    <div class="course-progress-container">
                        <div class="progress-labels">
                            <span class="progress-text">Course Progress</span>
                            <span class="progress-percentage">${percentage}%</span>
                        </div>
                            <div class="progress-track">
                                <div class="progress-fill" style="width: ${percentage}%;"></div>
                        </div>
                        </div>
                    </div>
            `;
            container.innerHTML += CourseCard;
        }
    } catch (error) {
        console.error("Error Loading Courses:", error);
        container.innerHTML = "<p>Error connecting to server.</p>";
    }
}
async function fetchCourseProgress(courseId){
    const userid = parseInt(sessionStorage.getItem("currentuserId"));
    const BASE_URL = window.location.origin;
    const response = await fetch(`${BASE_URL}/Users/${courseId}/progress/${userid}`,{ cache: "no-store" });

    const data = await response.json();
    if(data.totalLesson === 0){
        return  0;
    }else{
        return Math.round((data.completedLesson / data.totalLesson) * 100);
    }
}