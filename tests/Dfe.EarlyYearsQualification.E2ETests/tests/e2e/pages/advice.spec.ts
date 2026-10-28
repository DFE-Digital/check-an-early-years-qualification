import {test, expect} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    doesNotExist,
    hasAttribute
} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests advice pages', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the Qualifications achieved outside the United Kingdom details are on the page", async ({page}) => {
        await page.goto("/advice/qualification-outside-the-united-kingdom");
        await checkText(page, "#static-page-heading", "Qualifications achieved outside the United Kingdom");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the level 2 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#static-page-heading", "Level 2 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the Qualifications achieved in Scotland details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-scotland");
        await checkText(page, "#static-page-heading", "Qualifications achieved in Scotland");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the Qualifications achieved in Wales details are on the page", async ({page}) => {

        await page.goto("/advice/qualifications-achieved-in-wales");
        await checkText(page, "#static-page-heading", "Qualifications achieved in Wales");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the Qualifications achieved in Northern Ireland details are on the page", async ({page}) => {

        await page.goto("advice/qualifications-achieved-in-northern-ireland");
        await checkText(page, "#static-page-heading", "Qualifications achieved in Northern Ireland");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the level 7 between 1 Sept 2014 and 31 Aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await checkText(page, "#static-page-heading", "Level 7 qualifications started between 1 September 2014 and 31 August 2019");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Checks the Level 7 qualification after aug 2019 details are on the page", async ({page, context}) => {

        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2020%22%7D', journeyCookieName);
        await page.goto("/advice/level-7-qualification-after-aug-2019");
        await checkText(page, "#static-page-heading", "Level 7 qualification after aug 2019");
        await checkText(page, "#static-page-body", "Test Static Page Body");
    });

    test("Find an approved early years course page is indexable and uses its OpenGraph description", async ({page}) => {
        await page.goto("/advice/find-an-approved-early-years-course");
        await doesNotExist(page, "meta[name='robots']");
        await expect(page).toHaveTitle("Dynamic Page Heading - Check an Early Years qualification");
        await hasAttribute(page, "meta[property='og:title']", "content", "Find an approved early years course OG Title");
        await hasAttribute(page, "meta[name='description']", "content", "Find an approved early years course OG Description");
    });

    test("Other advice pages are not indexable", async ({page}) => {
        await page.goto("/advice/qualifications-achieved-in-scotland");
        await hasAttribute(page, "meta[name='robots']", "content", "noindex, nofollow");
        await expect(page).toHaveTitle("Qualifications achieved in Scotland - Check an Early Years qualification");
    });
});