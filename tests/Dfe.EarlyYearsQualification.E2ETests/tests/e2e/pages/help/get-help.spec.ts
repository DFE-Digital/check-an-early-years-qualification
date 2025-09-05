import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    exists,
    checkUrl,
    isVisible,
    checkTextContains,
    checkElementIsChecked
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the get help page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the content is on the page", async ({page, context}) => {
        await page.goto("/help/get-help");

        await checkText(page, "#help-page-heading", "Get help with the Check an early years qualification service");
        await checkText(page, "#post-heading-content", "Use this form to ask a question about a qualification or report a problem with the service or the information it provides. We aim to respond to all queries within 5 working days. Complex cases may take longer.");
        await checkText(page, "#reason-for-enquiry-heading", "Why are you contacting us?");

        await exists(page, "#QuestionAboutAQualification");
        await checkText(page, "label[for='QuestionAboutAQualification']", "I have a question about a qualification");

        await exists(page, "#IssueWithTheService");
        await checkText(page, "label[for='IssueWithTheService']", "I am experiencing an issue with the service");
    
        await checkText(page, "#reason-for-enquiring-form-submit", "Continue")
    });

    test("Checks the QuestionAboutAQualification enquiry reason navigates to correct page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#reason-for-enquiring-form-submit");

        await checkUrl(page, "/help/qualification-details");
    });

    test("Checks the IssueWithTheService enquiry reason navigates to correct page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");

        await checkUrl(page, "/help/provide-details");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");

        await page.goBack();

        await checkElementIsChecked(page, "#IssueWithTheService");
    });

    test("shows an error message when a user doesnt enter required details on help page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#reason-for-enquiring-form-submit");

        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Select one option", 0);

        await isVisible(page, "#option-error");
        await checkTextContains(page, "#option-error", "Select one option");

        await checkText(page, "#reason-for-enquiry-heading", "Why are you contacting us?");
    });
});