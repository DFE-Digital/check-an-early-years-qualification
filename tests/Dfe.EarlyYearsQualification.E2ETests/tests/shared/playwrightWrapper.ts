import {BrowserContext, Page, expect, APIResponse} from "@playwright/test";

export const cookiePreferencesCookieName = "cookies_preferences_set";
export const journeyCookieName = 'user_journey';

export async function authorise(context: BrowserContext) {
    await setCookie(context, process.env.AUTH_SECRET, 'auth-secret');
}

export async function startJourney(page: Page, context: BrowserContext) {
    await authorise(context);
    await page.goto("/", {waitUntil: 'domcontentloaded'});
    await page.waitForFunction(() => document.title === "Start - Check an Early Years qualification")
    await expect(page.locator("#start-now-button")).toBeVisible();
    await page.locator("#start-now-button").click();
}

export async function checkUrl(page: Page, expectedUrl: string) {
    await page.waitForLoadState("domcontentloaded");
    expect(page.url()).toContain(expectedUrl);
}

export async function checkText(page: Page, locator: string, expectedText: string, nth: number = null) {
    await page.waitForLoadState("domcontentloaded");
    var element = page.locator(locator);
    if (nth != null) {
        element = element.nth(nth);
    }
    await expect(element).toHaveText(expectedText);
}

export async function checkTextContains(page: Page, locator: string, expectedText: string) {
    await page.waitForLoadState("domcontentloaded");
    var element = page.locator(locator);
    await expect(element).toContainText(expectedText);
}

export async function inputText(page: Page, locator: string, text: string) {
    await page.locator(locator).fill(text);
}

export async function checkError(page: Page, locator: string, expectedText: string) {
    await checkText(page, locator + " > span", "Error:");
    await checkText(page, locator, `Error:${expectedText}`);
}

export async function checkDisclaimer(page: Page, expectedText: string) {
    await checkText(page, ".govuk-warning-text__icon", "!");
    await checkText(page, ".govuk-warning-text__text" + " > span", "Warning:");
    await checkText(page, ".govuk-warning-text__text", `Warning:${expectedText}`);
}

export async function checkValue(page: Page, locator: string, expectedValue: any) {
    await page.waitForLoadState("domcontentloaded");
    await expect(page.locator(locator)).toHaveValue(expectedValue);
}

export async function checkEmptyValue(page: Page, locator: string) {
    await page.waitForLoadState("domcontentloaded");
    await expect(page.locator(locator).first()).toBeEmpty();
}

export async function clickBackButton(page: Page) {
    await page.locator("#back-button").click();
}

export async function checkCookieValue(context: BrowserContext, value: string) {
    var cookies = await context.cookies();
    var cookie = cookies.find((c) => c.name === cookiePreferencesCookieName);

    expect(cookie.value).toBe(value);
}

export async function checkJourneyCookieValue(context: BrowserContext, value: string) {
    var cookies = await context.cookies();
    var cookie = cookies.find((c) => c.name === journeyCookieName);

    expect(cookie.value).toBe(value);
}

export async function setCookie(context: BrowserContext, value: string, cookieName: string) {
    await context.addCookies([
        {
            name: cookieName,
            value: value,
            path: '/',
            domain: process.env.DOMAIN
        }
    ]);
}

interface JourneyStateParams {
    context: BrowserContext,
    location: string,
    startDate: number[],
    awardDate: number[],
    level: number,
    organisation: string,
    organisationNotOnList: boolean,
    searchCriteria: string,
    additionalQuestions: string[][],
    selectedFromList: boolean
};

function getQualificationId(level: number) {
    switch (level) {
        case 2:
            return 'eyq-241';
        case 3:
            return 'eyq-240';
        case 4:
            return 'eyq-105';
        case 5:
            return 'eyq-107';
        case 6:
            return 'eyq-108';
        case 7:
            return 'eyq-111';
    }
}

