document.addEventListener("DOMContentLoaded",async function(){
    const userid = sessionStorage.getItem("currentuserId");
    if(!userid){
        alert("Please Login First!");
        window.location.href = "login.html";
        return;
    }
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/dashboard/${userid}`);
        if(response.ok){
            const data = await response.json();
            document.getElementById("name").innerText = data.fullName;
            document.getElementById("email").innerText = data.email;
            document.getElementById("level").innerText = data.studenLevel;
            const date = new Date(data.joinDate);
            document.getElementById("cretedat").innerText = date.toLocaleDateString();
        }
        else{
            console.error("Failed to fetch dashboard data");
        }
    } catch (error) {
        console.error("Error",error);
    }
});

