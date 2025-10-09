function getUrls(authSecret){
    let defaultActions = [
        'navigate to http://localhost:5000/challenge',
        'wait for url to be http://localhost:5000/challenge',
        `set field #PasswordValue to ${authSecret}`,
        'click element #question-submit',
        'wait for url to be http://localhost:5000/',
        'click element #start-now-button',
        'wait for url to be http://localhost:5000/questions/pre-check',
        'click element #yes',
        'click element #pre-check-submit',
        'wait for url to be http://localhost:5000/questions/are-you-checking-your-own-qualification',
        'click element #no',
        'click element #question-submit',
        'wait for url to be http://localhost:5000/questions/where-was-the-qualification-awarded',
        'click element #england',
        'click element #question-submit',
        'wait for url to be http://localhost:5000/questions/when-was-the-qualification-started-and-awarded',
        'set field #StartedQuestion.SelectedMonth to 7',
        'set field #StartedQuestion.SelectedYear to 2015',
        'set field #AwardedQuestion.SelectedMonth to 9',
        'set field #AwardedQuestion.SelectedYear to 2017',
        'click element #question-submit',
        'wait for url to be http://localhost:5000/questions/what-level-is-the-qualification',
        'click element #3',
        'click element #question-submit',
        'wait for url to be http://localhost:5000/questions/what-is-the-awarding-organisation',
        'click element #awarding-organisation-not-in-list',
        'click element #question-submit',
        'wait for url to be http://localhost:5000/questions/check-your-answers',
    ];
    
    return [
        {
            url: "http://localhost:5000/",
            actions: defaultActions,
        },
        {
            url: "http://localhost:5000/questions/are-you-checking-your-own-qualification",
            actions: defaultActions,
        }
    ]
}

const config = {
    defaults: {
        standard: 'WCAG2AA',
        chromeLaunchConfig: {
            executablePath: "/usr/bin/google-chrome"
        },
        hideElements: 'svg[role=presentation]'
    },
    // urls: [
    //     "http://localhost:5000/",
    //     "http://localhost:5000/accessibility-statement",
    //     "http://localhost:5000/cookies",
    //     "http://localhost:5000/questions/are-you-checking-your-own-qualification",
    //     "http://localhost:5000/questions/where-was-the-qualification-awarded",
    //     "http://localhost:5000/questions/when-was-the-qualification-started-and-awarded",
    //     "http://localhost:5000/questions/what-level-is-the-qualification",
    //     "http://localhost:5000/questions/what-is-the-awarding-organisation",
    //     "http://localhost:5000/questions/check-your-answers",
    //     "http://localhost:5000/select-a-qualification-to-check",
    //     "http://localhost:5000/confirm-qualification/eyq-240",
    //     "http://localhost:5000/qualifications/qualification-details/eyq-240",
    //     "http://localhost:5000/qualifications/check-additional-questions/eyq-240/1",
    //     "http://localhost:5000/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019",
    //     "http://localhost:5000/advice/qualification-outside-the-united-kingdom",
    //     "http://localhost:5000/advice/qualifications-achieved-in-scotland",
    //     "http://localhost:5000/advice/qualifications-achieved-in-wales",
    //     "http://localhost:5000/advice/qualifications-achieved-in-northern-ireland",
    //     "http://localhost:5000/advice/qualification-not-on-the-list",
    //     "http://localhost:5000/advice/qualification-level-7",
    //     "http://localhost:5000/advice/level-6-qualification-pre-2014",
    //     "http://localhost:5000/advice/level-6-qualification-post-2014",
    //     "http://localhost:5000/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019",
    //     "http://localhost:5000/advice/level-7-qualification-after-aug-2019",
    //     "http://localhost:5000/help/get-help",
    //     "http://localhost:5000/help/qualification-details",
    //     "http://localhost:5000/help/provide-details",
    //     "http://localhost:5000/help/email-address",
    //     "http://localhost:5000/help/confirmation",
    //     "http://localhost:5000/give-feedback",
    //     "http://localhost:5000/give-feedback/confirmation"
    // ]
};

function createPa11yCiConfiguration(urls, defaults) {

    console.error('Env:', process.env.AUTH_SECRET);

    return {
        defaults: defaults,
        urls: getUrls(process.env.AUTH_SECRET)
    }
};

// Important ~ call the function, don't just return a reference to it!
module.exports = createPa11yCiConfiguration(config.defaults);