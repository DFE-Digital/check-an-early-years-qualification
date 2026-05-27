import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    inputText,
    isVisible,
    checkTextContains,
    exists
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the confirmation page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the qualification query content is on the page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");
        await inputText(page, "#QualificationName", "Entered qualification name");
        await page.click("#OnOrAfter1September2014");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "1");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "2015");
        await inputText(page, "#AwardedDate\\.SelectedMonth", "2");
        await inputText(page, "#AwardedDate\\.SelectedYear", "2022");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");

        await checkText(page, ".govuk-panel__title", "Message sent");
        await checkText(page, "#main-content > div > div > p", "Your message was successfully sent to the Check an early years qualification team.");
        await checkText(page, "#help-confirmation-body-heading", "What happens next");
        await checkText(page, "#help-confirmation-body > p:nth-child(1)", "The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer. We may need to contact you for more information before we can respond.");
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
        await checkText(page, "#feedback-form-submit", "Submit feedback");
        await checkText(page, "#post-heading-form-content", "Post Feedback Form Content");
        
    });

    test("Checks the technical content is on the page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");

        await checkText(page, ".govuk-panel__title", "Message sent");
        await checkText(page, "#main-content > div > div > p", "Your message was successfully sent to the Check an early years qualification team.");
        await checkText(page, "#help-confirmation-body-heading", "What happens next");
        await checkText(page, "#help-confirmation-body > p:nth-child(1)", "We may need to contact you for more information about the issue you are experiencing with the service.");
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
        await checkText(page, "#feedback-form-submit", "Submit feedback");
        await checkText(page, "#post-heading-form-content", "Post Feedback Form Content");
    });

    test("Press submit on feedback form without entering details progresses to confirmation page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await inputText(page, "#EmailAddress", "test@test.com");
        await page.click("#question-submit");

        await page.locator("#feedback-form-submit").click();
        await isVisible(page, ".govuk-panel__title");
        await isVisible(page, "#main-confirmation-body");
        await isVisible(page, "#return-button");
        await checkText(page, ".govuk-panel__title", "Your feedback has been successfully submitted");
        await checkTextContains(page, "#main-confirmation-body", "Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone.");
        await checkText(page, "#return-button", "Home");
    });
});