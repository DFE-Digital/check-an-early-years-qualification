import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    inputText,
    isVisible,
    isNotVisible,
    hasClass,
    checkTextContains} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the get help page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the technical content is on the page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");

        await checkText(page, "#back-button", "Back to get help with the Check an early years qualification service");
        await checkText(page, "#additional-information-heading", "Tell us about the technical issue");
        await checkText(page, "#additional-information-hint", "Give as much detail as you can about the technical issue you are experiencing");
        await checkText(page, "#warning-text-container > strong", "Warning Do not include any personal information");
        await checkText(page, "#question-submit", "Continue");
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
        await checkText(page, "#back-button", "Back to what are the qualification details");
        await isNotVisible(page, "#no-js-character-limit-hint");
        await isVisible(page, "#character-limit-hint");
        await checkText(page, "#additional-information-heading", "How can we help you?");
        await checkText(page, "#additional-information-hint", "Give as much detail as you can. This helps us give you the right support.");
        await checkText(page, "#warning-text-container > strong", "Warning Do not include any personal information");
        await checkText(page, "#question-submit", "Continue");
    });

    test("Check back button links to correct page depending if QuestionAboutAQualification selected", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await checkUrl(page, "/help/proceed-with-qualification-query");

        await checkText(page, "#back-button", "Back to get help");
        await page.click("#back-button");
        await checkUrl(page, "/help/help/get-help");
    });

    test("Check back button links to correct page depending if IssueWithTheService selected", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");

        await checkText(page, "#back-button", "Back to get help with the Check an early years qualification service");
        await page.click("#back-button");
        await checkUrl(page, "/help/get-help");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");

        await inputText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
        await page.click("#question-submit");
        await page.goBack();
        await checkText(page, "#ProvideAdditionalInformation", "This is some additional info the user has entered");
    });

    test("Displays an error message when a user doesnt enter required details", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");

        await page.click("#question-submit");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Provide information about how we can help you");
        await checkTextContains(page, "#additional-information-error", "Provide information about how we can help you");
        await isNotVisible(page, "#too-many-characters-error");
    });

    test("Displays an error message when a user enters too many characters", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        
        // 1001 characters
        let message = "ugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll1oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol2lll33oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol8lll99oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll22oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol6lll77oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol5lll66oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol4lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll11oewrftoergopkmomegiomeiogmioergmiomimiomiimmiims1";

        await inputText(page, "#ProvideAdditionalInformation", message);
        
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Enter 1,000 characters or less");
        await isNotVisible(page, "#additional-information-error");
        await checkTextContains(page, "#too-many-characters-error", "Enter 1,000 characters or less");
    });

    test("Continues to the next page if the number of entered characters is equal to the limit", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");

        // 1000 characters
        let message = "ugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll1oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol2lll33oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol8lll99oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll22oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol6lll77oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol5lll66oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol4lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll11oewrftoergopkmomegiomeiogmioergmiomimiomiimmiims";

        await inputText(page, "#ProvideAdditionalInformation", message);

        await page.click("#question-submit");
        await checkUrl(page, "/help/email-address");
    });

    test("Check the character limit hint text changes when the user enters 5 characters", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await checkTextContains(page, "#character-limit-hint", "You have 1,000 characters remaining");

        // 5 characters
        let message = "12345";
        // pressSequentially triggers the key-up event that is used by JQuery to change the hint text
        await page.locator("#ProvideAdditionalInformation").pressSequentially(message);

        await checkTextContains(page, "#character-limit-hint", "You have 995 characters remaining");
    });

    test("Check the character limit hint text changes when the user enters 999 characters", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await checkTextContains(page, "#character-limit-hint", "You have 1,000 characters remaining");

        // 999 characters
        let message = "ugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll1oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol2lll33oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol8lll99oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll22oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol6lll77oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol5lll66oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol4lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll11oewrftoergopkmomegiomeiogmioergmiomimiomiimmiim";
        // pressSequentially triggers the key-up event that is used by JQuery to change the hint text
        await page.locator("#ProvideAdditionalInformation").pressSequentially(message);

        await checkTextContains(page, "#character-limit-hint", "You have 1 character remaining");
    });

    test("Check the character limit hint text changes when the user enters 1001 characters", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await checkTextContains(page, "#character-limit-hint", "You have 1,000 characters remaining");

        // 1001 characters
        let message = "ugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll1oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol2lll33oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol8lll99oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll22oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol6lll77oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol5lll66oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol4lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll11oewrftoergopkmomegiomeiogmioergmiomimiomiimmiim12";
        // pressSequentially triggers the key-up event that is used by JQuery to change the hint text
        await page.locator("#ProvideAdditionalInformation").pressSequentially(message);

        await checkTextContains(page, "#character-limit-hint", "You have entered 1 character too many");
        await hasClass(page, "#character-limit-hint", "govuk-error-message")
    });

    test("Check the character limit hint text changes when the user enters 1002 characters", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#IssueWithTheService");
        await page.click("#form-submit");
        await checkUrl(page, "/help/provide-details");
        await checkTextContains(page, "#character-limit-hint", "You have 1,000 characters remaining");

        // 1002 characters
        let message = "ugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll1oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol2lll33oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol8lll99oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll22oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol6lll77oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol5lll66oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol4lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol1lll44oewrftoergopkmomegiomeiogmioergmiomimiomiimmiimugurthnurthnurthurthjrtijrtjirtjgijrthiojikjkol3lll11oewrftoergopkmomegiomeiogmioergmiomimiomiimmiim123";
        // pressSequentially triggers the key-up event that is used by JQuery to change the hint text
        await page.locator("#ProvideAdditionalInformation").pressSequentially(message);

        await checkTextContains(page, "#character-limit-hint", "You have entered 2 characters too many");
        await hasClass(page, "#character-limit-hint", "govuk-error-message")
    });
});