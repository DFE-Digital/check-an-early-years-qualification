import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    checkError,
    checkDisclaimer,
    setCookie,
    journeyCookieName,
    doesNotExist,
    exists,
    isVisible
} from '../shared/playwrightWrapper';

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
        await doesNotExist(page, "#various-ao-content");
        await checkText(page, "#radio-heading", "Test radio heading");
        await exists(page, 'input[value="yes"]');
        await exists(page, 'input[value="no"]');
        await checkText(page, 'label[for="yes"]', "yes");
        await checkText(page, 'label[for="no"]', "no");
        await doesNotExist(page, '#warning-text-container');
        await checkText(page, "#confirm-qualification-button", "Test button text");
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#confirm-qualification-choice-error");
    });

    test("Checks the various content is on the page", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-250");

        await exists(page, '#various-ao-content');
        await checkText(page, '#various-ao-content', "Various awarding organisation explanation text");
    });

    test("Checks the warning content is on the page when the qualification has no additional requirement questions", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-115");

        await exists(page, '#warning-text-container');
        await checkDisclaimer(page, "Answer disclaimer text");
        await checkText(page, "#confirm-qualification-button", "Get result");
    });

    test("Shows errors if user does not select an option", async ({page}) => {
        await page.goto("/confirm-qualification/eyq-240");

        await page.click("#confirm-qualification-button");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "Test error banner heading");
        await checkText(page, "#error-banner-link", "Test error banner link");
        await checkError(page, "#confirm-qualification-choice-error", "Test error text");
        await isVisible(page, ".govuk-form-group--error");
    });
});