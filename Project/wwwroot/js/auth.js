document.getElementById('signup-form').addEventListener('submit', SignUp);

async function SignUp(event) {
    event.preventDefault();

    const username = document.getElementById('username');
    const email = document.getElementById('email');
    const pass = document.getElementById('password');
    const errorMessage = document.getElementById('error-message');

    if (!username.value || !email.value || !pass.value) {
        alert('Fill the Required Fields');
        return;
    }

    const NewSignIn = {
        FullName: username.value,
        Email: email.value,
        Password: pass.value
    };
    showMessage("Sending Server...","#38bdf8")
    loginbtn.disabled = false;

    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/signup`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(NewSignIn)
        });

        if (response.ok) {
            errorMessage.innerText = "Signup Successful!";
            errorMessage.style.color = "green";

            setTimeout(() => {
                window.location.href = "login.html";
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