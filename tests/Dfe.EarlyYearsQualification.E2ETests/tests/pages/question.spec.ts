import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    checkUrl,
    setCookie,
    journeyCookieName,
    exists,
    doesNotExist,
    hasClass,
    doesNotHaveClass,
    isVisible,
    hasAttribute,
    doesNotHaveAttribute
} from '../shared/playwrightWrapper';

test.describe("A spec that tests question pages", () => {
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
        await page.click("#question-submit");
        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#option-error");
        await checkText(page, '#option-error', "Test error message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
    });

    test("Checks the content on when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await checkText(page, "#question", "Test Date Question");
        await exists(page, "#date-started-month");
        await exists(page, "#date-started-year");
    });

    test("Checks additional information on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotHaveAttribute(page, ".govuk-details", "open");

        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await hasAttribute(page, ".govuk-details", "open");
    });

    test("shows the month and year missing error message when a user doesnt type a date on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Test Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#date-started-month", /govuk-input--error/);
        await hasClass(page, "#date-started-year", /govuk-input--error/);
    });

    test("shows the month missing error message when a user doesnt type a month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#date-started-year', '2024');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Month Banner Link Text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Missing Month Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#date-started-month", /govuk-input--error/);
        await doesNotHaveClass(page, "#date-started-year", /govuk-input--error/);
    });

    test("shows the year missing error message when a user doesnt type a month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#date-started-month', '10');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Year Banner Link Text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Missing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await doesNotHaveClass(page, "#date-started-month", /govuk-input--error/);
        await hasClass(page, "#date-started-year", /govuk-input--error/);
    });

    test.describe("When the month selected on the when-was-the-qualification-started page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({page}) => {
                await page.goto("/questions/when-was-the-qualification-started");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#date-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#date-started-month', value);
                await page.fill('#date-started-year', '2024');
                await page.click("#question-submit");
                await checkUrl(page, "/questions/when-was-the-qualification-started");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link Text");
                await exists(page, '#date-error');
                await checkText(page, '#date-error', "Month Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await hasClass(page, "#date-started-month", /govuk-input--error/);
                await doesNotHaveClass(page, "#date-started-year", /govuk-input--error/);
            });
        });
    });

    test.describe("When the year selected on the when-was-the-qualification-started page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({page}) => {
                await page.goto("/questions/when-was-the-qualification-started");

                await doesNotExist(page, ".govuk-error-summary");
                await doesNotExist(page, "#date-error");
                await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await page.fill('#date-started-month', '1');
                await page.fill('#date-started-year', value);
                await page.click("#question-submit");
                await checkUrl(page, "/questions/when-was-the-qualification-started");
                await isVisible(page, ".govuk-error-summary");
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link", "Year Out Of Bounds Error Link Text");
                await exists(page, '#date-error');
                await checkText(page, '#date-error', "Year Out Of Bounds Error Message");
                await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
                await doesNotHaveClass(page, "#date-started-month", /govuk-input--error/);
                await hasClass(page, "#date-started-year", /govuk-input--error/);
            });
        });
    });

    test("shows the month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#date-started-month', '0');
        await page.fill('#date-started-year', '20');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link TextYear Out Of Bounds Error Link Text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Month Out Of Bounds Error MessageYear Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#date-started-month", /govuk-input--error/);
        await hasClass(page, "#date-started-year", /govuk-input--error/);
    });

    test("shows the month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#date-started-month', '0');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link TextMissing Year Banner Link Text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Month Out Of Bounds Error MessageMissing Year Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#date-started-month", /govuk-input--error/);
        await hasClass(page, "#date-started-year", /govuk-input--error/);
    });

    test("shows the month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await doesNotExist(page, ".govuk-error-summary");
        await doesNotExist(page, "#date-error");
        await doesNotHaveClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await page.fill('#date-started-year', '20');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Month Banner Link TextYear Out Of Bounds Error Link Text");
        await exists(page, '#date-error');
        await checkText(page, '#date-error', "Missing Month Error MessageYear Out Of Bounds Error Message");
        await hasClass(page, ".govuk-form-group", /govuk-form-group--error/, 0);
        await hasClass(page, "#date-started-month", /govuk-input--error/);
        await hasClass(page, "#date-started-year", /govuk-input--error/);
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
        await page.click("#question-submit");
        await checkUrl(page, "/questions/what-level-is-the-qualification");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#option-error");
        await checkText(page, '#option-error', "Test error message");
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
        await page.click("#question-submit");
        await checkUrl(page, "/questions/what-is-the-awarding-organisation");
        await isVisible(page, ".govuk-error-summary");
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await exists(page, "#dropdown-error");
        await checkText(page, '#dropdown-error', "Test Error Message");
        await hasClass(page, "#awarding-organisation-select", /govuk-select--error/);
    });
});