$(document).ready(function () {

    const radioButtons = document.querySelectorAll('input[type="radio"]');

    const checkedRadioButton = document.querySelector('input[type="radio"][checked]');

    if (checkedRadioButton != null && checkedRadioButton.attributes.nested != undefined) {
        let hiddenContainer = checkedRadioButton.parentElement.nextElementSibling;
        hiddenContainer.classList.remove('govuk-!-display-none');
    }

    radioButtons.forEach(radio => {
        radio.addEventListener('change', function () {
            radioButtons.forEach(input => {
                if (input.attributes.nested != undefined) {
                    let hiddenContainer = input.parentElement.nextElementSibling;
                    hiddenContainer.classList.add('govuk-!-display-none');
                }
            });

            // Radio button has a nested attribute so display when selected
            if (this.checked && this.attributes.nested != undefined) {
                let hiddenContainer = radio.parentElement.nextElementSibling;

                hiddenContainer.classList.remove('govuk-!-display-none');
                //Focus on the first input within the revealed container
                hiddenContainer.querySelector("input").focus();
            }
        });
    });
});