const config = {
    defaults: {
        standard: 'WCAG2AA',
        chromeLaunchConfig: {
            executablePath: "/usr/bin/google-chrome"
        },
        headers: {
            Cookie: 'auth-secret=${AUTH_SECRET},user_journey=%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A0%7D'
        }
    },
    urls: [
        "http://localhost:5000/",
        "http://localhost:5000/accessibility-statement",
        "http://localhost:5000/cookies",
        "http://localhost:5000/questions/where-was-the-qualification-awarded",
        "http://localhost:5000/questions/when-was-the-qualification-started",
        "http://localhost:5000/questions/what-level-is-the-qualification",
        "http://localhost:5000/questions/what-is-the-awarding-organisation",
        "http://localhost:5000/qualifications",
        "http://localhost:5000/confirm-qualification/eyq-240",
        "http://localhost:5000/qualifications/qualification-details/eyq-240",
        "http://localhost:5000/qualifications/check-additional-questions/eyq-240/1",
        "http://localhost:5000/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019",
        "http://localhost:5000/advice/qualification-outside-the-united-kingdom",
        "http://localhost:5000/advice/qualifications-achieved-in-scotland",
        "http://localhost:5000/advice/qualifications-achieved-in-wales",
        "http://localhost:5000/advice/qualifications-achieved-in-northern-ireland",
        "http://localhost:5000/advice/qualification-not-on-the-list",
        "http://localhost:5000/advice/qualification-level-7",
        "http://localhost:5000/advice/level-6-qualification-pre-2014",
        "http://localhost:5000/advice/level-6-qualification-post-2014",
        "http://localhost:5000/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019",
        "http://localhost:5000/advice/level-7-qualification-after-aug-2019"
    ]
};

function createPa11yCiConfiguration(urls, defaults) {

    console.error('Env:', process.env.AUTH_SECRET);

    defaults.headers.Cookie = defaults.headers.Cookie.replace('${AUTH_SECRET}', process.env.AUTH_SECRET)

    return {
        defaults: defaults,
        urls: urls
    }
};

// Important ~ call the function, don't just return a reference to it!
module.exports = createPa11yCiConfiguration(config.urls, config.defaults);