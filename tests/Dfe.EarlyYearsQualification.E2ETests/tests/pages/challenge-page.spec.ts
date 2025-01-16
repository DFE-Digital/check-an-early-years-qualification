import {expect, test} from '@playwright/test';
import {startJourney, checkText} from '../shared/playwrightWrapper';

test.describe('A spec that tests the challenge page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await page.goto("/challenge");
    });

    test("should show the missing password error when the user doesn't enter a password", async ({page}) => {
        await expect(page.locator("#error-banner")).toHaveCount(0);
        await expect(page.locator("#error-message")).toHaveCount(0);

        await page.locator("#question-submit").click();

        await expect(page.locator("#error-banner")).toHaveCount(1);
        await checkText(page, '#error-banner-link', "Test Missing Password Text");
        await checkText(page, '#error-message', "Test Missing Password Text");
    });

    test("should show the incorrect password error when the user enters an incorrect password", async ({page}) => {
        await expect(page.locator("#error-banner")).toHaveCount(0);
        await expect(page.locator("#error-message")).toHaveCount(0);

        await page.locator("#PasswordValue").fill("Some incorrect password");
        await page.locator("#question-submit").click();

        await expect(page.locator("#error-banner")).toHaveCount(1);
        await checkText(page, '#error-banner-link', "Test Incorrect Password Text");
        await checkText(page, '#error-message', "Test Incorrect Password Text");
    });

    test("clicking the show password button changes the password input to text, clicking it again turns it back", async ({page}) => {
        await page.locator("#PasswordValue").fill("password");
        await expect(page.locator("#PasswordValue")).toHaveAttribute('type', 'password');
        await page.locator("#togglePassword").click();
        await expect(page.locator("#PasswordValue")).toHaveAttribute('type', 'text');
        await page.locator("#togglePassword").click();
        await expect(page.locator("#PasswordValue")).toHaveAttribute('type', 'password');
    });
});