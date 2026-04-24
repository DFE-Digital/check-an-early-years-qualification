function getUrls(authSecret, port) {
    let basicActions = [
        `navigate to http://localhost:${port}/challenge`,
        `wait for url to be http://localhost:${port}/challenge`,
        `set field #PasswordValue to ${authSecret}`,
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/`,
        'click element h1',
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
        `wait for url to be http://localhost:${port}/questions/when-was-the-qualification-started`,
        'click element #OnOrAfter1September2014',
        'set field #question-month-label+input to 7',
        'set field #question-year-label+input to 2020',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/when-was-the-qualification-awarded`,
        'set field #awarded-month-label+input to 9',
        'set field #awarded-year-label+input to 2020',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/what-level-is-the-qualification`,
        'click element input[id="3"]',
        'click element #question-submit',
        `wait for url to be http://localhost:${port}/questions/what-is-the-awarding-organisation`,
        'set field #awarding-organisation-select to NCFE',
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
        {
            url: `http://localhost:${port}/accessibility-statement`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/accessibility-statement`)
        },
        {
            url: `http://localhost:${port}/cookies`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/cookies`)
        },
        {
            url: `http://localhost:${port}/questions/are-you-checking-your-own-qualification`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/questions/are-you-checking-your-own-qualification`)
        },
        {
            url: `http://localhost:${port}/questions/where-was-the-qualification-awarded`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/questions/where-was-the-qualification-awarded`)
        },
        {
            url: `http://localhost:${port}/questions/when-was-the-qualification-started`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/when-was-the-qualification-started`)
        },
        {
            url: `http://localhost:${port}/questions/when-was-the-qualification-awarded`,
            actions: fullJourneyActions.concat(`navigate to http://localhost:${port}/questions/when-was-the-qualification-awarded`)
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
        {
            url: `http://localhost:${port}/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019`)
        },
        {
            url: `http://localhost:${port}/advice/qualification-outside-the-united-kingdom`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualification-outside-the-united-kingdom`)
        },
        {
            url: `http://localhost:${port}/advice/qualifications-achieved-in-scotland`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualifications-achieved-in-scotland`)
        },
        {
            url: `http://localhost:${port}/advice/qualifications-achieved-in-wales`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualifications-achieved-in-wales`)
        },
        {
            url: `http://localhost:${port}/advice/qualifications-achieved-in-northern-ireland`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualifications-achieved-in-northern-ireland`)
        },
        {
            url: `http://localhost:${port}/advice/qualification-not-on-the-list`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualification-not-on-the-list`)
        },
        {
            url: `http://localhost:${port}/advice/qualification-level-7`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/qualification-level-7`)
        },
        {
            url: `http://localhost:${port}/advice/level-6-qualification-pre-2014`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/level-6-qualification-pre-2014`)
        },
        {
            url: `http://localhost:${port}/advice/level-6-qualification-post-2014`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/level-6-qualification-post-2014`)
        },
        {
            url: `http://localhost:${port}/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019`)
        },
        {
            url: `http://localhost:${port}/advice/level-7-qualification-after-aug-2019`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/level-7-qualification-after-aug-2019`)
        },
        `http://localhost:${port}/advice/level-7-qualification-after-aug-2019`,
        {
            url: `http://localhost:${port}/advice/nursing-qualifications`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/advice/nursing-qualifications`)
        },
        {
            url: `http://localhost:${port}/help/I-need-a-copy-of-the-qualification-certificate-or-transcript`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/help/I-need-a-copy-of-the-qualification-certificate-or-transcript`)
        },
        {
            url: `http://localhost:${port}/help/I-do-not-know-what-level-the-qualification-is`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/help/I-do-not-know-what-level-the-qualification-is`)
        },
        {
            url: `http://localhost:${port}/help/I-want-to-check-whether-a-course-is-approved-before-I-enrol`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/help/I-want-to-check-whether-a-course-is-approved-before-I-enrol`)
        },
        {
            url: `http://localhost:${port}/help/get-help`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/help/get-help`)
        },
        {
            url: `http://localhost:${port}/early-years-qualification-list`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/early-years-qualification-list`)
        },
        {
            url: `http://localhost:${port}/help/proceed-with-qualification-query`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/proceed-with-qualification-query`
            ]
        },
        {
            url: `http://localhost:${port}/help/qualification-details`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/proceed-with-qualification-query`,
                'click element #ContactTheEarlyYearsQualificationTeam',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`
            ]
        },
        {
            url: `http://localhost:${port}/help/provide-details`,
            actions: [
                ...basicActions,
                `navigate to http://localhost:${port}/help/get-help`,
                'click element #QuestionAboutAQualification',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/proceed-with-qualification-query`,
                'click element #ContactTheEarlyYearsQualificationTeam',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`,
                'click element #QualificationName',
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
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/proceed-with-qualification-query`,
                'click element #ContactTheEarlyYearsQualificationTeam',
                'click element #form-submit',
                `wait for url to be http://localhost:${port}/help/qualification-details`,
                'click element #QualificationName',
                'set field #QualificationName to Testing',
                'set field #awarded-month-label+input to 9',
                'set field #awarded-year-label+input to 2015',
                'set field #AwardingOrganisation to Testing',
                'click element #question-submit',
                `wait for url to be http://localhost:${port}/help/provide-details`,
                'click element #ProvideAdditionalInformation',
                'set field #ProvideAdditionalInformation to Test',
                'click element #question-submit',
                `wait for url to be http://localhost:${port}/help/email-address`,
            ]
        },
        {
            url: `http://localhost:${port}/help/confirmation`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/help/confirmation`)
        },
        {
            url: `http://localhost:${port}/give-feedback`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/give-feedback`)
        },
        {
            url: `http://localhost:${port}/give-feedback/confirmation`,
            actions: basicActions.concat(`navigate to http://localhost:${port}/give-feedback/confirmation`)
        }
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