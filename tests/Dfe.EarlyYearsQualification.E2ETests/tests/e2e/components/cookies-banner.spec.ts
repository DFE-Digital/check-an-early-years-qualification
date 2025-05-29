import {test} from '@playwright/test';
import {pages} from "../../shared/urls-to-check";
import {authorise, checkText, checkCookieValue, isVisible, doesNotExist, setCookie, journeyCookieName} from '../../shared/playwrightWrapper';

test.describe('A spec that tests that the cookies banner shows on all pages', {tag: "@e2e"}, () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });

    pages.forEach((url) => {
        test(`Checks that the cookies banner is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);

            await isVisible(page, "#choose-cookies-preference");
            await doesNotExist(page, "#cookies-preference-chosen");
            await checkText(page, ".govuk-cookie-banner__heading", "Test Cookies Banner Title");
            await checkText(page, ".govuk-cookie-banner__content", "This is the cookies banner content");
        });

        test(`Accepting the cookies shows the accept message at the URL: ${url}, then clicking to hide the banner should hide the banner`, async ({
                                                                                                                                                      page,
                                                                                                                                                      context
                                                                                                                                                  }) => {
            await page.goto(url);
            await page.locator("#accept-cookies-button").click();
            await doesNotExist(page, "#choose-cookies-preference");
            await isVisible(page, "#cookies-preference-chosen");
            await checkCookieValue(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Afalse%7D");
            await checkText(page, "#cookies-banner-pref-chosen-content", "This is the accepted cookie content");
            await page.locator("#hide-cookie-banner-button").click();
            await doesNotExist(page, "#choose-cookies-preference");
            await doesNotExist(page, "#cookies-preference-chosen");
            await checkCookieValue(context, "%7B%22HasApproved%22%3Atrue%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Afalse%7D");
        });

        test(`Rejecting the cookies shows the reject message at the URL: ${url}, then clicking to hide the banner should hide the banner`, async ({
                                                                                                                                                      page,
                                                                                                                                                      context
                                                                                                                                                  }) => {
            await page.goto(url);
            await page.locator("#reject-cookies-button").click();
            await doesNotExist(page, "#choose-cookies-preference");
            await isVisible(page, "#cookies-preference-chosen");
            await checkCookieValue(context, "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Atrue%2C%22IsRejected%22%3Atrue%7D");
            await checkText(page, "#cookies-banner-pref-chosen-content", "This is the rejected cookie content");
            await page.locator("#hide-cookie-banner-button").click();
            await doesNotExist(page, "#choose-cookies-preference");
            await doesNotExist(page, "#cookies-preference-chosen");
            await checkCookieValue(context, "%7B%22HasApproved%22%3Afalse%2C%22IsVisible%22%3Afalse%2C%22IsRejected%22%3Atrue%7D");
        });
    });
});