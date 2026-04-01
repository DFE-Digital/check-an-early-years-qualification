$(window).on('load', function () {
    let qualificationCount = $('.govuk-summary-card').length;
    let keywordFilter = $('[value^="search-term"] span').first().text();
    let startDateFilter = $('[value^="start-date"] span').first().text();
    let qualificationLevelFilter = $('[value^="qualification-level"] span').first().text();

    window.dataLayer.push({
        'event': "webview-results-returned",
        'keywordFilter': keywordFilter,
        'startDateFilter': startDateFilter,
        'qualificationLevelFilter': qualificationLevelFilter,
        'totalQualifications': qualificationCount
    });
});

$("#remove-filter-form").on("submit", function (event) {
    window.dataLayer.push({
        'event': 'webview-remove-filter',
        'value': event.originalEvent.submitter.value.replace(/search-term-|start-date-|qualification-level-/g, ""),
    });
});