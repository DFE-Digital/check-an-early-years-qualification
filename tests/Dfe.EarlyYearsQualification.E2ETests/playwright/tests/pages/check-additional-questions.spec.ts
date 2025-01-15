import {expect, Page, test} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName} from '../shared/processLogic';

test.describe('A spec that tests the check additional questions page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
    });

    test("Checks the check additional questions details are on the first question page", async ({page}) => {
        await page.goto(`/qualifications/check-additional-questions/eyq-240/1`);

        await expect(page.locator("#back-button")).toHaveAttribute('href');
        expect(await page.locator("#back-button").getAttribute('href')).toContain('/qualifications');

        await checkText(page, '#question', 'Test question');
        await checkText(page, '#hint', 'This is the hint text');
        await checkText(page, ".govuk-details__summary-text", "This is the details heading");
        await checkText(page, ".govuk-details__text", "This is the details content");
        await checkText(page, "Label[for='yes']", "Yes");
        await checkText(page, "Label[for='no']", "No");
        await checkText(page, "#additional-requirement-button", "Get result");
    });


});