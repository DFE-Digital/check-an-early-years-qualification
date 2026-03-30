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
    
    // get all required fields and ensure they have a value
    let requiredElements = $("input[isrequired='True']");
    let requiredElementNames = [];
    $.each(requiredElements, function(index, value) { requiredElementNames.push(value.name); });
    let uniqueNames = requiredElementNames.filter((name, index, requiredElementNames) => {
        return index === requiredElementNames.indexOf(name);
    });

    let hasRequiredElements = true;
    $.each(uniqueNames, function(index, value) {
        let baseSelector = "input[name='" + value + "']";
        let checkedSelector = baseSelector + ":checked";
        let selector = $(baseSelector).hasClass("govuk-radios__input") ? checkedSelector : baseSelector;
        let isConditionalInput = $(baseSelector).hasClass("conditional-input");
        
        if (isConditionalInput) {
            // get paired input name and get value to ensure checked value matches
            let pairedInputName = $(baseSelector).parent().parent().parent().children("input:first").attr("name");
            let pairedInputSelector = "input[name='" + pairedInputName + "']";
            let pairedInputValue = $(pairedInputSelector).val();
            
            // Only check the conditional input if it's paired value has been selected
            if (pairedInputValue !== $(pairedInputSelector+":checked").val()) {
                return;
            }
        }
        
        if ($(selector).val() === undefined || $(selector).val().length <= 0) {
            hasRequiredElements = false;
        }
    });

    // only submit the event if all the required inputs have values
    if (hasRequiredElements) {
        window.dataLayer.push({
            'event': 'giveFeedbackFormSubmission',
            'question': question,
            'answer': answer
        });
    }
});

$('#get-help-enquiry-form').on("submit", function(){
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