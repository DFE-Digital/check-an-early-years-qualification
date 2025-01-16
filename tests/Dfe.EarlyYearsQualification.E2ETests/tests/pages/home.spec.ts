import {test, expect} from '@playwright/test';
import {startJourney, checkText, checkUrl} from '../shared/processLogic';


test.describe('A spec used to test the home page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the page contains the relevant components", async ({page}) => {
        await page.goto("/");

        await checkText(page, ".govuk-heading-xl", "Test Header");
        await checkText(page, "#pre-cta-content p", "This is the pre cta content");
        await checkText(page, ".govuk-button--start", "Start Button Text");
        await checkText(page, "#post-cta-content p", "This is the post cta content");
        await checkText(page, "#right-hand-content-header", "Related content");
        await checkText(page, "#right-hand-content p", "This is the right hand content");
    });

    test("Checks Crown copyright link text", async ({page}) => {
        await page.goto("/");

        await checkText(page, ".govuk-footer__copyright-logo", "Crown copyright", 0);
    });
});