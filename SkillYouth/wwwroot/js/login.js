document.getElementById('login-form').addEventListener('submit', Login);

async function Login(event) {
    event.preventDefault();

    const email = document.getElementById('E-mail');
    const pass = document.getElementById('password');
    const errorMessage = document.getElementById('error-message');
    const loginbtn = document.getElementById("loginbtn");

    if (!email.value || !pass.value) {
        alert("Fill all fields");
        return;
    }

    const loginData = {
        Email: email.value,
        Password: pass.value
    };

    showMessage("Contacting to Server...","#38bdf8")
    loginbtn.disabled = false;

    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(loginData)
        });

        if (response.ok) {

            const result = await response.json(); 

            if (result.success) {
                sessionStorage.setItem("currentuserId",result.userid);
                errorMessage.innerText = "Login Successful!";
                errorMessage.style.color = "green";

                setTimeout(() => {
                    window.location.href = "dashboard.html";
                }, 1000);
            }
            else {
                showMessage("⚠ " + result.message, "#ef4444");
            }

        } 
        else {
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