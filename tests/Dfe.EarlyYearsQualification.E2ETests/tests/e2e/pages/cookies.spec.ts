import {test} from '@playwright/test';
import {startJourney, checkText, checkError, doesNotExist, exists, isVisible, isNotVisible} from '../../_shared/playwrightWrapper';

test.describe("A spec that tests the cookies page", {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await page.goto("/cookies");
    });

    test("Checks the content is present", async ({page}) => {
        await doesNotExist(page, "#cookies-set-banner");
        await checkText(page, "#cookies-heading", "Test Cookies Heading");
        await checkText(page, "#cookies-body", "Test Cookies Page Body");
        await checkText(page, "#cookies-form-heading", "Test Form Heading");
        await exists(page, "#test-option-value-1");
        await exists(page, "#test-option-value-2");
        await checkText(page, "label[for='test-option-value-1']", "Test Option Label 1");
        await checkText(page, "label[for='test-option-value-2']", "Test Option Label 2");
        await isNotVisible(page, "#cookies-choice-error");
        await checkError(page, "#cookies-choice-error", "Test Error Text");
        await checkText(page, 'button[id="cookies-button"]', "Test Cookies Button");
    });

    test.describe("Check the functionality of the page", {tag: "@e2e"}, () => {
        test("Checks that the radio button validation is working", async ({page}) => {

            await page.click('#cookies-button');
            await doesNotExist(page, "#cookies-set-banner");
            await isVisible(page, "#cookies-choice-error");
        });

        ["test-option-value-1", "test-option-value-2"].forEach((option) => {
            test(`Checks that selecting ${option} reveals success banner`, async ({page}) => {

                await page.click(`#${option}`);
                await page.click('#cookies-button');
                await isVisible(page, "#cookies-set-banner");
                await checkText(page, "#cookies-set-banner-heading", "Test Success Banner Heading");
                await checkText(page, "#cookies-set-banner-content", "Test Success Banner Content");
                await isNotVisible(page, "#cookies-choice-error");
            });
        });
    });
});