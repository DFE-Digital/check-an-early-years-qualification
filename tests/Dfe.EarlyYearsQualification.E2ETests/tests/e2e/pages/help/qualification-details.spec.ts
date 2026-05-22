import { test } from '@playwright/test';
import { Page } from '@playwright/test';
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
    clickSubmit,
    checkElementIsChecked,
    checkAwardedFieldErrors,
    checkStartedFieldErrors,
    checkErrorSummary,
    fillQualificationDates,
    checkNoErrorsPresent,
    whereWasTheQualificationAwarded,
    startedBeforeSeptember2014,
    startedOnOrAfterSeptember2014,
    whenWasQualificationAwarded,
    checkingOwnQualificationOrSomeoneElsesPage
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
        await checkText(page, "legend h2[for=\"question\"]", "Start date");
        await checkText(page, "#started-month-label", "Month");
        await checkText(page, "#started-year-label", "Year");
        await checkText(page, "#awarded-header", "Award date");
        await checkText(page, "#awarded-month-label", "Month");
        await checkText(page, "#started-year-label", "Year");
        await checkText(page, "#awarding-organisation-heading", "Awarding organisation");
        await checkText(page, "#question-submit", "Continue");
        await checkText(page, "label[for=\"Before1September2014\"]", "Before 1 September 2014");
        await checkText(page, "label[for=\"OnOrAfter1September2014\"]", "On or after 1 September 2014");
        await checkText(page, "#started-hint", "Enter the specific start date. For example 9 2014.");
    });

    test("All details entered navigates to correct page - before the 1st September 2014", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await page.click("#Before1September2014");
        await inputText(page, "#AwardedDate\\.SelectedMonth", "2");
        await inputText(page, "#AwardedDate\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");
    });

    test("All details entered navigates to correct page - on or after 1st September 2014", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await page.click("#OnOrAfter1September2014");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "02");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "2015");
        await inputText(page, "#AwardedDate\\.SelectedMonth", "2");
        await inputText(page, "#AwardedDate\\.SelectedYear", "2017");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await checkUrl(page, "/help/provide-details");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated - before the 1st September 2014", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await page.click("#Before1September2014");
        await inputText(page, "#AwardedDate\\.SelectedMonth", "2");
        await inputText(page, "#AwardedDate\\.SelectedYear", "2002");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await page.goBack();
        await checkUrl(page, "/help/qualification-details");
        await checkValue(page, "#QualificationName", "Entered qualification name");
        await checkElementIsChecked(page, "#Before1September2014");
        await checkValue(page, "#AwardedDate\\.SelectedMonth", "2");
        await checkValue(page, "#AwardedDate\\.SelectedYear", "2002");
        await checkValue(page, "#AwardingOrganisation", "Entered awarding organisation");
    });

    test("Navigates to next page, returns to original page their selection is pre-populated - on or after 1st September 2014", async ({ page, context }) => {
        await inputText(page, "#QualificationName", "Entered qualification name");
        await page.click("#OnOrAfter1September2014");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "2");
        await inputText(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "2015");
        await inputText(page, "#AwardedDate\\.SelectedMonth", "2");
        await inputText(page, "#AwardedDate\\.SelectedYear", "2016");
        await inputText(page, "#AwardingOrganisation", "Entered awarding organisation");
        await page.click("#question-submit");
        await page.goBack();
        await checkUrl(page, "/help/qualification-details");
        await checkValue(page, "#QualificationName", "Entered qualification name");
        await checkElementIsChecked(page, "#OnOrAfter1September2014");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "2");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "2015");
        await checkValue(page, "#AwardedDate\\.SelectedMonth", "2");
        await checkValue(page, "#AwardedDate\\.SelectedYear", "2016");
        await checkValue(page, "#AwardingOrganisation", "Entered awarding organisation");
    });

    test("shows empty qualification name / awarding org / option / awarded date error message", async ({ page, context }) => {
        await page.click("#question-submit");
        await checkText(page, "#qualification-name-error", "Error: Enter the qualification name");
        await checkText(page, "#error-banner-link-QualificationName", "Enter the qualification name");
        await checkText(page, "#option-error", "Error: Select when the qualification was started");
        await checkText(page, "#error-banner-link-Before1September2014", "Select when the qualification was started");
        await checkText(page, '#awarded-error', "Error: Enter the date the qualification was awarded");
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "Enter the month and year that the qualification was awarded");
        await checkText(page, "#qualification-organisation-error", "Error: Enter the awarding organisation");
        await checkText(page, "#error-banner-link-AwardingOrganisation", "Enter the awarding organisation");
    });

    test("Help qualification details are pre-populated from normal journey - before 1 September 2014", async ({ page, context }) => {
        await startJourney(page, context);
        await checkingOwnQualificationOrSomeoneElsesPage(page, "#no");
        await whereWasTheQualificationAwarded(page, "#england");
        await startedBeforeSeptember2014(page);
        await whenWasQualificationAwarded(page, "1", "2012");
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");
        await checkUrl(page, "/help/qualification-details");
        await checkElementIsChecked(page, "#Before1September2014");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "");
        await checkValue(page, "#AwardedDate\\.SelectedMonth", "1");
        await checkValue(page, "#AwardedDate\\.SelectedYear", "2012");
    });

    test("Help qualification details are pre-populated from normal journey - after 1 September 2014", async ({ page, context }) => {
        await startJourney(page, context);
        await checkingOwnQualificationOrSomeoneElsesPage(page, "#no");
        await whereWasTheQualificationAwarded(page, "#england");
        await startedOnOrAfterSeptember2014(page, "10", "2020");
        await whenWasQualificationAwarded(page, "1", "2021");
        await page.goto("/help/get-help");
        await page.click("#QuestionAboutAQualification");
        await page.click("#form-submit");
        await page.click("input#ContactTheEarlyYearsQualificationTeam");
        await page.click("button#form-submit");
        await checkUrl(page, "/help/qualification-details");
        await checkElementIsChecked(page, "#OnOrAfter1September2014");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "10");
        await checkValue(page, "#RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "2020");
        await checkValue(page, "#AwardedDate\\.SelectedMonth", "1");
        await checkValue(page, "#AwardedDate\\.SelectedYear", "2021");
    });

    test("shows the awarded month and year missing error message when a user doesnt type an awarded date on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await fillQualificationDates(page, "1", "2025", "", "");
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "Enter the month and year that the qualification was awarded");
        await doesNotExist(page, '#started-error');
        await exists(page, "#awarded-error");
        await checkError(page, '#awarded-error', "Enter the date the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, false, false);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the started month missing error message when a user doesnt type a started month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.click("#OnOrAfter1September2014");
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "Enter the month that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, true, false);
    });

    test("shows the started year missing error message when a user doesnt type a year month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.click("#OnOrAfter1September2014");
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "Enter the year that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the year that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, false, true);
    });

    test("shows the awarded month missing error message when a user doesnt type an awarded month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.fill('#AwardedDate\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "Enter the month that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkAwardedFieldErrors(page, true, false);
    });

    test("shows the awarded year missing error message when a user doesnt type an awarded year on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.fill('#AwardedDate\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedYear", "Enter the year that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the year that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkAwardedFieldErrors(page, false, true);
    });

    test("shows the awarded year is before start year error message when a user enters a later awarded date on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await fillQualificationDates(page, "1", "2025", "1", "2000");
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The awarded date must be after the started date");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, false, false);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the awarded year is before start year error message when a user enters a same awarded date on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await fillQualificationDates(page, "1", "2025", "1", "2025");
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The awarded date must be after the started date");
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, false, false);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing started month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await fillQualificationDates(page, "", "2025", "12", "2020");
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "Enter the month that the qualification was started");
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The awarded date must be after the started date");
        await exists(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was started");
        await checkError(page, '#awarded-error', "The awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, true, false);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing awarded month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await fillQualificationDates(page, "1", "2025", "", "2020");
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "Enter the month that the qualification was awarded", 0);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The awarded date must be after the started date", 1);
        await doesNotExist(page, "#started-error");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awardedThe awarded date must be after the started date");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, false, false);
        await checkAwardedFieldErrors(page, true, true);
    });

    test.describe("When the started month selected on the help qualification-details page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await checkNoErrorsPresent(page);
                await page.click("#OnOrAfter1September2014");
                await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth', value);
                await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await checkErrorSummary(page);
                await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await checkStartedFieldErrors(page, true, false);
            });
        });
    });

    test.describe("When the started year selected on the help qualification-details page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await checkNoErrorsPresent(page);
                await page.click("#OnOrAfter1September2014");
                await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth', '1');
                await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await checkErrorSummary(page);
                await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
                await exists(page, '#started-error');
                await checkError(page, '#started-error', "The year the qualification was started must be between 1900 and 2026");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await checkStartedFieldErrors(page, false, true);
            });
        });
    });

    test.describe("When the awarded month selected on the help qualification-details page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({ page }) => {
                await checkNoErrorsPresent(page);
                await page.fill('#AwardedDate\\.SelectedMonth', value);
                await page.fill('#AwardedDate\\.SelectedYear', '2024');
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await checkErrorSummary(page);
                await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await checkAwardedFieldErrors(page, true, false);
            });
        });
    });

    test.describe("When the awarded year selected on the help qualification-details page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({ page }) => {
                await checkNoErrorsPresent(page);
                await page.fill('#AwardedDate\\.SelectedMonth', '1');
                await page.fill('#AwardedDate\\.SelectedYear', value);
                await clickSubmit(page);
                await checkUrl(page, "/help/qualification-details");
                await checkErrorSummary(page);
                await checkText(page, "#error-banner-link-AwardedDate\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "The year the qualification was awarded must be between 1900 and 2026");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await checkAwardedFieldErrors(page, false, true);
            });
        });
    });

    test("shows the started month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.click("#OnOrAfter1September2014");
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth', '0');
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12The year the qualification was started must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, true, true);
    });

    test("shows the started month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.click("#OnOrAfter1September2014");
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "The month the qualification was started must be between 1 and 12");
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "Enter the year that the qualification was started");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "The month the qualification was started must be between 1 and 12Enter the year that the qualification was started");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, true, true);
    });

    test("shows the started month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.click("#OnOrAfter1September2014");
        await page.fill('#RadioButtonWithDateInputModel\\.Question\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedMonth", "Enter the month that the qualification was started");
        await checkText(page, "#error-banner-link-RadioButtonWithDateInputModel\\.Question\\.SelectedYear", "The year the qualification was started must be between 1900 and 2026");
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "Enter the month that the qualification was startedThe year the qualification was started must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkStartedFieldErrors(page, true, true);
    });

    test("shows the awarded month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.fill('#AwardedDate\\.SelectedMonth', '0');
        await page.fill('#AwardedDate\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12The year the qualification was awarded must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the awarded month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.fill('#AwardedDate\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "The month the qualification was awarded must be between 1 and 12");
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedYear", "Enter the year that the qualification was awarded");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "The month the qualification was awarded must be between 1 and 12Enter the year that the qualification was awarded");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkAwardedFieldErrors(page, true, true);
    });

    test("shows the awarded month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the help qualification-details page", async ({ page }) => {
        await checkNoErrorsPresent(page);
        await page.fill('#AwardedDate\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/help/qualification-details");
        await checkErrorSummary(page);
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedMonth", "Enter the month that the qualification was awarded");
        await checkText(page, "#error-banner-link-AwardedDate\\.SelectedYear", "The year the qualification was awarded must be between 1900 and 2026");
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "Enter the month that the qualification was awardedThe year the qualification was awarded must be between 1900 and 2026");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await checkAwardedFieldErrors(page, true, true);
    });
});
