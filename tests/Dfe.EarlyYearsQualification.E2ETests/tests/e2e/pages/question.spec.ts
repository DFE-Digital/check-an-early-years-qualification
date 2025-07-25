﻿import {test} from '@playwright/test';
import {
    startJourney,
    checkDisclaimer,
    checkText,
    checkError,
    checkUrl,
    setCookie,
    journeyCookieName,
    exists,
    doesNotExist,
    hasClass,
    doesNotHaveClass,
    isVisible,
    hasAttribute,
    doesNotHaveAttribute,
    whenWasQualificationStarted,
    clickSubmit
} from '../../_shared/playwrightWrapper';

test.describe("A spec that tests question pages", {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the content on where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");

        await checkText(page, "#question", "Where was the qualification awarded?");
        await exists(page, "#england");
        await exists(page, "#scotland");
        await exists(page, "#wales");
        await exists(page, "#northern-ireland");
        await checkText(page, ".govuk-radios__divider", "or")
        await exists(page, "#outside-uk");
    });

    test("Checks additional information on the where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");
        await doesNotHaveAttribute(page, ".govuk-details", "open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await hasAttribute(page, ".govuk-details", "open");
    });

    test("shows an error message when a user doesnt select an option on the where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#option-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await clickSubmit(page);
        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#option-error");
        await checkError(page, '#option-error', "Test error message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
    });

    test("Checks the content on when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await checkText(page, "#question", "Test Dates Questions");

        await checkText(page, "#started-header", "started- Test Question Hint Header");
        await checkText(page, "#started-hint", "started- Test Question Hint");
        await checkText(page, "#started-month-label", "started- Test Month Label");
        await checkText(page, "#started-year-label", "started- Test Year Label");
        await exists(page, "#StartedQuestion\\.SelectedMonth");
        await exists(page, "#StartedQuestion\\.SelectedYear");

        await checkText(page, "#awarded-header", "awarded- Test Question Hint Header");
        await checkText(page, "#awarded-hint", "awarded- Test Question Hint");
        await checkText(page, "#awarded-month-label", "awarded- Test Month Label");
        await checkText(page, "#awarded-year-label", "awarded- Test Year Label");
        await exists(page, "#AwardedQuestion\\.SelectedMonth");
        await exists(page, "#AwardedQuestion\\.SelectedYear");
    });

    test("shows the started month and year missing error message when a user doesnt type a started date on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await whenWasQualificationStarted(page, "", "", "1", "2025");
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Test error banner link text");
        await exists(page, '#started-error');
        await doesNotExist(page, "#awarded-error");
        await checkError(page, '#started-error', "started- Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month and year missing error message when a user doesnt type an awarded date on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await whenWasQualificationStarted(page, "1", "2025", "", "");
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Test error banner link text");
        await doesNotExist(page, '#started-error');
        await exists(page, "#awarded-error");
        await checkError(page, '#awarded-error', "awarded- Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started and awarded month and year missing error message when a user doesnt type a date on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Test error banner link text", 0);
        await checkText(page, "#error-banner-link", "awarded- Test error banner link text", 1);
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

    test("shows the started month missing error message when a user doesnt type a started month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Missing Month Banner Link Text", 0);
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Month Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started year missing error message when a user doesnt type a year month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Missing Year Banner Link Text", 0);
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message when a user doesnt type an awarded month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedYear', '2024');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Missing Month Banner Link Text", 1);
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Month Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year missing error message when a user doesnt type an awarded year on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedMonth', '10');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Missing Year Banner Link Text", 1);
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded year is before start year error message when a user enters a later awarded date on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "12", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Error- AwardedDateIsAfterStartedDateErrorText");
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

    test("shows the awarded year is before start year error message when a user enters a same awarded date on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "1", "2025");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Error- AwardedDateIsAfterStartedDateErrorText");
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

    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing started month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "", "2025", "12", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Missing Month Banner Link Text", 0);
        await checkText(page, "#error-banner-link", "Error- AwardedDateIsAfterStartedDateErrorText", 1);
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


    test("shows the awarded year is before start year error message and missing month error when a user enters a later awarded year but missing awarded month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);

        await whenWasQualificationStarted(page, "1", "2025", "", "2020");

        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Missing Month Banner Link Text", 0);
        await checkText(page, "#error-banner-link", "Error- AwardedDateIsAfterStartedDateErrorText", 1);
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
            test(`is ${value} then it shows the month out of bounds error message`, async ({page}) => {
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
                await checkText(page, "#error-banner-link", "started- Month Out Of Bounds Error Link Text", 0);
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
            test(`is ${value} then it shows the incorrect year format error message`, async ({page}) => {
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
                await checkText(page, "#error-banner-link", "started- Year Out Of Bounds Error Link Text", 0);
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
            test(`is ${value} then it shows the month out of bounds error message`, async ({page}) => {
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
                await checkText(page, "#error-banner-link", "awarded- Month Out Of Bounds Error Link Text", 1);
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
            test(`is ${value} then it shows the incorrect year format error message`, async ({page}) => {
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
                await checkText(page, "#error-banner-link", "awarded- Year Out Of Bounds Error Link Text", 1);
                await exists(page, '#awarded-error');
                await checkError(page, '#awarded-error', "awarded- Year Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 3);
                await doesNotHaveClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
                await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
            });
        });
    });

    test("shows the started month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
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
        await checkText(page, "#error-banner-link", "started- Month Out Of Bounds Error Link Text", 0);
        await checkText(page, "#error-banner-link", "started- Year Out Of Bounds Error Link Text", 1);
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Month Out Of Bounds Error Messagestarted- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Month Out Of Bounds Error Link Text", 0);
        await checkText(page, "#error-banner-link", "started- Missing Year Banner Link Text", 1);
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Month Out Of Bounds Error Messagestarted- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the started month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#started-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#StartedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "started- Missing Month Banner Link Text", 0);
        await checkText(page, "#error-banner-link", "started- Year Out Of Bounds Error Link Text", 1);
        await exists(page, '#started-error');
        await checkError(page, '#started-error', "started- Missing Month Error Messagestarted- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#StartedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#StartedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
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
        await checkText(page, "#error-banner-link", "awarded- Month Out Of Bounds Error Link Text", 1);
        await checkText(page, "#error-banner-link", "awarded- Year Out Of Bounds Error Link Text", 2);
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Month Out Of Bounds Error Messageawarded- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedMonth', '0');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Month Out Of Bounds Error Link Text", 1);
        await checkText(page, "#error-banner-link", "awarded- Missing Year Banner Link Text", 2);
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Month Out Of Bounds Error Messageawarded- Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });

    test("shows the awarded month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started-and-awarded page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started-and-awarded");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#awarded-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#AwardedQuestion\\.SelectedYear', '20');
        await clickSubmit(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "awarded- Missing Month Banner Link Text", 1);
        await checkText(page, "#error-banner-link", "awarded- Year Out Of Bounds Error Link Text", 2);
        await exists(page, '#awarded-error');
        await checkError(page, '#awarded-error', "awarded- Missing Month Error Messageawarded- Year Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#AwardedQuestion\\.SelectedMonth", /govuk-input--error/);
        await hasClass(page, "#AwardedQuestion\\.SelectedYear", /govuk-input--error/);
    });


    test("Checks the content on what-level-is-the-qualification page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await checkText(page, "#question", "What level is the qualification?");
        await exists(page, 'input[id="2"]');
        await exists(page, 'input[id="3"]');
        await exists(page, 'input[id="6"]');
        await exists(page, 'label[for="6"]');
        await checkText(page, 'div[id="6_hint"]', "Some hint text");
        await exists(page, 'input[id="7"]');
    });

    test("Checks additional information on the what-level-is-the-qualification page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await doesNotHaveAttribute(page, ".govuk-details", "open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await hasAttribute(page, ".govuk-details", "open");
    });

    test("shows an error message when a user doesnt select an option on the what-level-is-the-qualification page", async ({
                                                                                                                              page,
                                                                                                                              context
                                                                                                                          }) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#option-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await clickSubmit(page);
        await checkUrl(page, "/questions/what-level-is-the-qualification");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#option-error");
        await checkError(page, '#option-error', "Test error message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
    });

    test("Checks the content on what-is-the-awarding-organisation page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");

        await checkText(page, '#question', "Test Dropdown Question");
        await exists(page, "#awarding-organisation-select");
        await exists(page, "#awarding-organisation-not-in-list");
        await exists(page, '#question-submit');
    });

    test("Checks additional information on the what-is-the-awarding-organisation page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");

        await doesNotHaveAttribute(page, ".govuk-details", "open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await hasAttribute(page, ".govuk-details", "open");
    });

    test("shows an error message when a user doesnt select an option from the dropdown list" +
        "and also does not check 'not in the list' on the what-is-the-awarding-organisation", async ({
                                                                                                         page,
                                                                                                         context
                                                                                                     }) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#dropdown-error");
        await doesNotHaveClass(page, "#awarding-organisation-select", /govuk-select--error/);
        await clickSubmit(page);
        await checkUrl(page, "/questions/what-is-the-awarding-organisation");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#dropdown-error");
        await checkError(page, '#dropdown-error', "Test Error Message");
        await hasClass(page, "#awarding-organisation-select", /govuk-select--error/);
    });

    test("Checks the content on pre-check page", async ({page}) => {
        await page.goto("/questions/pre-check");

        await checkText(page, '#header', "Get ready to start the qualification check");
        await checkText(page, '#post-header-content', "This is the post header content");
        await checkText(page, '#question', "Do you have all the information you need to complete the check?");
        await exists(page, 'input[id="yes"]');
        await exists(page, 'input[id="no"]');
        await checkDisclaimer(page, "You need all the information listed above to get a result. If you do not have it, you will not be able to complete this check.");
        await checkText(page, '#pre-check-submit', "Continue");
    });

    test("Checks the error message content on pre-check page when a user doesn't select an option", async ({page}) => {
        await page.goto("/questions/pre-check");

        await doesNotExist(page, "#error-banner");
        await doesNotExist(page, "#error-banner-link");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 1);
        await page.click("#pre-check-submit");
        await checkUrl(page, "/questions/pre-check");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        const errorMessage = "Confirm if you have all the information you need to complete the check";
        await checkText(page, "#error-banner-link", errorMessage);
        await checkError(page, '#option-error', errorMessage);
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 1);
    });
});