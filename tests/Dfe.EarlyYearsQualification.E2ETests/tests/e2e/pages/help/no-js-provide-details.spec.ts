import { test } from '@playwright/test';
import {
    startJourney,
    checkUrl,
    inputText,
    isVisible,
    isNotVisible,
    checkTextContains} from '../../../_shared/playwrightWrapper';

// Disables JavaScript for all tests inside this spec
test.use({ javaScriptEnabled: false });

test.describe("A spec that tests the provide details page when JS is disabled", { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });
    
    test("Check the character limit hint text is correct when JS is disabled", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await isVisible(page, "#no-js-character-limit-hint");
        await isNotVisible(page, "#character-limit-hint");
        await checkTextContains(page, "#no-js-character-limit-hint", "You can enter up to 1000 characters");

        // 5 characters
        let message = "12345";
        // pressSequentially triggers the key-up event that is used by JQuery to change the hint text
        await page.locator("#ProvideAdditionalInformation").pressSequentially(message);

        await checkTextContains(page, "#no-js-character-limit-hint", "You can enter up to 1000 characters");
    });
});