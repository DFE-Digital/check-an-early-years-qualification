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

        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
    });

    test("Checks the content is on the page", async ({ page }) => {
        await checkText(page, "#back-button", "Back to how can we help you");
        await checkText(page, "#email-address-heading", "What is your email address?");
        await checkText(page, "#email-address-hint", "We will only use this email address to reply to your message");
        await checkText(page, "#question-submit", "Send message");
    });

    test("Check back button links to correct page", async ({ page }) => {
        await checkText(page, "#back-button", "Back to how can we help you");
        await page.click("#back-button");
        await checkUrl(page, "/help/provide-details");
    });

    test("Displays an error message when a user doesnt enter required details", async ({ page }) => {
        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter an email address");
        await checkTextContains(page, "#email-address-error", "Enter an email address");
    });

    test("Displays an error message when a user enters an invalid email address", async ({ page }) => {
        await inputText(page, "#EmailAddress", "this is an invalid email address");

        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter an email address in the correct format, for example name@example.com");
        await checkTextContains(page, "#email-address-error", "Enter an email address in the correct format, for example name@example.com");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page }) => {
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");
        await checkUrl(page, "/help/confirmation");
    });
});