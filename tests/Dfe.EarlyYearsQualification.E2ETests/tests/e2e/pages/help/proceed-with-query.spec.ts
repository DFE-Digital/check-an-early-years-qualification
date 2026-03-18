import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    exists,
    checkUrl,
    isVisible,
    checkTextContains,
    checkElementIsChecked
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the proceed with query page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    test("Checks the content is on the page", async ({page, context}) => {
        await page.goto("/help/get-help");
        await page.click("input#QuestionAboutAQualification");
        await page.click("button#form-submit");

        await checkText(page, "#enquiry-heading", "Check the qualification before contacting us");
        await checkText(page, "#question-heading", "What do you want to do next?");

        await exists(page, "#CheckTheQualificationUsingTheService");
        await checkText(page, "label[for='CheckTheQualificationUsingTheService']", "Check the qualification using the service");

        await exists(page, "#ContactTheEarlyYearsQualificationTeam");
        await checkText(page, "label[for='ContactTheEarlyYearsQualificationTeam']", "Contact the early years qualification team");
    
        await checkText(page, "#form-submit", "Continue")
    });

    test("Checks the CheckTheQualificationUsingTheService enquiry reason navigates to correct page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");

        await page.click("input#CheckTheQualificationUsingTheService");
        await page.click("button#form-submit");

        await checkUrl(page, "/");
    });

    test("Checks the ContactTheEarlyYearsQualificationTeam enquiry reason navigates to correct page", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");

        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");

        await checkUrl(page, "/help/qualification-details");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page, context }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");

        await checkUrl(page, "/help/qualification-details");

        await page.goBack();

        await checkElementIsChecked(page, "#ContactTheEarlyYearsQualificationTeam");
    });

    test("shows an error message when a user doesnt enter required details on help page", async ({ page }) => {
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");

        await checkUrl(page, "/help/proceed-with-qualification-query");

        await page.click("#form-submit");

        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, ".govuk-error-summary__list > li", "Select one option", 0);

        await isVisible(page, "#option-error");
        await checkTextContains(page, "#option-error", "Select one option");

        await checkText(page, "#question-heading", "What do you want to do next?");
    });
});