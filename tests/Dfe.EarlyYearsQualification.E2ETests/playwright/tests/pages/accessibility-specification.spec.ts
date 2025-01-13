import {test} from '@playwright/test';
import {startJourney, checkText} from '../shared/processLogic';

test.describe('A spec that tests the accessibility statement page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the heading and content are present", async ({page}) => {
        await page.goto("/accessibility-statement");
        await checkText(page, "#accessibility-statement-heading", "Test Accessibility Statement Heading");
        await checkText(page, "#accessibility-statement-body", "Test Accessibility Statement Body");
    });
});