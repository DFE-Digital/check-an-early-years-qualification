import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    inputText,
    isVisible,
    checkTextContains
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the confirmation page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);

        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");
    });

    test("Checks the content is on the page", async ({ page }) => {
        await checkText(page, ".govuk-panel__title", "Message sent");
        await checkText(page, "#main-content > div > div > p", "Your message was successfully sent to the Check an early years qualification team.");
        await checkText(page, "#help-confirmation-body-heading", "What happens next");

        await checkText(page, "#help-confirmation-body > p:nth-child(1)", "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer. We may need to contact you for more information before we can respond.");
        await checkText(page, "#feedback-header", "Give feedback");

        await checkText(page, "#feedback-component > p", "Your feedback matters and will help us improve the service.");
        await checkText(page, "#return-button", "Return to the homepage");
    });
});