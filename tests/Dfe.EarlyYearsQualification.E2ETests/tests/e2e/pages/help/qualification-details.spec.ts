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
    whenWasQualificationStarted,
    clickSubmit
} from '../../../_shared/playwrightWrapper';

test.describe('A spec that tests the help qualification page', { tag: "@e2e" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);

        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#reason-for-enquiring-form-submit");
        await checkUrl(page, "/help/qualification-details");
    });

    test("Checks the content is on the page", async ({ page, context }) => {
        await checkText(page, "#help-page-heading", "What are the qualification details?");
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

    test("All required fields vaid, optional start date not entered submits to next page", async ({ page }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedMonth", "2");
        await inputText(page, "#QuestionModel\\.AwardedQuestion\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");

        await checkUrl(page, "/help/provide-details");
    });

    test("shows the started month and year missing error message when a user doesnt type a started date on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await whenWasQualificationStarted(page, "", "", "1", "2025");
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Test error banner link text");
        await exists(page, '#started-error');
        await doesNotExist(page, "#awarded-error");
        await checkError(page, '#started-error', "started- Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month and year missing error message when a user doesnt type an awarded date on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await whenWasQualificationStarted(page, "1", "2025", "", "");
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Test error banner link text");
        await doesNotExist(page, '#started-error');
        await exists(page, "#awarded-error");
        await checkError(page, '#awarded-error', "awarded- Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started and awarded month and year missing error message when a user doesnt type a date on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Test error banner link text");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Test error banner link text");
        await exists(page, '#started-error');
        await exists(page, "#awarded-error");
        await checkError(page, '#started-error', "started- Test Error Message");
        await checkError(page, '#awarded-error', "awarded- Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month missing error message when a user doesnt type a started month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Missing Month Banner Link Text");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Month Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started year missing error message when a user doesnt type a year month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedYear", "started- Missing Year Banner Link Text");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message when a user doesnt type an awarded month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Missing Month Banner Link Text");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Month Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year missing error message when a user doesnt type an awarded year on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedYear", "awarded- Missing Year Banner Link Text");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message when a user enters a later awarded date on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "12", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "Error- AwardedDateIsAfterStartedDateErrorText");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Error- AwardedDateIsAfterStartedDateErrorText");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message when a user enters a same awarded date on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "1", "2025");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "Error- AwardedDateIsAfterStartedDateErrorText");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Error- AwardedDateIsAfterStartedDateErrorText");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing started month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "", "2025", "12", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Missing Month Banner Link Text");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "Error- AwardedDateIsAfterStartedDateErrorText");
        await exists(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#started-error', "started- Missing Month Error Message");
        await checkError(page, '#awarded-error', "Error- AwardedDateIsAfterStartedDateErrorText");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });


    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing awarded month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Missing Month Banner Link Text", 0);
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "Error- AwardedDateIsAfterStartedDateErrorText", 1);
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Month Error MessageError- AwardedDateIsAfterStartedDateErrorText");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test.describe("When the started month selected on the when-was-the-qualification-started-and-awarded page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await page.goto("/questions/when-was-the-qualification-started-and-awarded");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#started-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#StartedQuestion\\.SelectedMonth', value);
                await page.fill('#StartedQuestion\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Month Out Of Bounds Error Link Text");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "started- Month Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
                await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the started year selected on the when-was-the-qualification-started-and-awarded page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await page.goto("/questions/when-was-the-qualification-started-and-awarded");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#started-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#StartedQuestion\\.SelectedMonth', '1');
                await page.fill('#StartedQuestion\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedYear", "started- Year Out Of Bounds Error Link Text");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "started- Year Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
                await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the awarded month selected on the when-was-the-qualification-started-and-awarded page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await page.goto("/questions/when-was-the-qualification-started-and-awarded");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#awarded-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#AwardedQuestion\\.SelectedMonth', value);
                await page.fill('#AwardedQuestion\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Month Out Of Bounds Error Link Text");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "awarded- Month Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
                await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
                await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test.describe("When the awarded year selected on the when-was-the-qualification-started-and-awarded page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await page.goto("/questions/when-was-the-qualification-started-and-awarded");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#awarded-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#AwardedQuestion\\.SelectedMonth', '1');
                await page.fill('#AwardedQuestion\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedYear", "awarded- Year Out Of Bounds Error Link Text");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "awarded- Year Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
                await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
                await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test("shows the started month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedMonth', '0');
        await page.fill('#StartedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Month Out Of Bounds Error Link Text");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedYear", "started- Year Out Of Bounds Error Link Text");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Month Out Of Bounds Error Messagestarted- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Month Out Of Bounds Error Link Text");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedYear", "started- Missing Year Banner Link Text");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Month Out Of Bounds Error Messagestarted- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedMonth", "started- Missing Month Banner Link Text");
        await checkText(page, "#error-banner-link-StartedQuestion\\.SelectedYear", "started- Year Out Of Bounds Error Link Text");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Month Error Messagestarted- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedMonth', '0');
        await page.fill('#AwardedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Month Out Of Bounds Error Link Text");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedYear", "awarded- Year Out Of Bounds Error Link Text");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Month Out Of Bounds Error Messageawarded- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Month Out Of Bounds Error Link Text");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedYear", "awarded- Missing Year Banner Link Text");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Month Out Of Bounds Error Messageawarded- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started-and-awarded page", async ({ page }) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedMonth", "awarded- Missing Month Banner Link Text");
        await checkText(page, "#error-banner-link-AwardedQuestion\\.SelectedYear", "awarded- Year Out Of Bounds Error Link Text");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Month Error Messageawarded- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });
});