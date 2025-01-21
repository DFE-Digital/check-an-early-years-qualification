$("#togglePassword").on("click", function() {
    let e = document.getElementById("PasswordValue")
    if (e.type === "password") {
        e.type = "text"
    } else {
        e.type = "password"
    }
});