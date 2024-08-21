window.dataLayer = window.dataLayer || [];
$("#radio-question-form").on("submit", function(){
    let question = $("#question").text();
    let answer = $("input[name='Option']:checked").val();
    window.dataLayer.push({
        'event': 'radioQuestionFormSubmission',
        'question': question,
        'answer': answer
    });
});

$("#date-question-form").on("submit", function(){
    let question = $("#question").text();
    let month = $("#date-started-month").val();
    let year = $('#date-started-year').val();
    window.dataLayer.push({
        'event': 'dateQuestionFormSubmission',
        'question': question,
        'dateStarted': `${month}-${year}`
    });
});

$("#dropdown-question-form").on("submit", function(){
    let question = $("#question").text();
    let selectedAO = $("#awarding-organisation-select :selected").val();
    let isNotOnTheListChecked = $("#awarding-organisation-not-in-list").is(":checked");
    window.dataLayer.push({
        'event': 'dropdownQuestionFormSubmission',
        'question': question,
        'selectedAwardingOrganisation': selectedAO,
        'isNotOnTheListChecked': isNotOnTheListChecked
    });
});