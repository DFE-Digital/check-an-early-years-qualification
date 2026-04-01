$(window).on('load', function () {
    let totalQualifications = $('.govuk-summary-card').length;
    let filterKeyword = $('[value^="search-term"] span').first().text();
    let filterStartDate = $('[value^="start-date"] span').first().text();
    let filterQualificationLevel = $('[value^="qualification-level"] span').first().text();

    window.dataLayer.push({
        'event': "webview-results-returned",
        'filterKeyword': filterKeyword,
        'filterStartDate': filterStartDate,
        'filterQualificationLevel': filterQualificationLevel,
        'totalQualifications': totalQualifications
    });
});

$("#remove-filter-form").on("submit", function (event) {
    window.dataLayer.push({
        'event': 'webview-remove-filter',
        'answer': event.originalEvent.submitter.value.replace(/search-term-|start-date-|qualification-level-/g, ""),
    });
});