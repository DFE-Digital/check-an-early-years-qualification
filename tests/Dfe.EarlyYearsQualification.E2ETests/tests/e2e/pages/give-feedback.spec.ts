import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    inputText,
    setCookie,
    journeyCookieName,
    exists,
    isVisible,
    checkTextContains, isNotVisible
} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the give feedback pages', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the feedback form details are on the page", async ({page, context}) => {
        await page.goto("/give-feedback");
        await checkText(page, "#feedback-page-heading", "Give feedback");
        await checkText(page, "#post-heading-content", "This is the post heading content");
        await checkText(page, "[id='0_question']", "Did you get everything you needed today?");
        await exists(page, "[id='0_yes']");
        await exists(page, "[id='0_no']");
        await checkText(page, "[id='1_question']", "Tell us about your experience (optional)");
        await checkText(page, "#textarea_1_hint", "Do not include personal information, for example the name of the qualification holder");
        await checkText(page, "[id='2_question']", "Would you like us to contact you about future user research?");
        await exists(page, "[id='0_yes']");
        await exists(page, "[id='0_no']");
        await checkText(page, "#feedback-form-submit", "Submit feedback")
    });

    test("Press submit on feedback form without entering details shows error messages", async ({page, context}) => {
        await page.goto("/give-feedback");
        
        await isNotVisible(page, "#error-banner");
        
        await page.locator("#feedback-form-submit").click();

        await isVisible(page, "#error-banner");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-0_yes", "Select whether you got everything you needed today");
        await checkText(page, "#error-banner-link-2_yes", "Select whether you want to be contacted about future research");
        await checkTextContains(page, "[id='0_question_error-message']", "Select whether you got everything you needed today");
        await checkTextContains(page, "[id='2_question_error-message']", "Select whether you want to be contacted about future research");
    });

    test("Press submit on feedback form without entering additional information shows error message", async ({page, context}) => {
        await page.goto("/give-feedback");

        await isNotVisible(page, "#error-banner");
        await isNotVisible(page, "[id='2_additionalInfo']");

        await page.click("[id='0_yes']");
        await page.click("[id='2_yes']");
        await isVisible(page, "[id='2_additionalInfo']");
        await page.locator("#feedback-form-submit").click();
        
        await isVisible(page, "#error-banner");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-2_additionalInfo", "Enter your email address");
        await checkTextContains(page, "[id='2_question_error-message-additional-info']", "Enter your email address");
    });

    test("Press submit on feedback form when additional information contains invalid email shows format error message", async ({page, context}) => {
        await page.goto("/give-feedback");

        await isNotVisible(page, "#error-banner");
        await isNotVisible(page, "[id='2_additionalInfo']");

        await page.click("[id='0_yes']");
        await page.click("[id='2_yes']");
        await isVisible(page, "[id='2_additionalInfo']");
        await inputText(page, "[id='2_additionalInfo']", "test");
        await page.locator("#feedback-form-submit").click();

        await isVisible(page, "#error-banner");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-2_additionalInfo", "Enter an email address in the correct format, like name@example.com");
        await checkTextContains(page, "[id='2_question_error-message-additional-info']", "Enter an email address in the correct format, like name@example.com");
    });
    
    test("Checks the details are on the feedback confirmation page when no email was supplied", async ({page}) => {
        await page.goto("/give-feedback/confirmation");

        await isVisible(page, "#success-message");
        await isVisible(page, "#main-confirmation-body");
        await isVisible(page, "#return-button");

        await checkText(page, "#success-message" ,"Your feedback has been successfully submitted");
        await checkTextContains(page, "#main-confirmation-body" ,"Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        await checkText(page, "#return-button", "Home");
    });

    test("Checks the details are on the feedback confirmation page when an email was supplied", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22%22%2C%22WhenWasQualificationStarted%22%3A%22%22%2C%22WhenWasQualificationAwarded%22%3A%22%22%2C%22LevelOfQualification%22%3A%22%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%2C%22SearchCriteria%22%3A%22%22%2C%22AdditionalQuestionsAnswers%22%3A%7B%7D%2C%22QualificationWasSelectedFromList%22%3A0%2C%22HasSubmittedEmailAddressInFeedbackFormQuestion%22%3Atrue%7D', journeyCookieName);
        await page.goto("/give-feedback/confirmation");

        await isVisible(page, "#success-message");
        await isVisible(page, "#main-confirmation-body");
        await isVisible(page, "#optional-body-heading");
        await isVisible(page, "#optional-confirmation-body");
        await isVisible(page, "#return-button");

        await checkText(page, "#success-message" ,"Your feedback has been successfully submitted");
        await checkTextContains(page, "#main-confirmation-body" ,"Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        await checkTextContains(page, "#optional-body-heading" ,"What happens next");
        await checkTextContains(page, "#optional-confirmation-body" ,"As you agreed to be contacted about future research, someone from our research team may contact you by email.");
        await checkText(page, "#return-button", "Home");
    });
});