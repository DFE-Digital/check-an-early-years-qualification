import {test} from '@playwright/test';
import {checkUrl, authorise} from './playwrightWrapper';

const expectedUrl = "https://vdp.security.education.gov.uk/.well-known/security.txt";

test.describe('A spec used to test the security.txt redirects', {tag: ["@e2e", "@smoke"]}, () => {
    ["/security.txt", "/.well-known/security.txt"].forEach((option) => {
        test(`Checks that going to ${option} redirects to correct location`, async ({page, context}) => {
            await authorise(context);
            await page.goto(option);
            await page.waitForURL(expectedUrl);

            await checkUrl(page, expectedUrl);
        });
    });
});