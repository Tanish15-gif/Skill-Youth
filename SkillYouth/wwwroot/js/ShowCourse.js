document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById('view-course-btn');
    if (btn) {
        btn.addEventListener('click', LoadCourse);
    }
    const hideLesson = document.getElementById("lesson-management-section");
    if (hideLesson) hideLesson.style.display = "none";
})

window.LoadCourse = async function () {
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/AdminDashBoard/ShowCourse`, { method: 'GET' });
        if (response.ok) {
            const ShowCourse = await response.json();
            const tableBody = document.getElementById('course-table-body');
            tableBody.innerHTML = '';

            if (ShowCourse === 0) {
                tableBody.innerHTML = "<tr><td colspan='4' class='text-center text-muted'>No User found.</td></tr>";
                return;
            } if (ShowCourse.length > 0) {
                document.getElementById("TotalCourse").innerText = ShowCourse[0].totalCourse;
            }
            ShowCourse.forEach(course => {
                tableBody.innerHTML += `
                <tr>
                    <td>
                    <img src="${course.url}" class="courses-img" alt="Course Image">
                    </td>
                    <td>${course.title}</td>
                    <td class="level-cell">${course.description}</td>
                    <td class="date-cell">${(course.duration)}</td>
                    <td>
                        <button class="btn"  onclick='openEdit(${JSON.stringify(course)})'>Edit</button>
                        <button class="btn" id="delete-course-btn" onclick='DeleteCourse(${course.courseId})'>Delete</button>
                        <button class="btn" onclick="openLessonEditor(${course.courseId}, '${course.title}')">Add Lesson</button>
                    </td>
                </tr>
                `;
            });
            applyCourseTableStyles();

            document.getElementById('course-list').style.display = 'block';
        }
        const Popular = await fetch(`${BASE_URL}/Users/AdminDashBoard/PopularCourse`,{method : 'GET'});
        if(Popular.ok)
        {
            const ShowPopular = await Popular.json();
            document.getElementById('PopularCourse').innerText = ShowPopular.popularCourse;
            document.getElementById('EnrollCount').innerText = ShowPopular.totalEnroll;
        }

    } catch (error) {
        console.error("Server error: ", error);
    }

};

document.getElementById('hide-course-btn').addEventListener('click', function () {
    document.getElementById('course-list').style.display = 'none';
});
function applyCourseTableStyles() {

    document.querySelectorAll("#course-table-body tr").forEach(row => {

        row.addEventListener("click", () => {

            document.querySelectorAll("#course-table-body tr")
                .forEach(r => r.classList.remove("active"));

            row.classList.add("active");
        });
    });
}
