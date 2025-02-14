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

$("#date-question-form").on("submit", function () {
    let question = $("#question").text();
    let startedMonth = $("#date-started-month").val();
    let startedYear = $('#date-started-year').val();
    let awardedMonth = $("#date-awarded-month").val();
    let awardedYear = $('#date-awarded-year').val();
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
    let questionCount = $("input[name='QuestionCount']").val();
    let qualificationId = $("input[name='qualificationId']").val();
    let questions = new Map();
    let answers = new Map();
    for (let i = 0; i < questionCount; i++) {
        const questionId = `question_${i}`;
        const answerId = `answer_${i}`;
        
        let question = $(`#${questionId}`).text();
        const answerName = `Answers[${question}]`;
        let selectedAnswer = $(`input[name='${answerName}']:checked`).val();
        
        questions.set(questionId, question);
        answers.set(answerId, selectedAnswer)
    }
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

$("#challenge-form").on("submit", function(){
    let passwordValue = $("#PasswordValue").val();
    window.dataLayer.push({
        'event': 'challengePageFormSubmission',
        'challengeValue': passwordValue
    });
});