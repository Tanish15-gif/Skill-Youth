document.addEventListener('DOMContentLoaded', async function () {
    const adminid = sessionStorage.getItem("AdminuserId");
    const role = sessionStorage.getItem("AdminRole");

    if(role !== "SuperAdmin")
    {
        document.getElementById('add-admin-btn').style.display = "none";
    }
    if (!adminid) {
        alert("Login First");
        return;
    }
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/AdminDashBoard/${adminid}`);
        if (response.ok) {
            const data = await response.json();
            document.getElementById("display-name").innerText = data.fullName;
            document.getElementById("display-email").innerText = data.email;
            const date = new Date(data.joinDate);
            document.getElementById("display-date").innerText = date.toLocaleDateString();
        } else {
            console.error("Failed to fetch dashboard data");
        }
    } catch (error) {
        console.error("Error",error);
    }
})