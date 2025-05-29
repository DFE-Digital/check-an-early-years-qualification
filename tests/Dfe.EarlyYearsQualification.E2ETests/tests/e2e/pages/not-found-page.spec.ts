import { test } from '@playwright/test';
import { startJourney, checkText, checkUrl, attributeContains } from '../../shared/playwrightWrapper';

test.describe('A spec used to test the not found page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the page contains the relevant components", async ({ page }) => {
        await page.goto("/error/404");

        await checkText(page, "#page-not-found-heading", "Page not found");
        await checkText(page, "#page-not-found-statement-body", "If you typed out the web address, check it is correct. If you pasted the web address, check you copied the entire address. If the web address is correct, or you selected a link or button, contact the check an early years qualification team by emailing techsupport.EARLY-YEARS-QUALS@education.gov.uk to report a fault with the service.");

        await attributeContains(page, "#page-not-found-link", "href", "mailto:techsupport.EARLY-YEARS-QUALS@education.gov.uk");
    });

    test("Check that visiting a URL that doesn't exist shows this page without altering the URL", async ({ page }) => {
        await page.goto("/does-not-exist");

        await checkUrl(page, "/does-not-exist");
        await checkText(page, "#page-not-found-heading", "Page not found");
    });
});