import {test} from '@playwright/test';
import {pagesThatRedirectIfDateMissing} from "../../_shared/urls-to-check";
import {startJourney, checkUrl} from '../../_shared/playwrightWrapper';

test.describe('A spec used to check that if the user skips entering the date of the qual, then they are redirected back to the date selection page', {tag: "@e2e"}, () => {

    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    pagesThatRedirectIfDateMissing.forEach((url) => {
        test(`navigating to ${url} should redirect the user to the date selection page`, async ({page}) => {
            await page.goto(url);
            await checkUrl(page, "/questions/are-you-checking-your-own-qualification");
        })
    });
});