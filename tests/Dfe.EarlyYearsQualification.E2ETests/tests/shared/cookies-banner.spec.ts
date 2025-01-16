import {test, expect} from '@playwright/test';
import {pages} from "../shared/urls-to-check";
import {authorise, checkText, checkCookieValue} from './processLogic';

test.describe('A spec that tests that the cookies banner shows on all pages', () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
    });

    pages.forEach((url) => {
        test(`Checks that the cookies banner is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);

            await expect(page.locator("#choose-cookies-preference")).toBeVisible();
            await expect(page.locator("#cookies-preference-chosen")).toHaveCount(0);
            await checkText(page, ".govuk-cookie-banner__heading", "Test Cookies Banner Title");
            await checkText(page, ".govuk-cookie-banner__content", "This is the cookies banner content");
        });

        test(`Accepting the cookies shows the accept message at the URL: ${url}, then clicking to hide the banner should hide the banner`, async ({
                                                                                                                                                      page,
                                                                                                                                                      context
                                                                                                                                                  }) => {
            await page.goto(url);
            await page.locator("#accept-cookies-button").click();
            await expect(page.locator("#choose-cookies-preference")).toHaveCount(0);
            await expect(page.locator("#cookies-preference-chosen")).toBeVisible();
            await checkCookieValue(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Afalse%7D");
            await checkText(page, "#cookies-banner-pref-chosen-content", "This is the accepted cookie content");
            await page.locator("#hide-cookie-banner-button").click();
            await expect(page.locator("#choose-cookies-preference")).toHaveCount(0);
            await expect(page.locator("#cookies-preference-chosen")).toHaveCount(0);
            await checkCookieValue(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D");
        });

        test(`Rejecting the cookies shows the reject message at the URL: ${url}, then clicking to hide the banner should hide the banner`, async ({
                                                                                                                                                      page,
                                                                                                                                                      context
                                                                                                                                                  }) => {
            await page.goto(url);
            await page.locator("#reject-cookies-button").click();
            await expect(page.locator("#choose-cookies-preference")).toHaveCount(0);
            await expect(page.locator("#cookies-preference-chosen")).toBeVisible();
            await checkCookieValue(context, "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Atrue%7D");
            await checkText(page, "#cookies-banner-pref-chosen-content", "This is the rejected cookie content");
            await page.locator("#hide-cookie-banner-button").click();
            await expect(page.locator("#choose-cookies-preference")).toHaveCount(0);
            await expect(page.locator("#cookies-preference-chosen")).toHaveCount(0);
            await checkCookieValue(context, "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Atrue%7D");
        });
    });
});