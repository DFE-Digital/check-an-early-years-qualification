const saveLevelUrl = document.currentScript.dataset.saveLevelUrl;

$(document).ready(function () {
    const form = document.getElementById('radio-question-form');

    if (form == null || !saveLevelUrl) {
        return;
    }

    // Save any level the user has selected before taking them to the qualification levels page,
    // so it is pre-selected when they use the back link to return
    const levelsLinks = form.querySelectorAll('a[href$="/advice/early-years-qualification-levels"]');

    levelsLinks.forEach(link => {
        link.addEventListener('click', function (event) {
            // let modified clicks (e.g. open in new tab) behave as a normal link
            if (event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey) {
                return;
            }

            event.preventDefault();
            form.action = saveLevelUrl;
            form.submit();
        });
    });
});
