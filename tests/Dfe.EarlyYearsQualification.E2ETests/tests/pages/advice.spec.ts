import {Page, test} from '@playwright/test';
import {
    startJourney,
    checkText,
    inputText,
    setCookie,
    journeyCookieName,
    exists,
    doesNotExist,
    doesNotHaveClass,
    checkUrl,
    isVisible,
    checkTextContains
} from '../shared/playwrightWrapper';

async function checkFeedbackBanners(page: Page) {
    await checkText(page, ".govuk-notification-banner__title", "Test banner title", 0);
    await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 0);
    await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 0);

    await checkText(page, ".govuk-notification-banner__title", "Test banner title", 1);
    await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 1);
    await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 1);
}

test.describe('A spec that tests advice pages', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the Qualifications achieved outside the United Kingdom details are on the page", async ({page}) => {
        await page.goto("/advice/qualification-outside-the-united-kingdom");
        await checkText(page, "#advice-page-heading", "Qualifications achieved outside the United Kingdom");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Scotland details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-scotland");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Scotland");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Wales details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-wales");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Wales");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Northern Ireland details are on the page", async ({page}) => {

        await page.goto("advice/qualifications-achieved-in-northern-ireland");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Northern Ireland");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the level 7 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 7 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Level 7 qualification after aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2020%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualification-after-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 7 qualification after aug 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Help details are on the page", async ({page, context}) => {

        await page.goto("/advice/help");
        await checkText(page, "#help-page-heading", "Help Page Heading");
        await checkText(page, "#post-heading-content", "This is the post heading text");
        await checkText(page, "#reason-for-enquiry-heading", "Choose the reason of your enquiry");
        await checkText(page, "#reason-for-enquiry-heading-hint", "Select one option");
        await exists(page, "#Option\\ 1");
        await exists(page, "#Option\\ 2");
        await exists(page, "#Option\\ 3");
        await checkText(page, '#additional-information-heading > label', "Provide further information about your enquiry");
        await checkText(page, '#additional-information-hint', "Provide details about the qualification you are checking for or the specific issue you are experiencing with the service.");
        await checkText(page, "#warning-text-container > .govuk-warning-text__text", "Warning:Do not include personal information, for example the name of the qualification holder");
        await checkText(page, "#email-address-heading > label", "Enter your email address (optional)");
        await checkText(page, "#email-address-hint", "If you do not enter your email address we will not be able to contact you in relation to your enquiry");
        await checkText(page, "#help-form-submit", "Send message")
    });

    test("shows an error message when a user doesnt enter required details on help page", async ({page}) => {
        await page.goto("/advice/help");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#option-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.click("#help-form-submit");
        await checkUrl(page, "/advice/help");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Select one option", 0);
        await checkText(page, ".govuk-error-summary__list > li", "Enter further information about your enquiry", 1);
        await checkText(page, ".govuk-error-summary__list > li", "Enter an email address", 2);
        await isVisible(page, "#option-error");
        await isVisible(page, "#additional-information-error");
        await isVisible(page, "#email-address-error");
        await checkTextContains(page, "#option-error", "Select one option");
        await checkTextContains(page, "#additional-information-error", "Enter further information about your enquiry");
        await checkTextContains(page, "#email-address-error", "Enter an email address");
    });

    test("shows an error message when a user doesnt enter a valid email address on help page", async ({page}) => {
        await page.goto("/advice/help");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#option-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await inputText(page, "#EmailAddress", "test");
        await page.click("#help-form-submit");
        await checkUrl(page, "/advice/help");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter a valid email address", 2);
        await isVisible(page, "#email-address-error");
        await checkTextContains(page, "#email-address-error", "Enter a valid email address");
    });

    test("Checks the details are on the help confirmation page", async ({page}) => {
        await page.goto("/advice/help/confirmation");

        await isVisible(page, "#success-message");
        await isVisible(page, "#help-confirmation-body-heading");
        await isVisible(page, "#help-confirmation-body");
        
        await checkText(page, "#success-message" ,"This is the success message");
        await checkText(page, "#help-confirmation-body-heading" ,"Body heading");
        await checkText(page, "#help-confirmation-body" ,"This is the body");
    });
});