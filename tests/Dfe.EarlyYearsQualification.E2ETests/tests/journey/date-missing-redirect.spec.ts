import {test} from '@playwright/test';
import {pagesThatRedirectIfDateMissing} from "../shared/urls-to-check";
import {startJourney, checkUrl} from '../shared/playwrightWrapper';

test.describe('A spec used to check that if the user skips entering the date of the qual, then they are redirected back to the date selection page', () => {

    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    pagesThatRedirectIfDateMissing.forEach((url) => {
        test(`navigating to ${url} should redirect the user to the date selection page`, async ({page}) => {
            await page.goto(url);
            await checkUrl(page, "/questions/when-was-the-qualification-started");
        })
    });
});