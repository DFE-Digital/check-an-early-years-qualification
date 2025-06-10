$(".print-button").on('click', function() {
    $('.govuk-details').attr('open', 'open');
    window.print();
});

window.addEventListener("afterprint", (event) => {
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