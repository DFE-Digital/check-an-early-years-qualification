import {test} from '@playwright/test';
import { startJourney, checkText, clickBackButton, checkUrl, doesNotExist } from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the accessibility statement page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the heading and content are present", async ({page}) => {
        await page.goto("/accessibility-statement");
        await checkText(page, "#accessibility-statement-heading", "Test Accessibility Statement Heading");
        await checkText(page, "#accessibility-statement-body", "Test Accessibility Statement Body");
        await checkText(page, "a[href='/accessibility-statement']", "Accessibility statement");
        await doesNotExist(page, "a[href='/early-years-qualification-list/accessibility-statement']");
    });

    test("Checks the heading and content are present within the EYQL accessibility statement", async ({ page }) => {
        await page.goto("/early-years-qualification-list/accessibility-statement");
        await checkText(page, "#accessibility-statement-heading", "Test EYQL Accessibility Statement Heading");
        await checkText(page, "#accessibility-statement-body", "Test EYQL Accessibility Statement Body");
        await checkText(page, "a[href='/early-years-qualification-list/accessibility-statement']", "Accessibility statement");
        await doesNotExist(page, "a[href='/accessibility-statement']");
        await clickBackButton(page);
        await checkUrl(page, "/early-years-qualification-list");
    });
});