export async function goToDetailsPageOfQualification({
                                                         context,
                                                         startDate,
                                                         awardDate,
                                                         level,
                                                         organisation,
                                                         organisationNotOnList,
                                                         searchCriteria,
                                                         additionalQuestions,
                                                         selectedFromList,
                                                         location = '',
                                                     }: JourneyStateParams, page: Page) {
    var qualificationId = getQualificationId(level);

    var additionQuestionsValue = "";
    if (additionalQuestions != null) {
        for (let i = 0; i < additionalQuestions.length; i++) {
            var additionalQuestion = additionalQuestions[i];
            additionQuestionsValue = additionQuestionsValue + additionalQuestion[0] + '%22%3A%22' + additionalQuestion[1];
            if (i != additionalQuestions.length - 1) {
                additionQuestionsValue += '%22%2C%22';
            }
        }
    }
    var startValue = startDate == null ? '' : `${startDate[0] ?? ''}%2F${startDate[1] ?? ''}`;
    var awardValue = awardDate == null ? '' : `${awardDate[0] ?? ''}%2F${awardDate[1] ?? ''}`;

    var additionQuestionsValue = '';
    if (additionalQuestions != null) {
        for (let i = 0; i < additionalQuestions.length; i++) {
            var additionalQuestion = additionalQuestions[i];
            additionQuestionsValue = additionQuestionsValue + additionalQuestion[0] + '%22%3A%22' + additionalQuestion[1];
            if (i != additionalQuestions.length - 1) {
                additionQuestionsValue += '%22%2C%22';
            }
        }
        additionQuestionsValue = `%22${additionQuestionsValue}%22`;
    }

    var cookie = `%7B%22WhereWasQualificationAwarded%22%3A%22${location ?? ''}%22%2C%22WhenWasQualificationStarted%22%3A%22${startValue}%22%2C%22WhenWasQualificationAwarded%22%3A%22${awardValue}%22%2C%22LevelOfQualification%22%3A%22${level ?? ''}%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22${organisation ?? ''}%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3A${organisationNotOnList ?? false}%2C%22SearchCriteria%22%3A%22${searchCriteria ?? ''}%22%2C%22AdditionalQuestionsAnswers%22%3A%7B${additionQuestionsValue}%7D%2C%22QualificationWasSelectedFromList%22%3A${(selectedFromList ?? false) ? 1 : 0}%7D`;
    await setCookie(context, cookie, journeyCookieName);

    await page.goto(`/qualifications/qualification-details/${qualificationId}`);
}

export function checkHeaderValue(response: APIResponse, headerName: string, headerValue: string) {
    expect(response.headers()[headerName]).toBe(headerValue);
}

export function checkHeaderExists(response: APIResponse, headerName: string) {
    expect(response.headers()[headerName]).toBeUndefined();
}

export async function exists(page: Page, locator: string) {
    await hasCount(page, locator, 1);
}

export async function doesNotExist(page: Page, locator: string) {
    await hasCount(page, locator, 0);
}

export async function hasCount(page: Page, locator: string, count: number) {
    await expect(page.locator(locator)).toHaveCount(count);
}

export async function isVisible(page: Page, locator: string) {
    await expect(page.locator(locator)).toBeVisible();
}

export async function isNotVisible(page: Page, locator: string) {
    await expect(page.locator(locator)).not.toBeVisible();
}

export async function hasAttribute(page: Page, locator: string, attribute: string, value?: string) {
    await expect(page.locator(locator)).toHaveAttribute(attribute, value);
}

export async function doesNotHaveAttribute(page: Page, locator: string, attribute: string) {
    await expect(page.locator(locator)).not.toHaveAttribute(attribute);
}

export async function attributeContains(page: Page, locator: string, attribute: string, value: string) {
    expect(await page.locator(locator).getAttribute(attribute)).toContain(value);
}

export async function hasClass(page: Page, locator: string, expectedClass: string | RegExp, nth: number = null) {
    var element = page.locator(locator);
    if (nth != null) {
        element = element.nth(nth);
    }
    await expect(element).toHaveClass(expectedClass);
}

export async function doesNotHaveClass(page: Page, locator: string, expectedClass: string | RegExp, nth: number = null) {
    var element = page.locator(locator);
    if (nth != null) {
        element = element.nth(nth);
    }
    await expect(element).not.toHaveClass(expectedClass);
}

export async function whereWasTheQualificationAwarded(page: Page, location: string) {
    checkUrl(page, "/questions/where-was-the-qualification-awarded");
    await page.locator(location).click();
    await page.locator("#question-submit").click();
}

export async function whenWasQualificationStarted(page: Page, startedMonth: string, startedYear: string, awardedMonth: string, awardedYear: string) {
    checkUrl(page, '/questions/when-was-the-qualification-started-and-awarded');
    await page.locator("#StartedQuestion\\.SelectedMonth").fill(startedMonth);
    await page.locator("#StartedQuestion\\.SelectedYear").fill(startedYear);
    await page.locator("#AwardedQuestion\\.SelectedMonth").fill(awardedMonth);
    await page.locator("#AwardedQuestion\\.SelectedYear").fill(awardedYear);
    await page.locator("#question-submit").click();
}

