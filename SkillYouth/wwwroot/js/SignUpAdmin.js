document.getElementById('admin-signup-form').addEventListener('submit', SignUpAdmin);
async function SignUpAdmin(event) {
    event.preventDefault();

    const name = document.getElementById('admin-name');
    const email = document.getElementById('admin-email');
    const password = document.getElementById('admin-password');
    const errorMessage = document.getElementById('error-message');
    const loginbtn = document.getElementById("loginbtn");

    if (!name.value || !email.value || !password.value) {
        alert('Fill all the Fields');
        return;
    };
    showMessage("Sending Server...","#38bdf8")
    loginbtn.disabled = false;
    const SignUp = {
        FullName: name.value,
        Email: email.value,
        Password: password.value
    };
    try {
        const response = await fetch("http://localhost:5080/Users/AdminSignUp?role=SuperAdmin", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(SignUp)
        });
        if (response.ok) {
            sessionStorage.setItem("adminAccessGranted", "true");
            errorMessage.innerText = "Signup Successful!";
            errorMessage.style.color = "green";

            setTimeout(() => {
                window.location.href = "admin-login.html";
            }, 1500);
        }
        else {
            const msg = await response.text();
            showMessage("⚠ " + msg, "#ef4444");
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