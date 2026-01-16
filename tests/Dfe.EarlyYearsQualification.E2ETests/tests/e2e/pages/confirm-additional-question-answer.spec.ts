import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    checkUrl,
    clickBackButton,
    checkDisclaimer,
    exists
} from '../../_shared/playwrightWrapper';

test.describe('A spec used to test the check additional requirements answer page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });

    test("Checks the check additional questions details are on the first question page", async ({page}) => {
        await page.goto("/qualifications/check-additional-questions/EYQ-240/confirm-answers");

        await checkText(page, ".govuk-heading-xl", "Test page heading");
        await checkText(page, "#question-1-question", "Test question");
        await checkText(page, "#question-1-answer", "Yes");
        await checkText(page, "#question-1-change", "Test change answer text Test question");
        await checkText(page, "#question-1-change .govuk-visually-hidden", "Test question");
        await checkText(page, "#question-2-question", "Test question 2");
        await checkText(page, "#question-2-answer", "Yes");
        await checkText(page, "#question-2-change", "Test change answer text Test question 2");
        await checkText(page, "#question-2-change .govuk-visually-hidden", "Test question 2"); 
        await exists(page, '#warning-text-container');
        await checkDisclaimer(page, "Test answer disclaimer text");
        await checkText(page, "#confirm-answers", "Test button text");
    });

    test("Navigates to the correct question page if the user clicks to change an answer", async ({page}) => {
        await page.goto("/qualifications/check-additional-questions/EYQ-240/confirm-answers");

        await page.click("#question-1-change a");
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/1");
        await page.click("#additional-requirement-button");
        await page.click("#additional-requirement-button");
        await page.click("#question-2-change a");
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/2");
    });

    test("Navigates back to the last question when the back button is clicked", async ({page}) => {
        await page.goto("/qualifications/check-additional-questions/EYQ-240/confirm-answers");
        await clickBackButton(page);
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/2");
    });
});