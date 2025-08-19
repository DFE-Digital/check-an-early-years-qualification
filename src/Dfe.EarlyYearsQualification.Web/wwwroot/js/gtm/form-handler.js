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

$("#pre-check-question-form").on("submit", function() {
    let question = $("#question").text();
    let answer = $("input[name='Option']:checked").val();
    window.dataLayer.push({
        'event': 'preCheckQuestionFormSubmission',
        'question': question,
        'answer': answer
    });
})

$("#date-question-form").on("submit", function () {
    let question = $("#question").text();
    let startedMonth = $('#StartedQuestion\\.SelectedMonth').val();
    let startedYear = $('#StartedQuestion\\.SelectedYear').val();
    let awardedMonth = $('#AwardedQuestion\\.SelectedMonth').val();
    let awardedYear = $('#AwardedQuestion\\.SelectedYear').val();
    window.dataLayer.push({
        'event': 'dateQuestionFormSubmission',
        'question': question,
        'dateStarted': `${startedMonth}-${startedYear}`,
        'dateAwarded': `${awardedMonth}-${awardedYear}`,
    });
});

$("#dropdown-question-form").on("submit", function(){
    const question = $("#question").text();
    const selectedAO = $("#awarding-organisation-select :selected").val();
    const isNotOnTheListChecked = $("#awarding-organisation-not-in-list").is(":checked");
    const eventName = 'dropdownQuestionFormSubmission';
    
    const payload = isNotOnTheListChecked ? 
        {
            'event': eventName,
            'question': question,
            'isNotOnTheListChecked': isNotOnTheListChecked
        }
    :
        {
            'event': eventName,
            'question': question,
            'selectedAwardingOrganisation': selectedAO
        };
    
    window.dataLayer.push(payload);
});

$("#confirm-qualification").on("submit", function(){
    let question = $("#radio-heading").text();
    let qualificationId = $("input[name='qualificationId']").val();
    let answer = $("input[name='ConfirmQualificationAnswer']:checked").val();
    window.dataLayer.push({
        'event': 'confirmQualificationFormSubmission',
        'qualificationId': qualificationId,
        'answer': answer,
        'question': question
    });
});

$("#check-additional-requirements").on("submit", function() {
    let qualificationId = $("input[name='qualificationId']").val();
    let questionIndex = $("input[name='questionIndex']").val();
    let question = $("input[name='question']").val();
    let selectedAnswer = $(`input[name='Answer']:checked`).val();

    const questionId = `question_${questionIndex}`;
    const answerId = `answer_${questionIndex}`;
    
    let questions = new Map();
    let answers = new Map();
    questions.set(questionId, question);
    answers.set(answerId, selectedAnswer)

    let questionsObj = Object.fromEntries(questions);
    let answersObj = Object.fromEntries(answers);
    
    window.dataLayer.push({
        'event': 'checkAdditionalRequirementsFormSubmission',
        'qualificationId': qualificationId,
        ...questionsObj,
        ...answersObj
    });
});

$("#refine-search-form").on("submit", function(){
    let searchTerm = $("#refineSearch").val();
    window.dataLayer.push({
        'event': 'refineSearchFormSubmission',
        'searchTerm': searchTerm
    });
});

$("#clear-search-form").on("submit", function(){
    window.dataLayer.push({
        'event': 'clearSearchFormSubmission'
    });
});

$("#give-feedback-form").on("submit", function () {
    // get first radio question container
    let firstRadioQuestionParent = $("div[class='govuk-radios']").first().parent();
    let question = $(firstRadioQuestionParent).children("h2:first").text();
    let inputName = $(firstRadioQuestionParent).children(".govuk-radios")
        .children(".govuk-radios__item")
        .children("input:first")
        .attr("name");
    let inputSelector = "input[name='" + inputName + "']";
    let answer = $(inputSelector).val();

    window.dataLayer.push({
        'event': 'giveFeedbackFormSubmission',
        'question': question,
        'answer': answer
    });
});