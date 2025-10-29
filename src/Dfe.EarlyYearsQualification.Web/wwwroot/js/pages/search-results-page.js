$(window).on('load', function() {
    let qualificationCount = $("#hdnQualificationCount").val();
    let filterCountry = $("#filter-country").text();
    let filterStartDate = $("#filter-start-date").text();
    let filterAwardedDate = $("#filter-awarded-date").text();
    let filterLevel = $("#filter-level").text();
    let filterOrg = $("#filter-org").text();
    let searchTerm = $("#refineSearch").val();
    
    let eventName = qualificationCount === '0' ? 'no-search-results-returned' : 'search-results-returned';

    if (searchTerm !== "") {
        $("#found-heading").attr("tabindex", "-1");
        $("#found-heading").focus();
        $("#found-heading").attr("role", "alert");
        $("#qualification-search-results").attr("role", "alert");

    }

    window.dataLayer.push({
        'event': eventName,
        'filterCountry': filterCountry,
        'filterStartDate': filterStartDate,
        'filterAwardedDate': filterAwardedDate,
        'filterLevel': filterLevel,
        'filterOrg': filterOrg,
        'totalQualifications': qualificationCount,
        'searchTerm': searchTerm
    });
});