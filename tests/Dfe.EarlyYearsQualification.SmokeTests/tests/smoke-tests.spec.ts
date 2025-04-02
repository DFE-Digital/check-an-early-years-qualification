import {test, expect} from '@playwright/test';

test.describe("A spec used to smoke test the environment once a deployment has happened", () => {
    test("should return search results", async ({page, context}) => {

        // attempt to set cookie and navigate to start page
        await context.addCookies([
            {name: 'auth-secret', value: process.env.AUTH_SECRET, path: '/', domain: process.env.DOMAIN}
        ]);

        await page.goto("/");

        //if we end up navigated to the challenge page, then fill in the password and continue
        if (page.url().includes("challenge")) {
            await page.locator("#PasswordValue").fill(process.env.AUTH_SECRET);
            await page.locator("#question-submit").click();
            await page.waitForURL("/");
        }

        // home page
        await expect(page.locator("#start-now-button")).toBeVisible();
        await page.locator("#start-now-button").click();

        // where-was-the-qualification-awarded page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/questions/where-was-the-qualification-awarded");
        await page.locator("#england").click();
        await page.locator("#question-submit").click();

        // when-was-the-qualification-started-and-awarded page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/questions/when-was-the-qualification-started-and-awarded");
        await page.locator("#StartedQuestion\\.SelectedMonth").fill("7");
        await page.locator("#StartedQuestion\\.SelectedYear").fill("2015");
        await page.locator("#AwardedQuestion\\.SelectedMonth").fill("9");
        await page.locator("#AwardedQuestion\\.SelectedYear").fill("2019");
        await page.locator("#question-submit").click();

        // what-level-is-the-qualification page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/questions/what-level-is-the-qualification");
        await page.locator('input[id="0"]').click();
        await page.locator("#question-submit").click();

        // what-is-the-awarding-organisation page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/questions/what-is-the-awarding-organisation");
        await page.locator("#awarding-organisation-not-in-list").click();
        await page.locator("#question-submit").click();

        // check-your-answers page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/questions/check-your-answers");
        await page.locator("#cta-button").click();

        // qualifications page
        await page.waitForLoadState("domcontentloaded");
        expect(page.url()).toContain("/qualifications");
        // If this shows then no qualifications are getting returned indicating possible issue
        await expect(page.locator("#no-result-content")).not.toBeVisible();
    });
});
