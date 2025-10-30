import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    inputText,
    isVisible,
    checkTextContains} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the get help page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the content is on the page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");

        await checkText(page, "#back-button", "Back to get help with the Check an early years qualification service");
        await checkText(page, "#additional-information-heading", "How can we help you?");
        await checkText(page, "#additional-information-hint", "Give as much detail as you can. This helps us give you the right support.");
        await checkText(page, "#warning-text-container > strong", "Warning Do not include any personal information");
        await checkText(page, "#question-submit", "Continue");
    });

    test("Check back button links to correct page depending if QuestionAboutAQualification selected", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/qualification-details");

        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", "1");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", "2001");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");

        await checkText(page, "#back-button", "Back to what are the qualification details");
        await page.click("#back-button");
        await checkUrl(page, "/help/qualification-details");
    });

    test("Check back button links to correct page depending if IssueWithTheService selected", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");

        await checkText(page, "#back-button", "Back to get help with the Check an early years qualification service");
        await page.click("#back-button");
        await checkUrl(page, "/help/get-help");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");

        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await page.goBack();
        await checkText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
    });

    test("Displays an error message when a user doesnt enter required details", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");

        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Provide information about how we can help you");
        await checkTextContains(page, "#additional-information-error", "Provide information about how we can help you");
    });
});