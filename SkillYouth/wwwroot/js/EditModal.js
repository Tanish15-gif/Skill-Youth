function openEdit(course) {
    document.getElementById("editOverlay").classList.add("active");

    document.getElementById("editCourseId").value = course.courseId;
    document.getElementById("edit-title").value = course.title;
    document.getElementById("edit-description").value = course.description;
    document.getElementById("edit-duration").value = course.duration;
    document.getElementById("previewImage").src = course.url;
}
let currentImageUrl = "";

async function uploadEditImage(event) {
    const file = event.target.files[0];
    if (!file) return;

    const formData = new FormData();
    formData.append("file", file);

    try {
        const response = await fetch("http://localhost:5080/Users/upload-image", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        currentImageUrl = data.url;

        document.getElementById("previewImage").src = data.url;

    } catch (error) {
        console.error("Upload failed", error);
    }
}

function closeEdit() {
    document.getElementById("editOverlay").classList.remove("active");
}
async function saveEdit() {
    const id = parseInt(document.getElementById("editCourseId").value);
    const Title = document.getElementById("edit-title").value;
    const Description = document.getElementById("edit-description").value;
    const Duration = document.getElementById("edit-duration").value;
    const Image = currentImageUrl ||
        document.getElementById("previewImage").src.replace(window.location.origin, "");
    if (!id || isNaN(id)) {
        showMessage("Invalid Course ID", "red");
        return;
    }


    const EditData = {
        CourseId: id,
        Title: Title,
        Description: Description,
        Duration: Duration,
        Url: Image
    }
    try {

        const response = await fetch(`http://localhost:5080/Users/update-course`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(EditData)
        });
        if (response.ok) {
            showMessage("Updated SuccessFully", "green");
            closeEdit();
        }
        else {
            const errorMessage = await response.json()
            showMessage(errorMessage.message, "red");
            return;
        }
    } catch (error) {
        showMessage("Connection Error", "red");
    }
}
async function DeleteCourse(id)
{

    if(!confirm(`Are you Sure you want to Delete id:${id} Course`)){
        return;
    }
    if(!id || isNaN(id))
    {
        alert("Id not Found!!");
        return;
    }
    try {
        const response = await fetch(`http://localhost:5080/Users/delete-course/${id}` ,{
            method : 'DELETE'
        });
        const data = await response.json();
        if(response.ok)
        {
            alert(data.message);
            LoadCourse();
        }
        else
        {
            alert(data.message);
        }
    } catch (error) {
        console.error(error);
        return;
    }
}

function showMessage(msg, color) {
    const error = document.getElementById("edit-error-message");

    error.innerText = msg;
    error.style.color = color;
    error.style.display = "block";
}
