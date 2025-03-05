$(window).on('load', function() {
    let level = $("#hdnLevelValue").val();
    let dateStarted = $("#hdnStartDateValue").val();
    
    let insetTextValue = $(".govuk-inset-text > .govuk-body").text();
    let splitInsetText = insetTextValue.split('started ');
    let summaryText = splitInsetText[1];
    
    let levelSummary = level === "Any level" ? level : `L${level}`;
    
    window.dataLayer.push({
        'event': 'cannot-find-qualification-page',
        'qualificationLevel': level,
        'dateStarted': dateStarted,
        'summary': `${levelSummary} - ${summaryText}`
    });
});