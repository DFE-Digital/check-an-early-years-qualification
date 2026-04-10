import { test } from '@playwright/test';
import {
    startJourney,
    checkText,
    checkValue,
    exists,
    checkUrl,
    inputText,
    isVisible,
    doesNotExist,
    hasClass,
    doesNotHaveClass,
    checkError,
    InputQualificationStartedAndAwardedDetailsOnHelpPage,
    clickSubmit
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the help qualification page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);

        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");

        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");

        await checkUrl(page, "/help/qualification-details");
    });

    test("Checks the content is on the page", async ({ page, context }) => {
        await checkText(page, "#enquiry-heading", "What are the qualification details?");
        await checkText(page, "#post-heading-content", "We need to know the following qualification details to quickly and accurately respond to any questions you may have.");
        await checkText(page, "#qualification-name-heading", "Qualification name");
        await checkText(page, "#started-header", "Start date (optional)");
        await checkText(page, "#started-month-label", "Month");
        await checkText(page, "#started-year-label", "Year");
        await checkText(page, "#awarded-header", "Award date");
        await checkText(page, "#awarded-month-label", "Month");
        await checkText(page, "#started-year-label", "Year");
        await checkText(page, "#awarding-organisation-heading", "Awarding organisation");
        await checkText(page, "#question-submit", "Continue")
    });

    test("All details entered navigates to correct page", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", "1");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", "2001");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");

        await checkUrl(page, "/help/provide-details");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", "1");
        await inputText(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", "2001");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");

        await page.goBack();

        await checkUrl(page, "/help/qualification-details");
        await checkValue(page, "#QualificationName", "Entered qualification name");
        await checkValue(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", "1");
        await checkValue(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", "2001");
        await checkValue(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await checkValue(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await checkValue(page, "#AwardingOrganisation", "Entered awarding organisation");
    });

    test("All required fields valid, optional start date not entered submits to next page", async ({ page }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");

        await checkUrl(page, "/help/provide-details");
    });

    test("shows the awarded month and year missing error message when a user doesnt type an awarded date on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await InputQualificationStartedAndAwardedDetailsOnHelpPage(page, "1", "2025", "", "");
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "Enter the month and year that the qualification was awarded");
        await doesNotExist(page, '#started-error');
        await exists(page, "#awarded-error");
        await checkError(page, '#awarded-error', "Enter the date the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month missing error message when a user doesnt type a started month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "Enter the month that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started year missing error message when a user doesnt type a year month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedYear", "Enter the year that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the year that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message when a user doesnt type an awarded month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "Enter the month that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year missing error message when a user doesnt type an awarded year on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedYear", "Enter the year that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the year that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message when a user enters a later awarded date on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await InputQualificationStartedAndAwardedDetailsOnHelpPage(page, "1", "2025", "12", "2020");
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The awarded date must be after the started date");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message when a user enters a same awarded date on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await InputQualificationStartedAndAwardedDetailsOnHelpPage(page, "1", "2025", "1", "2025");
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The awarded date must be after the started date");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing started month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await InputQualificationStartedAndAwardedDetailsOnHelpPage(page, "", "2025", "12", "2020");
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "Enter the month that the qualification was started");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The awarded date must be after the started date");
        await exists(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was started");
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing awarded month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await InputQualificationStartedAndAwardedDetailsOnHelpPage(page, "1", "2025", "", "2020");
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "Enter the month that the qualification was awarded", 0);
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The awarded date must be after the started date", 1);
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awardedThe awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test.describe("When the started month selected on the help qualification-details page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#started-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedMonth', value);
                await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
                await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the started year selected on the help qualification-details page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#started-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedMonth', '1');
                await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "The year the qualification was started must be between 1900 and 2026");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await doesNotHaveClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
                await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the awarded month selected on the help qualification-details page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#awarded-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedMonth', value);
                await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
                await doesNotHaveClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the awarded year selected on the help qualification-details page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#awarded-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedMonth', '1');
                await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "The year the qualification was awarded must be between 1900 and 2026");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await doesNotHaveClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
                await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test("shows the started month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedMonth', '0');
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12The year the qualification was started must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedYear", "Enter the year that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12Enter the year that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.StartedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedMonth", "Enter the month that the qualification was started");
        await checkText(page, "#error-banner-link-QuestionModel\\.StartedQuestion\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was startedThe year the qualification was started must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedMonth', '0');
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12The year the qualification was awarded must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedYear", "Enter the year that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12Enter the year that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the help qualification-details page", async ({ page }) => {
        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#QuestionModel\\.AwardedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedMonth", "Enter the month that the qualification was awarded");
        await checkText(page, "#error-banner-link-QuestionModel\\.AwardedQuestion\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awardedThe year the qualification was awarded must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });
});