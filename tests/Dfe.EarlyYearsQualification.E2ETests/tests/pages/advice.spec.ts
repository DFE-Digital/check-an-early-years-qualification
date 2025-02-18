import {Page, test} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName} from '../shared/playwrightWrapper';

async function checkFeedbackBanners(page: Page) {
    await checkText(page, ".govuk-notification-banner__title", "Test banner title", 0);
    await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 0);
    await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 0);

    await checkText(page, ".govuk-notification-banner__title", "Test banner title", 1);
    await checkText(page, ".govuk-notification-banner__heading", "Feedback heading", 1);
    await checkText(page, ".govuk-notification-banner__content > .govuk-body", "This is the body text", 1);
}

test.describe('A spec that tests advice pages', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the Qualifications achieved outside the United Kingdom details are on the page", async ({page}) => {
        await page.goto("/advice/qualification-outside-the-united-kingdom");
        await checkText(page, "#advice-page-heading", "Qualifications achieved outside the United Kingdom");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Scotland details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-scotland");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Scotland");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Wales details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-wales");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Wales");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Qualifications achieved in Northern Ireland details are on the page", async ({page}) => {

        await page.goto("advice/qualifications-achieved-in-northern-ireland");
        await checkText(page, "#advice-page-heading", "Qualifications achieved in Northern Ireland");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the level 7 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 7 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });

    test("Checks the Level 7 qualification after aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2020%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualification-after-aug-2019");
        await checkText(page, "#advice-page-heading", "Level 7 qualification after aug 2019");
        await checkText(page, "#advice-page-body", "Test Advice Page Body");

        await checkFeedbackBanners(page);
    });
});