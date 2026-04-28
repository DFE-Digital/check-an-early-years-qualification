import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    exists,
    isVisible,
    checkTextContains
} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the give feedback pages', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the feedback form details are on the page", async ({page, context}) => {
        await page.goto("/give-feedback");

        await checkText(page, "#feedback-page-heading", "Give feedback");
        await checkText(page, "#post-heading-content", "This is the post heading content");
        await checkText(page, "[id='0_question']", "Overall, how satisfied are you with this service?");
        await exists(page, "[id='0_VerySatisfied']");
        await exists(page, "[id='0_Satisfied']");
        await exists(page, "[id='0_Neutral']");
        await exists(page, "[id='0_Dissatisfied']");
        await exists(page, "[id='0_VeryDissatisfied']");
        await checkText(page, "[id='1_question']", "How confident are you with the information you received from the service?");
        await exists(page, "[id='1_VeryConfident']");
        await exists(page, "[id='1_Confident']");
        await exists(page, "[id='1_Neutral']");
        await exists(page, "[id='1_SlightlyConfident']");
        await exists(page, "[id='1_NotAtAllConfident']");
        await checkText(page, "[id='2_question']", "Share any feedback about your experience, including suggestions for how we could improve the service");
        await checkText(page, "#textarea_2_hint", "Do not include personal information, for example the name of the qualification holder");
        await checkText(page, "#feedback-form-submit", "Submit feedback")
    });

    test("Press submit on feedback form without entering details progresses to confirmation page", async ({page, context}) => {
        await page.goto("/give-feedback");
        
        await page.locator("#feedback-form-submit").click();
        await isVisible(page, ".govuk-panel__title");
        await isVisible(page, "#main-confirmation-body");
        await isVisible(page, "#return-button");
        await checkText(page, ".govuk-panel__title", "Your feedback has been successfully submitted");
        await checkTextContains(page, "#main-confirmation-body", "Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        await checkText(page, "#return-button", "Home");
    });

    test("Checks the details are on the feedback confirmation page when an email was supplied", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22%22%2C%22WhenWasQualificationStarted%22%3A%22%22%2C%22WhenWasQualificationAwarded%22%3A%22%22%2C%22LevelOfQualification%22%3A%22%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A0%2C%22HasSubmittedEmailAddressInFeedbackFormQuestion%22%3Atrue%7D', journeyCookieName);
        await page.goto("/give-feedback/confirmation");

        await isVisible(page, ".govuk-panel__title");
        await isVisible(page, "#main-confirmation-body");
        await isVisible(page, "#optional-body-heading");
        await isVisible(page, "#optional-confirmation-body");
        await isVisible(page, "#return-button");
        await checkText(page, ".govuk-panel__title" ,"Your feedback has been successfully submitted");
        await checkTextContains(page, "#main-confirmation-body" ,"Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        await checkTextContains(page, "#optional-body-heading" ,"What happens next");
        await checkTextContains(page, "#optional-confirmation-body" ,"As you agreed to be contacted about future research, someone from our research team may contact you by email.");
        await checkText(page, "#return-button", "Home");
    });
});