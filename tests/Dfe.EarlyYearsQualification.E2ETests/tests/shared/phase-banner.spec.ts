﻿import {test, expect} from '@playwright/test';
import {pages} from "../shared/urls-to-check";
import {authorise, checkText} from './playwrightWrapper';

test.describe('A spec that tests the phase banner is showing on all pages', () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
    });

    pages.forEach((url) => {
        test(`Checks that the phase banner is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);

            await expect(page.locator(".govuk-phase-banner")).toBeVisible();
            await checkText(page, ".govuk-phase-banner__content__tag", "Test phase banner name");
            await checkText(page, ".govuk-phase-banner__text", "Some TextLink Text");
        })
    });
});