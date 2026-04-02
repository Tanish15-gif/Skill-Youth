let lessonlist = [];
let currentLessonId = null;
let courseId = null;
let completedLessonIds = [];
document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    courseId = urlParams.get("courseId");

    if (courseId) {
        loadlesson(courseId);
    }
});

async function loadlesson(courseId) {
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/` + courseId + "/lessons");
        lessonlist = await response.json();

        const userid = parseInt(sessionStorage.getItem("currentuserId"));
        const recieve = await fetch(`${BASE_URL}/Users/completed-lesson/` + userid);
        completedLessonIds = await recieve.json();

        const sidebar = document.getElementById("lesson-list");
        sidebar.innerHTML = "";

        lessonlist.forEach((lesson, index) => {
            const li = document.createElement("li");
            if (completedLessonIds.includes(lesson.lessonId)) {
                li.classList.add("completed-lesson");
            }

            li.innerHTML = `
                <span class="chapter-num">${index + 1}.</span>
                <span class="chapter-title">${lesson.title}</span>
            `;

            li.onclick = () => showLessonOnStage(index);

            sidebar.appendChild(li);
        });

        if (lessonlist.length > 0) {
            showLessonOnStage(0);
        }

    } catch (error) {
        console.error("Error loading lessons:", error);
    }
}

function showLessonOnStage(index) {
    if (!lessonlist[index]) return;

    const activelesson = lessonlist[index];

    currentLessonId = activelesson.lessonId;

    document.getElementById("lesson-title").innerText = activelesson.title;
    document.getElementById("lesson-body").innerHTML = activelesson.content;

    const imgElement = document.getElementById("lesson-header-img");

    if (activelesson.headerImageUrl) {
        imgElement.src = activelesson.headerImageUrl;
        imgElement.style.display = "block";
    } else {
        imgElement.style.display = "none";
    }

    const allLessons = document.querySelectorAll("#lesson-list li");
    allLessons.forEach(li => li.classList.remove("active-lesson"));

    if (allLessons[index]) {
        allLessons[index].classList.add("active-lesson");
    }
    if (window.hljs) {
        document.querySelectorAll('#lesson-body pre code').forEach(block => {
            hljs.highlightElement(block);
        });
    }
    const button = document.getElementById("mark-complete-btn");
    button.disabled = false;
    button.classList.remove("completed-state")
    button.innerHTML = '<span class="btn-icon">✅</span><span class="btn-text">Mark as Complete</span>'

    if (completedLessonIds.includes(currentLessonId)) {
        const button = document.getElementById("mark-complete-btn");
        button.innerHTML = '<span class="btn-icon">✅</span><span class="btn-text">Completed!</span>';
        button.classList.add("completed-state");
        button.disabled = true;
    }
}

async function markLessonComplete() {

    const userid = parseInt(sessionStorage.getItem("currentuserId"));
    const data = {
        Userid: userid,
        LessonId: currentLessonId,
    }
    const BASE_URL = window.location.origin;
    const response = await fetch(`${BASE_URL}/Users/lessonprogress`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    if (response.ok) {
        const button = document.getElementById("mark-complete-btn");
        button.innerHTML = '<span class="btn-icon">✅</span><span class="btn-text">Completed!</span>';
        button.classList.add("completed-state");
        button.disabled = true;
        completedLessonIds.push(currentLessonId);
        const activeli = document.querySelector("#lesson-list li.active-lesson");
        if (activeli) {
            activeli.classList.add("completed-lesson");
        }
    }
    else {
        const error = await response.json();
        alert(error.message);
    }
}
