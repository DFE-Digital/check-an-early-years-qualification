import {test} from '@playwright/test';
import {startJourney, checkUrl} from '../../_shared/playwrightWrapper';

test.describe('A spec used to check that bookmarked legacy URLs are redirected to their new address', {tag: "@e2e"}, () => {

    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("navigating to the old are-you-checking-your-own-qualification URL redirects to what-qualification-are-you-checking", async ({page}) => {
        await page.goto("/questions/are-you-checking-your-own-qualification");
        await checkUrl(page, "/questions/what-qualification-are-you-checking");
    });
});
