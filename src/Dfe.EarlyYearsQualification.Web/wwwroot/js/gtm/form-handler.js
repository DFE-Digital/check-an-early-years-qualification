window.dataLayer = window.dataLayer || [];
$("#radio-question-form").on("submit", function(){
    let question = $("#question").text();
    let answer = $("input[name='Option']:checked").val();
    let startedMonth = $('#Month').val();
    let startedYear = $('#Year').val();
    if (startedMonth !== undefined && startedYear !== undefined) {
        window.dataLayer.push({
            'event': 'nestedRadioQuestionFormSubmission',
            'question': question,
            'answer': answer,
            'dateStarted': `${startedMonth}-${startedYear}`,
        });
    }
    else {
        window.dataLayer.push({
            'event': 'radioQuestionFormSubmission',
            'question': question,
            'answer': answer
        });
    }
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
    let awardedMonth = $('#AwardedQuestion\\.SelectedMonth').val();
    let awardedYear = $('#AwardedQuestion\\.SelectedYear').val();
    window.dataLayer.push({
        'event': 'dateQuestionFormSubmission',
        'question': question,
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

$("#clear-search-form").on("submit", function() {
    window.dataLayer.push({
        'event': 'clearSearchFormSubmission'
    });
});

$("#give-feedback-form").on("submit", function () {
    const questionGroups = document.querySelectorAll('#give-feedback-form .govuk-form-group');
    let answeredCount = 0;
    let details = [];

    questionGroups.forEach((group, index) => {
        const questionText = group.querySelector('legend, label')?.innerText.trim() || `Question ${index + 1}`;

        // Check for checked radio buttons or checkboxes
        const hasSelection = group.querySelectorAll('input[type="radio"]:checked, input[type="checkbox"]:checked').length > 0;

        // Check for text inputs or textareas that are not empty
        const textInputs = group.querySelectorAll('input[type="text"], textarea');
        const hasText = Array.from(textInputs).some(input => input.value.trim() !== "");

        if (hasSelection || hasText) {
            answeredCount++;
            details.push({ question: questionText, status: 'Answered' });
        } else {
            details.push({ question: questionText, status: 'Unanswered' });
        }
    });

    let payload = {};

    if (answeredCount > 0) {
        payload.event = 'giveFeedbackFormSubmission';
        // Add each question and its status to the payload
        details.forEach((detail, index) => {
            payload[`question_${index + 1}`] = detail.question;
            payload[`answer_${index + 1}`] = detail.status;
        });
    }
    else {
        payload.event = 'emptyFeedbackFormSubmission';
    }

    window.dataLayer.push(payload);
});

$('#get-help-enquiry-form').on("submit", function() {
    let selectedAnswer = $(`input[name='SelectedOption']:checked`).val();
    window.dataLayer.push({
        'event': 'reasonForEnquiringFormSubmission',
        'answer': selectedAnswer
    });
})

$('#proceed-with-qualification-enquiry-form').on("submit", function () {
    let selectedAnswer = $(`input[name='SelectedOption']:checked`).val();
    window.dataLayer.push({
        'event': 'proceedWithQualificationEnquiryFormSubmission',
        'answer': selectedAnswer
    });
})

$("#email-address-form").on("submit", function(){
    window.dataLayer.push({
        'event': 'helpPageFormSubmission'
    });
});