import {test, expect} from '@playwright/test';
import {startJourney, checkText, checkUrl, setCookie, journeyCookieName} from '../shared/playwrightWrapper';


test.describe("A spec that tests question pages", () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the content on where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");

        await checkText(page, "#question", "Where was the qualification awarded?");
        await expect(page.locator("#england")).toHaveCount(1);
        await expect(page.locator("#scotland")).toHaveCount(1);
        await expect(page.locator("#wales")).toHaveCount(1);
        await expect(page.locator("#northern-ireland")).toHaveCount(1);
        await checkText(page, ".govuk-radios__divider", "or")
        await expect(page.locator("#outside-uk")).toHaveCount(1);
    });

    test("Checks additional information on the where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");

        await expect(page.locator(".govuk-details")).not.toHaveAttribute("open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await expect(page.locator(".govuk-details")).toHaveAttribute("open");
    });

    test("shows an error message when a user doesnt select an option on the where-was-the-qualification-awarded page", async ({page}) => {
        await page.goto("/questions/where-was-the-qualification-awarded");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#option-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.click("#question-submit");
        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await expect(page.locator('#option-error')).toHaveCount(1);
        await checkText(page, '#option-error', "Test error message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
    });

    test("Checks the content on when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await checkText(page, "#question", "Test Date Question");
        await expect(page.locator("#date-started-month")).toHaveCount(1);
        await expect(page.locator("#date-started-year")).toHaveCount(1);
    });

    test("Checks additional information on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-details")).not.toHaveAttribute("open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await expect(page.locator(".govuk-details")).toHaveAttribute("open");
    });

    test("shows the month and year missing error message when a user doesnt type a date on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Test Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
    });

    test("shows the month missing error message when a user doesnt type a month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.fill('#date-started-year', '2024');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Month Banner Link Text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Missing Month Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).not.toHaveClass(/govuk-input--error/);
    });

    test("shows the year missing error message when a user doesnt type a month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.fill('#date-started-month', '10');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Year Banner Link Text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Missing Year Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).not.toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
    });

    test.describe("When the month selected on the when-was-the-qualification-started page", () => {
        ['0', '-1', '13', '99'].forEach((value) => {
            test(`is ${value} then it shows the month out of bounds error message`, async ({page}) => {
                await page.goto("/questions/when-was-the-qualification-started");

                await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
                await expect(page.locator("#date-error")).toHaveCount(0);
                await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
                await page.fill('#date-started-month', value);
                await page.fill('#date-started-year', '2024');
                await page.click("#question-submit");
                await checkUrl(page, "/questions/when-was-the-qualification-started");
                await expect(page.locator(".govuk-error-summary")).toBeVisible();
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link Text");
                await expect(page.locator('#date-error')).toHaveCount(1);
                await checkText(page, '#date-error', "Month Out Of Bounds Error Message");
                await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
                await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
                await expect(page.locator("#date-started-year")).not.toHaveClass(/govuk-input--error/);
            });
        });
    });

    test.describe("When the year selected on the when-was-the-qualification-started page", () => {
        ['0', '1899', '3000'].forEach((value) => {
            test(`is ${value} then it shows the incorrect year format error message`, async ({page}) => {
                await page.goto("/questions/when-was-the-qualification-started");

                await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
                await expect(page.locator("#date-error")).toHaveCount(0);
                await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
                await page.fill('#date-started-month', '1');
                await page.fill('#date-started-year', value);
                await page.click("#question-submit");
                await checkUrl(page, "/questions/when-was-the-qualification-started");
                await expect(page.locator(".govuk-error-summary")).toBeVisible();
                await checkText(page, ".govuk-error-summary__title", "There is a problem");
                await checkText(page, "#error-banner-link", "Year Out Of Bounds Error Link Text");
                await expect(page.locator('#date-error')).toHaveCount(1);
                await checkText(page, '#date-error', "Year Out Of Bounds Error Message");
                await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
                await expect(page.locator("#date-started-month")).not.toHaveClass(/govuk-input--error/);
                await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
            });
        });
    });

    test("shows the month out of bound error message and the year out of bounds error message when a user types an invalid month and year on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.fill('#date-started-month', '0');
        await page.fill('#date-started-year', '20');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link TextYear Out Of Bounds Error Link Text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Month Out Of Bounds Error MessageYear Out Of Bounds Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
    });

    test("shows the month out of bound error message and the year missing error message when a user types an invalid month and doesnt type a year on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.fill('#date-started-month', '0');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Month Out Of Bounds Error Link TextMissing Year Banner Link Text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Month Out Of Bounds Error MessageMissing Year Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
    });

    test("shows the month missing error message and the year out of bounds error message when a user doesnt type a year and types an invalid month on the when-was-the-qualification-started page", async ({page}) => {
        await page.goto("/questions/when-was-the-qualification-started");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#date-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.fill('#date-started-year', '20');
        await page.click("#question-submit");
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Missing Month Banner Link TextYear Out Of Bounds Error Link Text");
        await expect(page.locator('#date-error')).toHaveCount(1);
        await checkText(page, '#date-error', "Missing Month Error MessageYear Out Of Bounds Error Message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
        await expect(page.locator("#date-started-month")).toHaveClass(/govuk-input--error/);
        await expect(page.locator("#date-started-year")).toHaveClass(/govuk-input--error/);
    });

    test("Checks the content on what-level-is-the-qualification page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await checkText(page, "#question", "What level is the qualification?");
        await expect(page.locator('input[id="2"]')).toHaveCount(1);
        await expect(page.locator('input[id="3"]')).toHaveCount(1);
        await expect(page.locator('input[id="6"]')).toHaveCount(1);
        await expect(page.locator('label[for="6"]')).toHaveCount(1);
        await checkText(page, 'div[id="6_hint"]', "Some hint text");
        await expect(page.locator('input[id="7"]')).toHaveCount(1);
    });

    test("Checks additional information on the what-level-is-the-qualification page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await expect(page.locator('.govuk-details')).not.toHaveAttribute("open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await expect(page.locator('.govuk-details')).toHaveAttribute("open");
    });

    test("shows an error message when a user doesnt select an option on the what-level-is-the-qualification page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-level-is-the-qualification");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#option-error")).toHaveCount(0);
        await expect(page.locator(".govuk-form-group").nth(0)).not.toHaveClass(/govuk-form-group--error/);
        await page.click("#question-submit");
        await checkUrl(page, "/questions/what-level-is-the-qualification");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await expect(page.locator('#option-error')).toHaveCount(1);
        await checkText(page, '#option-error', "Test error message");
        await expect(page.locator(".govuk-form-group").nth(0)).toHaveClass(/govuk-form-group--error/);
    });

    test("Checks the content on what-is-the-awarding-organisation page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");
        
        await checkText(page, '#question', "Test Dropdown Question");
        await expect(page.locator("#awarding-organisation-select")).toHaveCount(1);
        await expect(page.locator("#awarding-organisation-not-in-list")).toHaveCount(1);
        await expect(page.locator('#question-submit')).toHaveCount(1);
    });

    test("Checks additional information on the what-is-the-awarding-organisation page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");

        await expect(page.locator('.govuk-details')).not.toHaveAttribute("open");
        await checkText(page, ".govuk-details__summary-text", "This is the additional information header");
        await checkText(page, ".govuk-details__text", "This is the additional information body");
        await page.click(".govuk-details__summary-text");
        await expect(page.locator('.govuk-details')).toHaveAttribute("open");
    });

    test("shows an error message when a user doesnt select an option from the dropdown list" +
        "and also does not check 'not in the list' on the what-is-the-awarding-organisation", async ({page, context}) => {
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
        await page.goto("/questions/what-is-the-awarding-organisation");

        await expect(page.locator(".govuk-error-summary")).toHaveCount(0);
        await expect(page.locator("#dropdown-error")).toHaveCount(0);
        await expect(page.locator("#awarding-organisation-select")).not.toHaveClass(/govuk-select--error/);
        await page.click("#question-submit");
        await checkUrl(page, "/questions/what-is-the-awarding-organisation");
        await expect(page.locator(".govuk-error-summary")).toBeVisible();
        await checkText(page, ".govuk-error-summary__title", "There is a problem");
        await checkText(page, "#error-banner-link", "Test error banner link text");
        await expect(page.locator('#dropdown-error')).toHaveCount(1);
        await checkText(page, '#dropdown-error', "Test Error Message");
        await expect(page.locator("#awarding-organisation-select")).toHaveClass(/govuk-select--error/);
    });
});