export async function whatLevelIsTheQualification(page: Page, level: number) {
    // what-level-is-the-qualification page - valid level moves us on
    checkUrl(page, "/questions/what-level-is-the-qualification");
    await page.locator('input[id="' + level + '"]').click();
    await page.locator("#question-submit").click();
}

export async function whatIsTheAwardingOrganisation(page: Page, dropdownIndex: number) {
    // what-is-the-awarding-organisation page - valid awarding organisation moves us on
    checkUrl(page, "/questions/what-is-the-awarding-organisation");
    await page.locator("#awarding-organisation-select").selectOption({index: dropdownIndex});
    await page.locator("#question-submit").click();
}

export async function checkYourAnswersPage(page: Page) {
    checkUrl(page, "/questions/check-your-answers");
    await page.locator("#cta-button").click();
}

export async function selectQualification(page: Page, qualificationId: string) {
    // qualifications page - click a qualification in the list to move us on
    checkUrl(page, "/select-a-qualification-to-check");
    await page.locator("a[href=\"/confirm-qualification/" + qualificationId + "\"]").click();
    checkUrl(page, "/confirm-qualification/" + qualificationId);
}

export async function confirmQualificiation(page: Page, answer: string) {
    // confirm qualification page
    await page.locator(answer).click();
    await page.locator('#confirm-qualification-button').click();

}

export async function processAdditionalRequirement(page: Page, qualificationId: string, additionalRequirementIndex: number, answer: string) {
    // check additional questions first page
    checkUrl(page, "/qualifications/check-additional-questions/" + qualificationId + "/" + additionalRequirementIndex);
    await page.locator(answer).click();
    await page.locator("#additional-requirement-button").click();

}

export async function confirmAdditonalRequirementsAnswers(page: Page, qualificationId: string) {
    checkUrl(page, "/qualifications/check-additional-questions/" + qualificationId + "/confirm-answers");
    await page.locator("#confirm-answers").click();
}

export async function checkDetailsPage(page: Page, qualificationId: string) {
    await checkUrl(page, "/qualifications/qualification-details/" + qualificationId);
}

export async function refineQualificationSearch(page: Page, searchTerm: string) {
    await page.locator("#refineSearch").fill(searchTerm);
    checkValue(page, "#refineSearch", searchTerm);
    await page.locator("#refineSearchButton").click();
}

export enum RatioStatus {
    Approved,
    NotApproved,
    FurtherActionRequired,
    PossibleRouteAvailable
}

function getStatusValues(status: RatioStatus) {
    switch (status) {
        case RatioStatus.Approved:
            return {
                Text: "Approved",
                Colour: /govuk-tag--green/
            };
        case RatioStatus.NotApproved:
            return {
                Text: "Not approved",
                Colour: /govuk-tag--red/
            };
        case RatioStatus.FurtherActionRequired:
            return {
                Text: "Further action required",
                Colour: /govuk-tag--grey/
            };
        case RatioStatus.PossibleRouteAvailable:
            return {
                Text: "Possible route available",
                Colour: /govuk-tag--blue/
            };
    }
}

export async function checkLevelRatioDetails(page: Page, nth: number, ratioHeading: string, status: RatioStatus, {
    summaryText = null,
    detailText = null
}: { summaryText?: string, detailText?: string }) {
    var statusValues = getStatusValues(status);

    await checkText(page, ".ratio-heading", ratioHeading, nth);
    await checkText(page, ".ratio-status-tag", statusValues.Text, nth);
    await hasClass(page, ".ratio-status-tag", statusValues.Colour, nth);

    var ratioId = ratioHeading.replace(/\s/g, "");
    if (summaryText == null && detailText == null) {
        await doesNotExist(page, `#ratio-${ratioId}-additional-info`);
    } else if (summaryText == null && detailText != null) {
        await checkText(page, `#ratio-${ratioId}-additional-info`, detailText);
    } else if (summaryText != null && detailText != null) {
        await checkText(page, `#ratio-${ratioId}-additional-info .govuk-details__summary-text`, summaryText);
        await checkText(page, `#ratio-${ratioId}-additional-info .govuk-details__text`, detailText);
    } else {
        throw new Error("Cannot determine additional info checks");
    }
}