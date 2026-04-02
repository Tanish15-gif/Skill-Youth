document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById('view-users-btn');
    if (btn) {
        btn.addEventListener('click', LoadUser);
    }
})

window.LoadUser = async function () {
    try {
        const BASE_URL = window.location.origin;
        const response = await fetch(`${BASE_URL}/Users/AdminDashBoard/ShowUser`, { method: 'GET' });
        if (response.ok) {
            const showUser = await response.json();
            const tableBody = document.getElementById('user-table-body');
            tableBody.innerHTML = '';

            if (showUser === 0) {
                tableBody.innerHTML = "<tr><td colspan='4' class='text-center text-muted'>No User found.</td></tr>";
                return;
            } if (showUser.length > 0) {
                document.getElementById("TotalUser").innerText = showUser[0].totalUsers;
            }
            showUser.forEach(user => {
                tableBody.innerHTML += `
                <tr>
                    <td>${user.fullName}</td>
                    <td class="level-cell">${user.level}</td>
                    <td class="date-cell">${new Date(user.joinDate).toLocaleDateString()}</td>
                </tr>
                `;
            });
            applyUserTableStyles();

            document.getElementById('user-list').style.display = 'block';
        }

    } catch (error) {
        console.error("Server error: ", error);
    }

};

document.getElementById('hide-users-btn').addEventListener('click', function () {
    document.getElementById('user-list').style.display = 'none';
});

function applyUserTableStyles() {

    document.querySelectorAll("#user-table-body tr").forEach(row => {

        row.addEventListener("click", () => {

            document.querySelectorAll("#user-table-body tr")
                .forEach(r => r.classList.remove("active"));

            row.classList.add("active");
        });

        const levelCell = row.querySelector(".level-cell");

        if (levelCell) {

            const level = levelCell.innerText.toLowerCase();

            if (level.includes("beginner")) {
                levelCell.classList.add("level-beginner");
            }
            else if (level.includes("intermediate")) {
                levelCell.classList.add("level-intermediate");
            }
            else if (level.includes("pro")) {
                levelCell.classList.add("level-pro");
            }
        }
    });
}
