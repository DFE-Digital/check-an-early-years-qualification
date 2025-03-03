$(window).on('load', function() {
    let qualificationCount = $("#hdnQualificationCount").val();
    let filterCountry = $("#filter-country").val();
    let filterStartDate = $("#filter-start-date").val();
    let filterAwardedDate = $("#filter-awarded-date").val();
    let filterLevel = $("#filter-level").val();
    let filterOrg = $("#filter-org").val();
    let searchTerm = $("#refineSearch").val();
    
    let eventName = qualificationCount === '0' ? 'no-search-results-returned' : 'search-results-returned';
    let eventData = {
        'filterCountry': filterCountry,
        'filterStartDate': filterStartDate,
        'filterAwardedDate': filterAwardedDate,
        'filterLevel': filterLevel,
        'filterOrg': filterOrg,
        'totalQualifications': qualificationCount,
        'searchTerm': searchTerm
    };

    window.dataLayer.push({
        'event': eventName,
        ...eventData
    });
});