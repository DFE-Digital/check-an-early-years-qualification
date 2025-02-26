$("#print-button").on('click', function() {
    $('.govuk-details').attr('open', 'open');
    window.print();
});

window.addEventListener("afterprint", (event) => {
    $('.govuk-details').removeAttr('open');
})