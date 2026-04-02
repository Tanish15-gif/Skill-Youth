document.addEventListener("DOMContentLoaded", function () {
    
    const logoutBtn = document.getElementById('logout-btn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function() {
            if (confirm('Are you sure you want to log out?')) {
                sessionStorage.clear();
                window.location.href = 'login.html';
            }
        });
    }

    const openBtn = document.getElementById("openCourseForm");
    const closeBtn = document.getElementById("closeCourseForm");
    const modal = document.getElementById("courseModal");

    if (openBtn && modal) {
        openBtn.onclick = () => {
            modal.style.display = "flex";
        };

        if (closeBtn) {
            closeBtn.onclick = () => {
                modal.style.display = "none";
            };
        }

        window.onclick = (e) => {
            if (e.target === modal) {
                modal.style.display = "none";
            }
        };
    }
});