import {test, expect} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName} from '../shared/playwrightWrapper';


test.describe('A spec that tests the confirm qualification page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
    });

    test("Checks the static page content is on the page", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-240");

        await checkText(page, "#heading", "Test heading");
        await checkText(page, "#post-heading", "The post heading content");
        await checkText(page, "#qualification-name-row dt", "Test qualification label");
        await checkText(page, "#qualification-level-row dt", "Test level label");
        await checkText(page, "#qualification-org-row dt", "Test awarding organisation label");
        await expect(page.locator("#various-ao-content")).toHaveCount(0);
        await checkText(page, "#radio-heading", "Test radio heading");
        await expect(page.locator('input[value="yes"]')).toHaveCount(1);
        await expect(page.locator('input[value="no"]')).toHaveCount(1);
        await checkText(page, 'label[for="yes"]', "yes");
        await checkText(page, 'label[for="no"]', "no");
        await expect(page.locator('#warning-text-container')).toHaveCount(0);
        await checkText(page, "#confirm-qualification-button", "Test button text");
        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#confirm-qualification-choice-error")).toHaveCount(0);
    });

    test("Checks the various content is on the page", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-250");

        await expect(page.locator('#various-ao-content')).toHaveCount(1);
        await checkText(page, '#various-ao-content', "Various awarding organisation explanation text");
    });

    test("Checks the warning content is on the page when the qualification has no additional requirement questions", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-115");

        await expect(page.locator('#warning-text-container')).toHaveCount(1);
        await checkText(page, '#warning-text-container', "Answer disclaimer text");
        await checkText(page, "#confirm-qualification-button", "Get result");
    });

    test("Shows errors if user does not select an option", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-240");

        await page.click("#confirm-qualification-button");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "Test error banner heading");
        await checkText(page, "#error-banner-link", "Test error banner link");
        await checkText(page, "#confirm-qualification-choice-error", "Test error text");
        await expect(page.locator(".govuk-form-group--error")).toBeVisible();
    });
});