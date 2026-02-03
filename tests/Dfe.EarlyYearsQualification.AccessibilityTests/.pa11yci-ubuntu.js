function getUrls(authSecret, port) {
    let basicActions = [
        `navigate to http://localhost:${port}/challenge`,
        `wait for url to be http://localhost:${port}/challenge`,
        `set field #PasswordValue to ${authSecret}`,
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/`,
        'click element #start-now-button',
        `wait for url to be http://localhost:${port}/questions/pre-check`,
        'click element #yes',
        'click element #pre-check-submit',
        `wait for url to be http://localhost:${port}/questions/are-you-checking-your-own-qualification`,
        'click element #no',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/where-was-the-qualification-awarded`,
    ];

    let fullJourneyActions = [
        ...basicActions,
        'click element #england',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/when-was-the-qualification-started-and-awarded`,
        'set field #started-month-label+input to 7',
        'set field #started-year-label+input to 2015',
        'set field #awarded-month-label+input to 9',
        'set field #awarded-year-label+input to 2017',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/what-level-is-the-qualification`,
        'click element input[id="3"]',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/what-is-the-awarding-organisation`,
        'click element #awarding-organisation-not-in-list',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/check-your-answers`,
        'click element #cta-button',
        `wait for url to be http://localhost:${port}/select-a-qualification-to-check`,
        'click element a[href="/confirm-qualification/EYQ-240"]',
        `wait for url to be http://localhost:${port}/confirm-qualification/EYQ-240`,
        'click element #yes',
        'click element #confirm-qualification-button',
        `wait for url to be http://localhost:${port}/qualifications/check-additional-questions/EYQ-240/1`,
        'click element #yes',
        'click element #additional-requirement-button',
        `wait for url to be http://localhost:${port}/qualifications/check-additional-questions/EYQ-240/2`,
        'click element #no',
        'click element #additional-requirement-button',
        `wait for url to be http://localhost:${port}/qualifications/check-additional-questions/EYQ-240/confirm-answers`,
        'click element #confirm-answers',
        `wait for url to be http://localhost:${port}/qualifications/qualification-details/EYQ-240`,
    ];

    return [
        {
            url: `http://localhost:${port}/`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/`)
        },
        `http://localhost:${port}/accessibility-statement`,
        `http://localhost:${port}/cookies`,
        {
            url: `http://localhost:${port}/questions/are-you-checking-your-own-qualification`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/questions/are-you-checking-your-own-qualification`)
        },
        {
            url: `http://localhost:${port}/questions/where-was-the-qualification-awarded`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/questions/where-was-the-qualification-awarded`)
        },
        {
            url: `http://localhost:${port}/questions/when-was-the-qualification-started-and-awarded`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/when-was-the-qualification-started-and-awarded`)
        },
        {
            url: `http://localhost:${port}/questions/what-level-is-the-qualification`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/what-level-is-the-qualification`)
        },
        {
            url: `http://localhost:${port}/questions/what-is-the-awarding-organisation`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/what-is-the-awarding-organisation`)
        },
        {
            url: `http://localhost:${port}/questions/check-your-answers`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/check-your-answers`)
        },
        {
            url: `http://localhost:${port}/select-a-qualification-to-check`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/select-a-qualification-to-check`)
        },
        {
            url: `http://localhost:${port}/confirm-qualification/EYQ-240`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/confirm-qualification/EYQ-240`)
        },
        {
            url: `http://localhost:${port}/qualifications/qualification-details/EYQ-240`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/qualifications/qualification-details/EYQ-240`)
        },
        {
            url: `http://localhost:${port}/qualifications/check-additional-questions/EYQ-240/1`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/qualifications/check-additional-questions/EYQ-240/1`)
        },
        `http://localhost:${port}/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019`,
        `http://localhost:${port}/advice/qualification-outside-the-united-kingdom`,
        `http://localhost:${port}/advice/qualifications-achieved-in-scotland`,
        `http://localhost:${port}/advice/qualifications-achieved-in-wales`,
        `http://localhost:${port}/advice/qualifications-achieved-in-northern-ireland`,
        `http://localhost:${port}/advice/qualification-not-on-the-list`,
        `http://localhost:${port}/advice/qualification-level-7`,
        `http://localhost:${port}/advice/level-6-qualification-pre-2014`,
        `http://localhost:${port}/advice/level-6-qualification-post-2014`,
        `http://localhost:${port}/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019`,
        `http://localhost:${port}/advice/level-7-qualification-after-aug-2019`,
        `http://localhost:${port}/help/get-help`,
        {
            url: `http://localhost:${port}/help/qualification-details`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #reason-for-enquiring-form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`
            ]
        },
        {
            url: `http://localhost:${port}/help/provide-details`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #reason-for-enquiring-form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`,
                'set field #QualificationName to Testing',
                'set field #awarded-month-label+input to 9',
                'set field #awarded-year-label+input to 2015',
                'set field #AwardingOrganisation to Testing',
                'click element #question-submit',
                `wait for url to be http://localhost:${port}/help/provide-details`
            ]
        },
        {
            url: `http://localhost:${port}/help/email-address`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #reason-for-enquiring-form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`,
                'set field #QualificationName to Testing',
                'set field #awarded-month-label+input to 9',
                'set field #awarded-year-label+input to 2015',
                'set field #AwardingOrganisation to Testing',
                'click element #question-submit',
                `wait for url to be http://localhost:${port}/help/provide-details`,
                'set field #ProvideAdditionalInformation to Test',
                'click element #question-submit',
                `wait for url to be http://localhost:${port}/help/email-address`,
            ]
        },
        `http://localhost:${port}/help/confirmation`,
        `http://localhost:${port}/give-feedback`,
        `http://localhost:${port}/give-feedback/confirmation`
    ];
}

const config = {
    defaults: {
        standard: 'WCAG2AA',
        hideElements: 'svg[role=presentation], img[id="offline-resources-1x"], img[id="offline-resources-2x"]',
        useIncognitoBrowserContext: true,
        wait: 2000,
        timeout: 60000
    }
};

function createPa11yCiConfiguration(defaults) {
    
    let port = process.env.PORT || 5000;
    
    return {
        defaults: defaults,
        urls: getUrls(process.env.AUTH_SECRET, port)
    }
};

// Important ~ call the function, don't just return a reference to it!
module.exports = createPa11yCiConfiguration(config.defaults);