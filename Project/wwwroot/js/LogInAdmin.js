document.getElementById('admin-login-form').addEventListener('submit',LoginAdmin);

async function LoginAdmin(event) {
    event.preventDefault();

    const email = document.getElementById('admin-email');
    const password = document.getElementById('admin-password');
    const errorMessage = document.getElementById('error-message');
    const loginbtn = document.getElementById("loginbtn");

    if(!email || !password)
    {
        alert("Fill all Fields");
        return;
    }
    showMessage("Contacting to server...", "#38bdf8");

    loginbtn.disabled = false;
    const AdminLogin = {
        Email : email.value,
        Password : password.value,
    };
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/AdminLogin`,{
            method : "POST",
            headers : {
                "Content-Type" : "application/json"
            },
            body : JSON.stringify(AdminLogin)
        });
        if(response.ok)
        {
            const result = await response.json();
            if(result.success)
            {
                sessionStorage.setItem("adminAccessGranted", "true");
                sessionStorage.setItem("AdminuserId",result.adminId);
                sessionStorage.setItem("AdminRole",result.role);
                
                errorMessage.innerText = "Login Successful!";
                errorMessage.style.color = "green";
                setTimeout(() => {
                    window.location.href = "AdminDashBoard.html";
                }, 1000);
            }
            else
            {
                showMessage("⚠ " + result.message, "#ef4444");

            }
        }
        else{
            errorMessage.innerText = "Login Failed";
            errorMessage.style.color = "red";
        }
    } catch (error) {
        errorMessage.innerText = "Server Error";
        console.error(error);
    }
}
function showMessage(msg, color) {
    const error = document.getElementById("error-message");

    error.innerText = msg;
    error.style.color = color;
    error.style.display = "block";
}
