import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    checkError,
    setCookie,
    journeyCookieName,
    isVisible,
    hasAttribute,
    attributeContains
} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the check additional questions page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
    });

    test("Checks the check additional questions details are on the first question page", async ({page}) => {
        await page.goto(`/qualifications/check-additional-questions/eyq-240/1`);

        await hasAttribute(page, "#back-button", 'href');
        await attributeContains(page, "#back-button", 'href', '/select-a-qualification-to-check');

        await checkText(page, '#question', 'Test question');
        await checkText(page, '#hint', 'This is the hint text: answer yes for full and relevant');
        await checkText(page, ".govuk-details__summary-text", "This is the details heading");
        await checkText(page, ".govuk-details__text", "This is the details content");
        await checkText(page, "Label[for='yes']", "Yes");
        await checkText(page, "Label[for='no']", "No");
        await checkText(page, "#additional-requirement-button", "Get result");
    });

    test("Checks the check additional questions details are on the second question page", async ({page}) => {
        await page.goto(`/qualifications/check-additional-questions/eyq-240/2`);

        await hasAttribute(page, "#back-button", 'href');
        await attributeContains(page, "#back-button", 'href', '/qualifications/check-additional-questions');
     
        await checkText(page, '#question', 'Test question 2');
        await checkText(page, '#hint', 'This is the hint text: answer no for full and relevant');
        await checkText(page, ".govuk-details__summary-text", "This is the details heading");
        await checkText(page, ".govuk-details__text", "This is the details content");
        await checkText(page, "Label[for='yes']", "Yes");
        await checkText(page, "Label[for='no']", "No");
        await checkText(page, "#additional-requirement-button", "Get result");
    });

    test("Shows errors if user does not select an option", async ({page}) => {
        await page.goto(`/qualifications/check-additional-questions/eyq-240/1`);

        await page.click("#additional-requirement-button");

        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There was a problem");
        await checkText(page, "#error-banner-link-yes", "This is a test error message");
        await isVisible(page, "#option-error");
        await checkError(page, "#option-error", "This is a test error message");
        await isVisible(page, ".govuk-form-group--error");
    });
});