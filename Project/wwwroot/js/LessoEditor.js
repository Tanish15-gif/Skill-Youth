let selectedCourseId = null;
let quill;

document.addEventListener("DOMContentLoaded", function () {
    quill = new Quill('#editor-container', {
        theme: 'snow',   

        modules: {       
            toolbar: [   
                ['bold', 'italic', 'underline', 'strike'],

                [{ 'header': 1 }, { 'header': 2 }],
                
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                
                ['blockquote', 'code-block'],

                [{ 'align': [] }],

                ['link', 'image','video'],

                ['clean']
            ]
        }
    });
});

window.openLessonEditor = function (courseId, courseTitle) {

    selectedCourseId = courseId;

    document.getElementById("course-title-display").innerText =
        "Course: " + courseTitle;

    document.getElementById("lesson-management-section").style.display = "block";

    document.getElementById("lessonTitle").value = "";
    document.getElementById("orderIndex").value = "";
    document.getElementById("headerImage").value = "";
    quill.root.innerHTML = "";

    window.scrollTo({
        top: document.getElementById("lesson-management-section").offsetTop,
        behavior: "smooth"
    });
};

document.getElementById("submit-btn").addEventListener("click", async function () {

    const title = document.getElementById("lessonTitle").value.trim();
    const orderIndex = document.getElementById("orderIndex").value.trim();
    const headerImage = document.getElementById("headerImage").value.trim();
    const content = quill.root.innerHTML;

    if (!title || !orderIndex || !selectedCourseId) {
        document.getElementById("Lesson-error-message").innerText =
            "Please fill all required fields.";
        return;
    }

    const BASE_URL = window.location.origin;

    const response = await fetch(`${BASE_URL}/Users/${selectedCourseId}/lessons`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            courseId: selectedCourseId,
            Title: title,
            OrderIndex: orderIndex,
            HeaderImageUrl: headerImage,
            Content: content
        })
    });

    if (response.ok) {
        alert("Lesson Added Successfully");
        document.getElementById("lesson-management-section").style.display = "none";
    } else {
        document.getElementById("Lesson-error-message").innerText =
            "Error adding lesson.";
    }
});