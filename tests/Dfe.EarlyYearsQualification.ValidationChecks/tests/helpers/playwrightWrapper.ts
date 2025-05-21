import { BrowserContext, Page, expect } from "@playwright/test";

export const cookiePreferencesCookieName = "cookies_preferences_set";
export const journeyCookieName = 'user_journey';

export async function authorise(context: BrowserContext) {
    await setCookie(context, process.env.AUTH_SECRET, 'auth-secret');
}

export async function startJourney(page: Page, context: BrowserContext) {
    await authorise(context);

    await page.goto("/", { waitUntil: 'domcontentloaded' });

    //if we end up navigated to the challenge page, then fill in the password and continue
    if (page.url().includes("challenge")) {
        await page.locator("#PasswordValue").fill(process.env.AUTH_SECRET);
        await page.locator("#question-submit").click();
        await page.waitForURL("/");
    }

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
    let element = page.locator(locator);
    if (nth != null) {
        element = element.nth(nth);
    }
    await expect(element).toHaveText(expectedText);
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

export async function whereWasTheQualificationAwarded(page: Page, location: string) {
    await checkUrl(page, "/questions/where-was-the-qualification-awarded");
    await page.locator(location).click();
    await page.locator("#question-submit").click();
}

export async function whenWasQualificationStarted(page: Page, startedMonth: string, startedYear: string, awardedMonth: string, awardedYear: string) {
    await checkUrl(page, '/questions/when-was-the-qualification-started-and-awarded');
    await page.locator("#StartedQuestion\\.SelectedMonth").fill(startedMonth);
    await page.locator("#StartedQuestion\\.SelectedYear").fill(startedYear);
    await page.locator("#AwardedQuestion\\.SelectedMonth").fill(awardedMonth);
    await page.locator("#AwardedQuestion\\.SelectedYear").fill(awardedYear);
    await page.locator("#question-submit").click();
}

export async function whatLevelIsTheQualification(page: Page, level: number) {
    // what-level-is-the-qualification page - valid level moves us on
    await checkUrl(page, "/questions/what-level-is-the-qualification");
    await page.locator('input[id="' + level + '"]').click();
    await page.locator("#question-submit").click();
}

export async function whatIsTheAwardingOrganisation(page: Page, value: string) {
    // what-is-the-awarding-organisation page - valid awarding organisation moves us on
    await checkUrl(page, "/questions/what-is-the-awarding-organisation");
    await page.locator("#awarding-organisation-select").selectOption({ value: value });
    await page.locator("#question-submit").click();
}

export async function selectNotOnTheListAsTheAwardingOrganisation(page: Page) {
    await checkUrl(page, "/questions/what-is-the-awarding-organisation");
    await page.locator("#awarding-organisation-not-in-list").click();
    await page.locator("#question-submit").click();
}

export async function checkYourAnswersPage(page: Page) {
    await checkUrl(page, "/questions/check-your-answers");
    await page.locator("#cta-button").click();
}

export async function selectQualification(page: Page, qualificationId: string) {
    // qualifications page - click a qualification in the list to move us on
    await checkUrl(page, "/select-a-qualification-to-check");
    await page.locator("a[href=\"/confirm-qualification/" + qualificationId + "\"]").click();
    await checkUrl(page, "/confirm-qualification/" + qualificationId);
}

export async function checkNumberOfMatchingQualifications(page: Page, numberOfExpectedQualifications: number) {
    // qualifications page - click a qualification in the list to move us on
    await checkUrl(page, "/select-a-qualification-to-check");
    await checkText(page, "#found-heading", `We found ${numberOfExpectedQualifications} matching qualifications`);
}

export async function confirmQualificiation(page: Page, answer: string) {
    // confirm qualification page
    await page.locator(answer).click();
    await page.locator('#confirm-qualification-button').click();

}

export async function processAdditionalRequirement(page: Page, qualificationId: string, additionalRequirementIndex: number, answer: string) {
    // check additional questions first page
    await checkUrl(page, "/qualifications/check-additional-questions/" + qualificationId + "/" + additionalRequirementIndex);
    await page.locator(answer).click();
    await page.locator("#additional-requirement-button").click();

}

export async function confirmAdditonalRequirementsAnswers(page: Page, qualificationId: string) {
    await checkUrl(page, "/qualifications/check-additional-questions/" + qualificationId + "/confirm-answers");
    await page.locator("#confirm-answers").click();
}

export async function checkDetailsPage(page: Page, qualificationId: string) {
    await checkUrl(page, "/qualifications/qualification-details/" + qualificationId);
}