import {test} from '@playwright/test';
import {
    startJourney,
    setCookie,
    journeyCookieName,
    checkText,
    exists,
    hasAttribute,
    doesNotExist
} from '../../_shared/playwrightWrapper';

const selector = "#main-content ~ .back-to-top #back-to-top-link";

const journeyCookie = (level: number) =>
    '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22LevelOfQualification%22%3A%22LEVEL%22%7D'.replace('LEVEL', level.toString());

async function checkBackToTopButton(page: any) {
    await exists(page, selector);
    await checkText(page, selector, "Back to top");
    await hasAttribute(page, selector, "href", "#top");
    await exists(page, `${selector} svg`);
}

test.describe('A spec that tests the back to top button', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Shows the button on the qualification not on the list default page", async ({page, context}) => {
        await setCookie(context, journeyCookie(5), journeyCookieName);
        await page.goto("/advice/qualification-not-on-the-list");
        await checkBackToTopButton(page);
    });

    test("Shows the button on the qualification not on the list specific page", async ({page, context}) => {
        await setCookie(context, journeyCookie(3), journeyCookieName);
        await page.goto("/advice/qualification-not-on-the-list");
        await checkBackToTopButton(page);
    });

    test("Shows the button on the early years qualification levels page", async ({page, context}) => {
        await setCookie(context, journeyCookie(3), journeyCookieName);
        await page.goto("/advice/early-years-qualification-levels");
        await checkBackToTopButton(page);
    });

    test("Shows the button on the qualification list page", async ({page}) => {
        await page.goto("/early-years-qualification-list");
        await checkBackToTopButton(page);
    });

    test("Does not show the button on a page without a linked entry", async ({page}) => {
        await page.goto("/advice/qualification-outside-the-united-kingdom");
        await doesNotExist(page, selector);
    });
});
