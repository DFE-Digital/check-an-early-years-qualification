import {test} from '@playwright/test';
import {startJourney, checkText, checkError, doesNotExist, exists, hasAttribute} from '../shared/playwrightWrapper';

test.describe('A spec that tests the challenge page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await page.goto("/challenge");
    });

    test("should show the missing password error when the user doesn't enter a password", async ({page}) => {
        await doesNotExist(page, "#error-banner");
        await doesNotExist(page, "#error-message");

        await page.locator("#question-submit").click();

        await exists(page, "#error-banner");
        await checkText(page, '#error-banner-link', "Test Missing Password Text");
        await checkError(page, '#error-message', "Test Missing Password Text");
    });

    test("should show the incorrect password error when the user enters an incorrect password", async ({page}) => {
        await doesNotExist(page, "#error-banner");
        await doesNotExist(page, "#error-message");

        await page.locator("#PasswordValue").fill("Some incorrect password");
        await page.locator("#question-submit").click();

        await exists(page, "#error-banner");
        await checkText(page, '#error-banner-link', "Test Incorrect Password Text");
        await checkError(page, '#error-message', "Test Incorrect Password Text");
    });

    test("clicking the show password button changes the password input to text, clicking it again turns it back", async ({page}) => {
        await page.locator("#PasswordValue").fill("password");
        await hasAttribute(page, "#PasswordValue", 'type', 'password');
        await page.locator("#togglePassword").click();
        await hasAttribute(page, "#PasswordValue", 'type', 'text');
        await page.locator("#togglePassword").click();
        await hasAttribute(page, "#PasswordValue", 'type', 'password');
    });
});