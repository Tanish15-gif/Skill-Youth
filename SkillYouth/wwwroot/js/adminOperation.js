document.getElementById('admin-btn').addEventListener('click', function () {
    window.location.href = 'admin-login.html';
});


if (document.getElementById('logout-btn')) {
    document.getElementById('logout-btn').addEventListener('click', function () {
        const proceed = confirm("Are you Sure you want to Log out???");
        if (proceed) {
            localStorage.removeItem('adminLoggedIn');
            window.location.href = 'admin-login.html';
        }
    });
}
document.getElementById('add-admin-btn').addEventListener('click', function () {
    sessionStorage.setItem('adminAccessGranted', 'true');
    window.location.href = 'admin-signup.html';
});