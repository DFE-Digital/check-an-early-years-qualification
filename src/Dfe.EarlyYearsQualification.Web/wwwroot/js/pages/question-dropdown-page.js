document.addEventListener('DOMContentLoaded', function() {
    let checkbox = document.getElementById('awarding-organisation-not-in-list');
    let dropdown = document.getElementById('awarding-organisation-select');

    dropdown.disabled = checkbox.checked;

    checkbox.addEventListener('click', function() {
        dropdown.disabled = !!checkbox.checked;
    });
});