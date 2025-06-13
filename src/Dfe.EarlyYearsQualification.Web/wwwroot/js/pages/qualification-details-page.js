/* Used in the print-button-clicked event. The value is overwritten by the button events and then
* reset by the afterprint event. */
let printType = 'browser';

$("#print-button-top").on('click', function() {
    printType = 'top';
    window.print();
});

$("#print-button-bottom").on('click', function() {
    printType = 'bottom';
    window.print();
});

window.addEventListener("beforeprint", (event) => {
    $('.govuk-details').attr('open', 'open');
    window.dataLayer.push({
        'event': 'print-button-clicked',
        'printType': printType
    });
})

window.addEventListener("afterprint", (event) => {
    printType = 'browser';
    $('.govuk-details').removeAttr('open');
})

$(window).on('load', function() {
    let fAndRValue = $("#qualification-result-message-heading").text();
    let qualificationName = $("#qualification-name-value").text();
    let qualificationLevel = $("#qualification-level-value").text();
    let qualificationAO = $("#awarding-organisation-value").text();

    window.dataLayer.push({
        'event': 'qualification-details',
        'fAndRValue': fAndRValue,
        'qualificationName': qualificationName,
        'qualificationLevel': qualificationLevel,
        'qualificationAO': qualificationAO
    });
});