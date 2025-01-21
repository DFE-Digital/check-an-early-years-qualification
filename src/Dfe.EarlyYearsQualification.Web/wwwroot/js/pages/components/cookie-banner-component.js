document.addEventListener('DOMContentLoaded', function() {
    const relativePath = window.location.pathname + window.location.search;

    const returnUrl = document.getElementById("returnUrl");
    if (returnUrl) {
        returnUrl.value = relativePath;
    }

    const returnUrlHideBanner = document.getElementById("returnUrlHideBanner");
    if (returnUrlHideBanner) {
        returnUrlHideBanner.value = relativePath;
    }
});