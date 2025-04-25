import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    hasClass,
    hasCount,
    checkJourneyCookieValue,
    setJourneyState,
    doesNotExist
} from '../shared/playwrightWrapper';

test.describe("A spec used to test the qualification details page", () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
    });

    test("Checks the qualification details are on the page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#page-header", "Test Main Heading");
        await checkText(page, "#qualification-details-header", "Qualification details");
        await checkText(page, "#qualification-name-label", "Qualification");
        await checkText(page, "#qualification-name-value", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
        await checkText(page, "#awarding-organisation-label", "Awarding Org Label");
        await checkText(page, "#awarding-organisation-value", "NCFE");
        await checkText(page, "#qualification-level-label", "Test Level Label");
        await checkText(page, "#qualification-level-value", "3");
        await checkText(page, "#date-started-date-label", "Qualification start date");
        await checkText(page, "#date-started-date-value", "July 2015");
        await checkText(page, "#date-awarded-date-value", "January 2025");
        await checkText(page, "#additional-requirement-0-label", "This is the confirmation statement 1");
        await checkText(page, "#additional-requirement-0-value", "Yes");
        await checkText(page, "#additional-requirement-1-label", "This is the confirmation statement 2");
        await checkText(page, "#additional-requirement-1-value", "No");
        await checkText(page, "#date-of-check-label", "Test Date Of Check Label");
        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await checkText(page, "#requirements-heading", "Test requirements heading");
        await checkText(page, "#requirements-heading + p[class='govuk-body']", "This is the requirements text");
        await checkText(page, "#check-another-qualification-link", "Check another qualification");
        await checkText(page, ".govuk-notification-banner__title", "Test banner title", 0);
        await checkText(page, ".govuk-notification-banner__heading", "Test heading", 0);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Test body", 0);
        await checkText(page, ".govuk-notification-banner__title", "Test banner title", 1);
        await checkText(page, ".govuk-notification-banner__heading", "Test heading", 1);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Test body", 1);
    });

    test("Checks the order of the ratios for a level 6 qualification when a user answers yes to the Qts Question", async ({
                                                                                                                              page,
                                                                                                                              context
                                                                                                                          }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%226%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22This%20is%20the%20Qts%20question%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-108");

        await hasCount(page, ".ratio-row", 4);
        await checkText(page, ".ratio-heading", "Level 6", 0);
        await checkText(page, ".ratio-heading", "Level 3", 1);
        await checkText(page, ".ratio-heading", "Level 2", 2);
        await checkText(page, ".ratio-heading", "Unqualified", 3);
        await checkText(page, ".govuk-tag", "Approved", 1);
        await checkText(page, ".govuk-tag", "Approved", 2);
        await checkText(page, ".govuk-tag", "Approved", 3);
        await checkText(page, ".govuk-tag", "Approved", 4);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 1);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 2);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 3);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 4);
    });

    test("Checks the order of the ratios for a level 6 qualification when a user answers no to the Qts Question but yes to the remaining question", async ({
                                                                                                                                                               page,
                                                                                                                                                               context
                                                                                                                                                           }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%226%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22This%20is%20the%20Qts%20question%22%3A%22no%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-108");

        await hasCount(page, ".ratio-row", 4);
        await checkText(page, ".ratio-heading", "Level 3", 0);
        await checkText(page, ".ratio-heading", "Level 2", 1);
        await checkText(page, ".ratio-heading", "Unqualified", 2);
        await checkText(page, ".ratio-heading", "Level 6", 3);
        await checkText(page, ".govuk-tag", "Approved", 1);
        await checkText(page, ".govuk-tag", "Approved", 2);
        await checkText(page, ".govuk-tag", "Approved", 3);
        await checkText(page, ".govuk-tag", "Not approved", 4);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 1);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 2);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 3);
        await hasClass(page, ".govuk-tag", /govuk-tag--red/, 4);
    });

    test("Checks the order of the ratios on the page when a user answers additional requirement questions indicating full and relevant", async ({
                                                                                                                                                    page,
                                                                                                                                                    context
                                                                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await hasCount(page, ".ratio-row", 4);
        await checkText(page, ".ratio-heading", "Level 3", 0);
        await checkText(page, ".ratio-heading", "Level 2", 1);
        await checkText(page, ".ratio-heading", "Unqualified", 2);
        await checkText(page, ".ratio-heading", "Level 6", 3);
        await checkText(page, ".govuk-tag", "Approved", 1);
        await checkText(page, ".govuk-tag", "Approved", 2);
        await checkText(page, ".govuk-tag", "Approved", 3);
        await checkText(page, ".govuk-tag", "Not approved", 4);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 1);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 2);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 3);
        await hasClass(page, ".govuk-tag", /govuk-tag--red/, 4);
    });

    test("Checks the order of the ratios on the page when a user answers an additional requirement question indicating not full and relevant", async ({
                                                                                                                                                          page,
                                                                                                                                                          context
                                                                                                                                                      }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-241");

        await hasCount(page, ".ratio-row", 4);
        await checkText(page, ".ratio-heading", "Unqualified", 0);
        await checkText(page, ".ratio-heading", "Level 2", 1);
        await checkText(page, ".ratio-heading", "Level 3", 2);
        await checkText(page, ".ratio-heading", "Level 6", 3);
        await checkText(page, ".govuk-tag", "Approved", 1);
        await checkText(page, ".govuk-tag", "Not approved", 2);
        await checkText(page, ".govuk-tag", "Not approved", 3);
        await checkText(page, ".govuk-tag", "Not approved", 4);
        await hasClass(page, ".govuk-tag", /govuk-tag--green/, 1);
        await hasClass(page, ".govuk-tag", /govuk-tag--red/, 2);
        await hasClass(page, ".govuk-tag", /govuk-tag--red/, 3);
        await hasClass(page, ".govuk-tag", /govuk-tag--red/, 4);
    });

    test("Checks the staff ratio text shows correctly when not full and relevant for a L3+ qualification started between Sep14 & Aug19", async ({
                                                                                                                                                    page,
                                                                                                                                                    context
                                                                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
    });

    test("Checks the staff ratio text shows correctly when not full and relevant for a L3+ qualification started after Sep19", async ({
                                                                                                                                          page,
                                                                                                                                          context
                                                                                                                                      }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2210%2F2019%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
    });

    test("When the user selects a qualification that is above a level 2, started between Sept 2014 and Aug 2019, and is not full and relevant with no questions, they see the level 2 qualification marked as 'Further action required'", async ({
                                                                                                                                                                                                                                                     page,
                                                                                                                                                                                                                                                     context
                                                                                                                                                                                                                                                 }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2016%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%225%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-114");

        await checkText(page, "#ratio-Level2-tag", "Further action required");
        await checkText(page, "#ratio-Level2-additional-info", "Level 2 further action required text");
    });

    test("When the user selects a qualification that is above a level 2, started between Sept 2014 and Aug 2019, and is not full and relevant due to their answers, they see the level 2 qualification marked as 'Further action required'", async ({
                                                                                                                                                                                                                                                        page,
                                                                                                                                                                                                                                                        context
                                                                                                                                                                                                                                                    }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2016%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22no%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-Level2-tag", "Further action required");
        await checkText(page, "#ratio-Level2-additional-info", "Level 2 further action required text");
    });

    test("Clicking the print button brings up the print dialog", async ({
                                                                            page,
                                                                            context
                                                                        }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await page.evaluate('(() => {window.waitForPrintDialog = new Promise(f => window.print = f);})()');
        await page.click('#print-button');
        await page.waitForFunction('window.waitForPrintDialog');
    });

    test("Checks the qualification result inset shows correctly when not full and relevant for a L3+ qualification started between Sep14 & Aug19", async ({
                                                                                                                                                              page,
                                                                                                                                                              context
                                                                                                                                                          }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 body");
    });

    test("Checks the qualification result inset shows correctly when not full and relevant for a L3+ qualification started after Sep19", async ({
                                                                                                                                                    page,
                                                                                                                                                    context
                                                                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2210%2F2019%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant body");
    });

    test("Checks the qualification result inset shows correctly when full and relevant", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Full and relevant");
        await checkText(page, "#qualification-result-message-body", "Full and relevant body");
    });


    test("Checks default cookie", async ({
                                             page,
                                             context
                                         }) => {

        await page.goto("/");

        await checkJourneyCookieValue(context, '%7B%22WhereWasQualificationAwarded%22%3A%22%22%2C%22WhenWasQualificationStarted%22%3A%22%22%2C%22WhenWasQualificationAwarded%22%3A%22%22%2C%22LevelOfQualification%22%3A%22%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A0%7D');


        await setJourneyState({
            context: context,
            location: null,
            startDate: null,
            awardDate: null,
            level: null,
            organisation: null,
            organisationNotOnList: null,
            searchCriteria: null,
            additionalQuestions: null,
            selectedFromList: null
        });

        await checkJourneyCookieValue(context, '%7B%22WhereWasQualificationAwarded%22%3A%22%22%2C%22WhenWasQualificationStarted%22%3A%22%22%2C%22WhenWasQualificationAwarded%22%3A%22%22%2C%22LevelOfQualification%22%3A%22%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A0%7D');
    });


    test("Checks the qualification result inset shows correctly when full and relevant level 2", async ({
                                                                                                            page,
                                                                                                            context
                                                                                                        }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [10, 2019],
            awardDate: [1, 2025],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Full and relevant");
        await checkText(page, "#qualification-result-message-body", "Full and relevant body");
    });


    test("Checks the qualification result inset shows correctly when not full and relevant level 2", async ({
                                                                                                                page,
                                                                                                                context
                                                                                                            }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [10, 2019],
            awardDate: [1, 2025],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant body");
    });

    test('Checks the qualification result inset shows not full and relevant at level 3 qual started sept 2014 -> aug 2019 (level 3)', async ({
                                                                                                                                                 page,
                                                                                                                                                 context
                                                                                                                                             }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2018],
            awardDate: [1, 2020],
            level: 3,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-240');

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 body");
    });

    test('Checks the qualification result inset shows not full and relevant at level 3 qual started sept 2014 -> aug 2019 (level 4)', async ({
                                                                                                                                                 page,
                                                                                                                                                 context
                                                                                                                                             }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2018],
            awardDate: [1, 2020],
            level: 4,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-105');

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 body");
    });

    test('Checks the qualification result inset shows not full and relevant at level 3 qual started sept 2014 -> aug 2019 (level 5)', async ({
                                                                                                                                                 page,
                                                                                                                                                 context
                                                                                                                                             }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2018],
            awardDate: [1, 2020],
            level: 5,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-107');

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 body");
    });

    test('Checks the qualification result inset shows not full and relevant at level 3 or level 6 qual started sept 2014 -> aug 2019 (level 6)', async ({
                                                                                                                                                            page,
                                                                                                                                                            context
                                                                                                                                                        }) => {

        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2018],
            awardDate: [1, 2020],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-108");

        await checkText(page, "#qualification-result-heading", "Qualification result heading");
        await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3 or L6");
        await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 or L6 body");
    });

    test('Checks level 2 not F&R sees no content under ratio header', async ({
                                                                                 page,
                                                                                 context
                                                                             }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [6, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 2 F&R awarded before June 2016 sees no content under ratio header', async ({
                                                                                                      page,
                                                                                                      context
                                                                                                  }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [5, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 2 F&R awarded in June 2016 sees additional requirement maybe content', async ({
                                                                                                         page,
                                                                                                         context
                                                                                                     }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [6, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 2 F&R awarded after June 2016 sees additional requirement will content', async ({
                                                                                                           page,
                                                                                                           context
                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [7, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-241");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 3 F&R awarded before September 2014 sees no content under ratio header', async ({
                                                                                                           page,
                                                                                                           context
                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 3,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 4 F&R awarded before September 2014 sees no content under ratio header', async ({
                                                                                                           page,
                                                                                                           context
                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 4,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-105');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 5 F&R awarded before September 2014 sees no content under ratio header', async ({
                                                                                                           page,
                                                                                                           context
                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 5,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-107');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 3 F&R awarded on or after September 2014 sees additional requirement maybe content', async ({
                                                                                                                       page,
                                                                                                                       context
                                                                                                                   }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [9, 2014],
            level: 3,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 4 F&R awarded before September 2014 sees additional requirement maybe content', async ({
                                                                                                                  page,
                                                                                                                  context
                                                                                                              }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [9, 2014],
            level: 4,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-105');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 5 F&R awarded before September 2014 sees additional requirement maybe content', async ({
                                                                                                                  page,
                                                                                                                  context
                                                                                                              }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [9, 2014],
            level: 5,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-107');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });


    test('Checks level 6 F&R (all levels) sees no content under ratio header', async ({
                                                                                          page,
                                                                                          context
                                                                                      }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-109');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 7 F&R (all levels) sees no content under ratio header', async ({
                                                                                          page,
                                                                                          context
                                                                                      }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 7,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-111');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 6 F&R (all but L6) awarded before September 2014 sees no content under ratio header', async ({
                                                                                                                        page,
                                                                                                                        context
                                                                                                                    }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-109');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 7 F&R (all but L6) awarded before September 2014 sees no content under ratio header', async ({
                                                                                                                        page,
                                                                                                                        context
                                                                                                                    }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [8, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-111');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 6 F&R (all but L6) awarded on September 2014 sees additional requirement maybe content', async ({
                                                                                                                           page,
                                                                                                                           context
                                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [9, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-109');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 7 F&R (all but L6) awarded on September 2014 sees additional requirement maybe content', async ({
                                                                                                                           page,
                                                                                                                           context
                                                                                                                       }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [9, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-111');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 6 F&R (all but L6) awarded after September 2014 sees additional requirement maybe content', async ({
                                                                                                                              page,
                                                                                                                              context
                                                                                                                          }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [10, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-109');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    test('Checks level 7 F&R (all but L6) awarded after September 2014 sees additional requirement maybe content', async ({
                                                                                                                              page,
                                                                                                                              context
                                                                                                                          }) => {
        await setJourneyState({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [10, 2014],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true
        });

        await page.goto('/qualifications/qualification-details/eyq-111');

        await checkText(page, "#ratio-heading", "Test ratio heading");
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe PFA");
        await doesNotExist(page, "#ratio-additional-info");
    });

    [
        [1, 2013],
        [1, 2014],
        [8, 2014],
        [9, 2019],
        [1, 2020],
    ].forEach((startDate) => {
        test(`(${startDate}) Checks level 3 not F&R started  before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text`, async ({
                                                                                                                                                                        page,
                                                                                                                                                                        context
                                                                                                                                                                    }) => {

            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 3,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-240');

            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 4 not F&R started  before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text`, async ({
                                                                                                                                                                        page,
                                                                                                                                                                        context
                                                                                                                                                                    }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 4,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-105');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 5 not F&R started  before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text`, async ({
                                                                                                                                                                        page,
                                                                                                                                                                        context
                                                                                                                                                                    }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 5,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-107');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 6 not F&R started  before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text`, async ({
                                                                                                                                                                        page,
                                                                                                                                                                        context
                                                                                                                                                                    }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 6,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-109');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 7 not F&R started  before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text`, async ({
                                                                                                                                                                        page,
                                                                                                                                                                        context
                                                                                                                                                                    }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 7,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-111');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });
    });


    [
        [9, 2014],
        [1, 2018],
        [8, 2019]
    ].forEach((startDate) => {
        test(`(${startDate}) Checks level 3 not F&R started between September 2014 and August 2019 sees correct content`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {

            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 3,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-240');

            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 4 not F&R started between September 2014 and August 2019 sees correct content`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 4,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-105');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 5 not F&R started between September 2014 and August 2019 sees correct content`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 5,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-107');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 6 not F&R started between September 2014 and August 2019 sees correct content`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 6,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-109');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });

        test(`(${startDate}) Checks level 7 not F&R started between September 2014 and August 2019 sees correct content`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {
            await setJourneyState({
                context: context,
                location: "england",
                startDate: startDate,
                awardDate: [12, 2020],
                level: 7,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            });

            await page.goto('/qualifications/qualification-details/eyq-111');
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
            await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
        });
    });
});
