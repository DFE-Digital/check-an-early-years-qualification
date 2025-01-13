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
    //expect(await page.title()).toBe("Start - Check an Early Years qualification");
    await expect(page.locator("#start-now-button")).toBeVisible();
    await page.locator("#start-now-button").click();
}

export async function checkUrl(page: Page, expectedUrl: string) {
    await page.waitForLoadState("domcontentloaded");
    expect(page.url()).toContain(expectedUrl);
}

export async function checkText(page: Page, locator: string, expectedText: string, nth: number = null) {
    await page.waitForLoadState("domcontentloaded");
    await page.waitForSelector(locator);
    var element = page.locator(locator);
    if (nth != null) {
        element = element.nth(nth);
    }
    await expect(element).toContainText(expectedText);
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

export function checkHeaderValue(response: APIResponse, headerName: string, headerValue: string) {
    expect(response.headers()[headerName]).toBe(headerValue);
}

export function checkHeaderExists(response: APIResponse, headerName: string, shouldExist: boolean) {
    expect(response.headers()[headerName]).toBeUndefined();
}

export async function whereWasTheQualificationAwarded(page: Page, location: string) {
    checkUrl(page, "/questions/where-was-the-qualification-awarded");
    await page.locator(location).click();
    await page.locator("#question-submit").click();
}

export async function whenWasQualificationStarted(page: Page, month: string, year: string) {
    checkUrl(page, '/questions/when-was-the-qualification-started');
    await page.locator("#date-started-month").fill(month);
    await page.locator("#date-started-year").fill(year);
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

export async function selectQualification(page: Page, qualificationId: string) {
    // qualifications page - click a qualification in the list to move us on
    checkUrl(page, "/qualifications");
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
