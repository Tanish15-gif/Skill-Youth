
async function handleAddCourse() {
    const title = document.getElementById('course-title').value;
    const Description = document.getElementById('course-desc').value;
    const Duration = document.getElementById('course-duration').value;
    const fileInput = document.getElementById('course-image');
    const errorMessage = document.getElementById('error-message');
    const loginBtn = document.getElementById("loginbtn");

    if (!title || !Description || !Duration) {
        showMessage(" Please fill all fields", "red");
        return;
    }
    let finalurl = "/images/default.jpg";

    if (fileInput.files.length > 0) {
        errorMessage.style.display = 'block';
        showMessage("Sending to server...", "#38bdf8");
        loginBtn.disabled = true;

        try {
            const formdata = new FormData();
            formdata.append('file', fileInput.files[0]);

            const uploadResponse = await fetch('http://localhost:5080/Users/upload-image', {
                method: 'POST',
                body: formdata
            });

            if (uploadResponse.ok) {
                const data = await uploadResponse.json();
                finalurl = data.url;
            } else {
                showMessage("Image upload failed", "red");
                return;
            }
        } catch (error) {
            console.error(error);
            showMessage("Network error while uploading", "red");
            return;
        }
    }
    const courseData = {
        Title: title,
        Description: Description,
        Duration: Duration,
        Url: finalurl
    };
    showMessage("Saving course...", "#38bdf8");
    try {
        const respone = await fetch('http://localhost:5080/Users/add-course', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(courseData)
        });
        if (respone.ok) {
            showMessage("Course Added SuccessFully", "limegreen");

        } else {
            showMessage("Failed to save course", "red");
        }
    } catch (error) {
        showMessage("❌ Server not responding", "red");
    }
}
function showMessage(msg, color) {
    const form = document.getElementById("courseForm");
    const error = form.querySelector("#error-message");

    error.innerText = msg;
    error.style.color = color;
    error.style.display = "block";
}
