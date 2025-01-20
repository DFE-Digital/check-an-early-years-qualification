import {test} from '@playwright/test';
import {pages} from "../shared/urls-to-check";
import {authorise, checkText, isVisible} from './playwrightWrapper';

test.describe('A spec that tests the phase banner is showing on all pages', () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
    });

    pages.forEach((url) => {
        test(`Checks that the phase banner is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);

            await isVisible(page, ".govuk-phase-banner");
            await checkText(page, ".govuk-phase-banner__content__tag", "Test phase banner name");
            await checkText(page, ".govuk-phase-banner__text", "Some TextLink Text");
        })
    });
});