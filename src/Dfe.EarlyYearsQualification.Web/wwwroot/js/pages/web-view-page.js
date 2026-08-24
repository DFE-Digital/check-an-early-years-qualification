$(window).on('load', function () {
    let totalQualifications = $('.govuk-summary-card').length;
    window.dataLayer.push({
        'event': "webview-results-returned",
        'totalQualifications': totalQualifications,
        ...getCurrentFilters()
    });
});

$("#remove-filter-form").on("submit", function (event) {
    window.dataLayer.push({
        'event': 'webview-remove-filter',
        'answer': event.originalEvent.submitter.value.replace(/search-term-|start-date-|nation-|qualification-level-/g, ""),
    });
});

$(window).on('afterprint', function () {
    window.dataLayer.push({
        'event': "webview-page-printed",
        ...getCurrentFilters()
    });
});

function getCurrentFilters() {
    const prefixes = ['search-term', 'nation', 'start-date', 'qualification-level'];
    const filters = {};

    prefixes.forEach(prefix => {
        // Formats string to camelCase matching your dataLayer keys
        const key = prefix.replace(/-([a-z])/g, g => g[1].toUpperCase());
        // Map 'searchTerm' or 'filter' prefix correctly
        const dataLayerKey = key === 'searchTerm' ? key : 'filter' + key.charAt(0).toUpperCase() + key.slice(1);

        filters[dataLayerKey] = $(`[value^="${prefix}"] span`).first().text();
    });

    return filters;
}