import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    hasClass,
    hasCount,
    isVisible,
    isNotVisible,
    checkTextContains
} from '../../_shared/playwrightWrapper';

test.describe("A spec used to test the qualification details page", {tag: "@e2e"}, () => {
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
        await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text requirements");
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

    test("When the user selects a qualification that is above a level 2, started between Sept 2014 and Aug 2019, and is not full and relevant due to their answers, they see the level 2 qualification marked as 'Approved'", async ({
                                                                                                                                                                                                                                                        page,
                                                                                                                                                                                                                                                        context
                                                                                                                                                                                                                                                    }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2016%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22no%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");

        await checkText(page, "#ratio-Level2-tag", "Approved");
        await checkText(page, "#ratio-Level2-additional-info", "Level 2 further action required text");
    });


    ["top", "bottom"].forEach((location) => {
        test(`Clicking the ${location} print button brings up the print dialog`, async ({
                                                                                            page,
                                                                                            context
                                                                                        }) => {
            await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
            await page.goto("/qualifications/qualification-details/eyq-240");

            await hasCount(page, '.print-button', 2);
            await page.evaluate('(() => {window.waitForPrintDialog = new Promise(f => window.print = f);})()');
            await page.click(`#print-button-${location}`);
            await page.waitForFunction('window.waitForPrintDialog');
        });
    });
    
    test("Checks the print content shows on the page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22no%22%7D%7D', journeyCookieName);
        await page.goto("/qualifications/qualification-details/eyq-240");
        
        await isVisible(page, '.print-button-container');
        await isVisible(page, '.print-button-container .govuk-details__summary-text');
        await checkText(page, '.print-button-container .govuk-details__summary-text', 'Print information heading');
        await isNotVisible(page, '.print-button-container .govuk-details__text');
        await page.click('.print-button-container .govuk-details__summary-text');
        await isVisible(page, '.print-button-container .govuk-details__text');
        await checkTextContains(page, '.print-button-container .govuk-details__text', 'Print information body');
    })

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
});