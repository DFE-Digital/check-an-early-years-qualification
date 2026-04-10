import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    inputText,
    isVisible,
    checkTextContains
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the email address page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the technical content is on the page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await checkText(page, "#back-button", "Back to tell us about the technical issue");
        await checkText(page, "#email-address-heading", "What is your email address?");
        await checkText(page, "#email-address-hint", "We will only use this email address if we need more information about the technical issue you are experiencing");
        await checkText(page, "#question-submit", "Send message");
    });

    test("Checks the qualification query content is on the page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");
        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", "1");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", "2001");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await checkText(page, "#back-button", "Back to how can we help you");
        await checkText(page, "#email-address-heading", "What is your email address?");
        await checkText(page, "#email-address-hint", "We will only use this email address to reply to your message");
        await checkText(page, "#question-submit", "Send message");
    });

    test("Displays an error message when a user doesnt enter required details", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await checkUrl(page, "/help/email-address");
        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter an email address");
        await checkTextContains(page, "#email-address-error", "Enter an email address");
    });

    test("Displays an error message when a user enters an invalid email address", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await checkUrl(page, "/help/email-address");
        await inputText(page, "#EmailAddress", "this is an invalid email address");
        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter an email address in the correct format, for example name@example.com");
        await checkTextContains(page, "#email-address-error", "Enter an email address in the correct format, for example name@example.com");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await checkUrl(page, "/help/email-address");
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");
        await checkUrl(page, "/help/confirmation");
    });
});