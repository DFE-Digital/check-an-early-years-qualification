import {expect, test} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName, checkUrl, clickBackButton} from '../shared/playwrightWrapper';

test.describe("A spec that tests the cookies page", () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await page.goto("/cookies");
    });

    test("Checks the content is present", async ({page}) => {

        await expect(page.locator('#cookies-set-banner')).toHaveCount(0);
        await checkText(page, "#cookies-heading", "Test Cookies Heading");
        await checkText(page, "#cookies-body", "Test Cookies Page Body");
        await checkText(page, "#cookies-form-heading", "Test Form Heading");
        await expect(page.locator("#test-option-value-1")).toHaveCount(1);
        await expect(page.locator("#test-option-value-2")).toHaveCount(1);
        await checkText(page, "label[for='test-option-value-1']", "Test Option Label 1");
        await checkText(page, "label[for='test-option-value-2']", "Test Option Label 2");
        await expect(page.locator("#cookies-choice-error")).not.toBeVisible();
        await checkText(page, "#cookies-choice-error", "Test Error Text");
        await checkText(page, 'button[id="cookies-button"]', "Test Cookies Button");
    });

    test.describe("Check the functionality of the page", () => {
        test("Checks that the radio button validation is working", async ({page}) => {

            await page.click('#cookies-button');
            await expect(page.locator("#cookies-set-banner")).toHaveCount(0);
            await expect(page.locator("#cookies-choice-error")).toBeVisible();
        });

        ["test-option-value-1", "test-option-value-2"].forEach((option) => {
            test(`Checks that selecting ${option} reveals success banner`, async ({page}) => {

                await page.click(`#${option}`);
                await page.click('#cookies-button');
                await expect(page.locator("#cookies-set-banner")).toBeVisible();
                await checkText(page, "#cookies-set-banner-heading", "Test Success Banner Heading");
                await checkText(page, "#cookies-set-banner-content", "Test Success Banner Content");
                await expect(page.locator("#cookies-choice-error")).not.toBeVisible();
            });
        });
    });
});