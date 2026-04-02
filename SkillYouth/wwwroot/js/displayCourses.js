document.addEventListener("DOMContentLoaded", function () {
    loadCourses();
});
async function loadCourses() {
    const container = document.getElementById('course-list');
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/get-all-courses`);

        if (!response.ok) {
            container.innerHTML = "<p>No Course found or Server Error<p/>";
            return;
        }

        const courses = await response.json();
        container.innerHTML = "";
        courses.forEach(course => {

            const CourseCard = `
                <div class="course">
                    <div class="course-img">
                        <img src="${course.url}" class="course-img" alt="${course.title}">
                    </div>
                    <h3>${course.title}</h3>
                    <p>${course.description}</p>
                    <p><small>Duration: ${course.duration}</small></p>
                    
                    <button class="btn" onclick="enrollUser(${course.courseId})">Enroll</button>
                </div>
            `;
            container.innerHTML += CourseCard;
        });
    } catch (error) {
        console.error("Error Loading Courses:", error);
        container.innerHTML = "<p>Error connecting to server.</p>";
    }
}
async function enrollUser(courseId) {
    const userid = sessionStorage.getItem("currentuserId");
    if (!userid) {
        alert("Login First!!!");
        window.location.href = "login.html";
        return;
    }
    const enrollmentData = {
        UserId: parseInt(userid),
        CourseId: courseId
    };
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/enroll-course`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(enrollmentData)
        });
        if (response.status === 409) {
            const data = await response.json();
            alert(data.message);
            return;
        }
        if (response.ok) {
            const data = await response.json();
            alert(data.message);
            window.location.href = "dashboard.html";
        } else {
            const errorData = await response.json().catch(() => ({message : "Server Error"}));
            alert(errorData.message || "Failed to enroll");
        }
    } catch (error) {
        console.error(error);
        alert("Unable to connect the Server ");
    }
}