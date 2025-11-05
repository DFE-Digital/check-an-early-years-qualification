import {test} from '@playwright/test';
import {pagesWithUpDownFeedback} from "../../_shared/urls-to-check";

import {
    authorise,
    checkText,
    checkTextContains,
    isVisible,
    isNotVisible,
    setCookie,
    journeyCookieName,
    checkUrl
} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests that the updown-feedback shows on all pages. Cookies enabled | Javascript enabled', {tag: "@e2e"}, () => {
    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });
    pagesWithUpDownFeedback.forEach((url) => {
        test(`Checks that the updown-feedback is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await checkUrl(page, url);
            await page.locator("#accept-cookies-button").click();
            await isVisible(page, "#ud-feedback");
            await isVisible(page, "#ud-prompt");
            await isVisible(page, "#yes-button");
            await isVisible(page, "#no-button");
            await isVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isNotVisible(page, "#ud-improve-service");
            await isNotVisible(page, "#ud-cancel");
            await checkText(page, ".ud-feedback__prompt-question", "Did you get everything you needed today?");
            await checkText(page, "#yes-button", "Yes this service is useful");
            await checkText(page, "#no-button", "No this service is not useful");
            await checkText(page, "#ud-get-help", "Get help with this page");
        });

        test(`Checks that the is-useful displays useful response at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await page.locator("#accept-cookies-button").click();
            await page.locator("#yes-button").click();
            await isVisible(page, "#ud-feedback");
            await isNotVisible(page, "#ud-prompt");
            await isNotVisible(page, "#yes-button");
            await isNotVisible(page, "#no-button");
            await isNotVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isVisible(page, "#ud-improve-service");
            await isVisible(page, "#feedback-component");
            await isVisible(page, "#ud-cancel");

            await checkTextContains(page, "#feedback-component", "Your feedback matters and will help us improve the service.");
            await checkText(page, "#ud-cancel", "Cancel");
        });

        test(`Checks that the is-not-useful displays improve-service and goes back correctly at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await checkUrl(page, url);
            await page.locator("#accept-cookies-button").click();
            await page.locator("#no-button").click();
            await isVisible(page, "#ud-feedback");
            await isNotVisible(page, "#ud-prompt");
            await isNotVisible(page, "#yes-button");
            await isNotVisible(page, "#no-button");
            await isNotVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isVisible(page, "#ud-improve-service");
            await isVisible(page, "#feedback-component");
            await isVisible(page, "#ud-cancel");

            await checkTextContains(page, "#feedback-component", "Your feedback matters and will help us improve the service.");
            await checkText(page, "#ud-cancel", "Cancel");

            await page.locator("#ud-cancel").click();

            await isVisible(page, "#ud-feedback");
            await isVisible(page, "#ud-prompt");
            await isVisible(page, "#yes-button");
            await isVisible(page, "#no-button");
            await isVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isNotVisible(page, "#ud-improve-service");
            await isNotVisible(page, "#ud-cancel");
        });

        test(`Checks that the get-help button redirects to help at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await page.locator("#accept-cookies-button").click();
            await page.locator("#ud-get-help").click();


            await checkUrl(page, "/help/get-help");
        });
    });
});
test.describe('A spec that tests that the updown-feedback shows on all pages. Cookies disabled | Javascript enabled', {tag: "@e2e"}, () => {
    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });
    
    pagesWithUpDownFeedback.forEach((url) => {
        test(`Checks that the updown-feedback is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await page.locator("#reject-cookies-button").click();
            await isVisible(page, "#ud-feedback");
            await isNotVisible(page, "#ud-prompt");
            await isNotVisible(page, "#yes-button");
            await isNotVisible(page, "#no-button");
            await isNotVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isVisible(page, "#ud-improve-service");
            await isVisible(page, "#feedback-component");
            await isNotVisible(page, "#ud-cancel");

            await checkTextContains(page, "#feedback-component", "Your feedback matters and will help us improve the service.");
        });
    });
});
test.describe('A spec that tests that the updown-feedback shows on all pages. Cookies enabled | Javascript disabled', {tag: "@e2e"}, () => {
    test.use({javaScriptEnabled: false});
    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });
    
    pagesWithUpDownFeedback.forEach((url) => {
        test(`Checks that the updown-feedback is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await page.locator("#accept-cookies-button").click();
            await page.goto(url);
            await isVisible(page, "#ud-feedback");
            await isNotVisible(page, "#ud-prompt");
            await isNotVisible(page, "#yes-button");
            await isNotVisible(page, "#no-button");
            await isNotVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isVisible(page, "#ud-improve-service");
            await isVisible(page, "#feedback-component");
            await isNotVisible(page, "#ud-cancel");

            await checkTextContains(page, "#feedback-component", "Your feedback matters and will help us improve the service.");
        });
    });
});

test.describe('A spec that tests that the updown-feedback shows on all pages. Cookies disabled | Javascript disabled', {tag: "@e2e"}, () => {
    test.use({javaScriptEnabled: false});
    test.beforeEach(async ({context}) => {
        await authorise(context);
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%2212%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%22Test%20question%22%3A%22yes%22%2C%22Test%20question%202%22%3A%22yes%22%7D%2C%22QualificationWasSelectedFromList%22%3A1%7D', journeyCookieName);
    });
   
    pagesWithUpDownFeedback.forEach((url) => {
        test(`Checks that the updown-feedback is present at the URL: ${url}`, async ({page}) => {
            await page.goto(url);
            await page.locator("#reject-cookies-button").click();
            await page.goto(url);
            await isVisible(page, "#ud-feedback");
            await isNotVisible(page, "#ud-prompt");
            await isNotVisible(page, "#yes-button");
            await isNotVisible(page, "#no-button");
            await isNotVisible(page, "#ud-get-help");
            await isNotVisible(page, "#ud-prompt-success");
            await isVisible(page, "#ud-improve-service");
            await isVisible(page, "#feedback-component");
            await isNotVisible(page, "#ud-cancel");

            await checkTextContains(page, "#feedback-component", "Your feedback matters and will help us improve the service.");
        });
    });
});