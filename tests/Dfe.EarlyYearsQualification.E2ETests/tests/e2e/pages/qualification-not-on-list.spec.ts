import {test} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the qualification not on list page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks content renders for the level 3 specific qualification not on the list page", async ({
                                                                                                          page,
                                                                                                          context
                                                                                                      }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%7D', journeyCookieName);
        await page.goto("/advice/qualification-not-on-the-list");

        await checkText(page, "#advice-page-heading", "This is the level 3 page");
        await checkText(page, "#advice-page-body", "This is the body text");

        await checkText(page, ".govuk-notification-banner__title", "Banner title", 0);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback banner heading", 0);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Banner body text", 0);

        await checkText(page, ".govuk-notification-banner__title", "Banner title", 1);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback banner heading", 1);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Banner body text", 1);
    });

    test("Checks content renders for the level 4 specific qualification not on the list page", async ({
                                                                                                          page,
                                                                                                          context
                                                                                                      }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%224%22%7D', journeyCookieName);
        await page.goto("/advice/qualification-not-on-the-list");

        await checkText(page, "#advice-page-heading", "This is the level 4 page");
        await checkText(page, "#advice-page-body", "This is the body text");

        await checkText(page, ".govuk-notification-banner__title", "Banner title", 0);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback banner heading", 0);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Banner body text", 0);

        await checkText(page, ".govuk-notification-banner__title", "Banner title", 1);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback banner heading", 1);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "Banner body text", 1);
    });

    test("Checks default content renders when no specific qualification not on the list page exists", async ({
                                                                                                                 page,
                                                                                                                 context
                                                                                                             }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%225%22%7D', journeyCookieName);
        await page.goto("/advice/qualification-not-on-the-list");

        await checkText(page, "#advice-page-heading", "Qualification not on the list");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkText(page, ".govuk-notification-banner__title", "Test banner title", 0);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 0);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 0);

        await checkText(page, ".govuk-notification-banner__title", "Test banner title", 1);
        await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 1);
        await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 1);
    });
});