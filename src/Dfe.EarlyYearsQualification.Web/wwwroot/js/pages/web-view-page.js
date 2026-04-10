$(window).on('load', function () {
    let totalQualifications = $('.govuk-summary-card').length;
    let searchTerm = $('[value^="search-term"] span').first().text();
    let filterStartDate = $('[value^="start-date"] span').first().text();
    let filterLevel = $('[value^="qualification-level"] span').first().text();

    window.dataLayer.push({
        'event': "webview-results-returned",
        'searchTerm': searchTerm,
        'filterStartDate': filterStartDate,
        'filterLevel': filterLevel,
        'totalQualifications': totalQualifications
    });
});

$("#remove-filter-form").on("submit", function (event) {
    window.dataLayer.push({
        'event': 'webview-remove-filter',
        'answer': event.originalEvent.submitter.value.replace(/search-term-|start-date-|qualification-level-/g, ""),
    });
});