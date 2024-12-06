import { test, expect } from '@playwright/test';

test.describe("A spec used to test the main back button route through the journey", () => {
    // Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService.
    test("back buttons should all navigate to the appropriate pages in the main journey", async ({ page, context }) => {
        await context.addCookies([
            { name: 'auth-secret', value: process.env.AUTH_SECRET, path: '/', domain: process.env.WEBAPP_URL }
        ]);

        // home page
        await page.goto("/");
        await expect(page.locator("#start-now-button")).toHaveCount(1);
        await page.locator("#start-now-button").click();

        // where-was-the-qualification-awarded page - england selection moves us on
        expect(page.url()).toContain("/questions/where-was-the-qualification-awarded");
        await page.locator("#england").click();
        await page.locator("#question-submit").click();

        // when-was-the-qualification-started page - valid date moves us on
        expect(page.url()).toContain("/questions/when-was-the-qualification-started");
        await page.locator("#date-started-month").fill("6");
        await page.locator("#date-started-year").fill("2022");
        await page.locator("#question-submit").click();

        // what-level-is-the-qualification page - valid level moves us on
        expect(page.url()).toContain("/questions/what-level-is-the-qualification");
        await page.locator("#3").click();
        await page.locator("#question-submit").click();

        // what-is-the-awarding-organisation page - valid awarding organisation moves us on
        expect(page.url()).toContain("/questions/what-is-the-awarding-organisation");
        await page.locator("#awarding-organisation-select").selectOption("1");
        await page.locator("#question-submit").click();

        // qualifications page - click a qualification in the list to move us on
        expect(page.url()).toContain("/qualifications");
        await page.locator("a[href=\"/confirm-qualification/EYQ-240\"]").click();

        // check additional questions first page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/1");
        await page.locator("#yes").click();
        await page.locator("#additional-requirement-button").click();
        
        // check additional questions second page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/2");
        await page.locator("#no").click();
        await page.locator("#additional-requirement-button").click();

        // confirm answers page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/confirm-answers");
        await page.locator("#confirm-answers").click();
        
        // qualifications page
        expect(page.url()).toContain("/qualifications/qualification-details/EYQ-240");

        /// Time to go back through the journey!
        await page.locator("#back-button").click();

        // confirm answers page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/confirm-answers");
        await page.locator("#back-button").click();

        // answered additional questions, so back to second additional questions page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/2");
        await page.locator("#back-button").click();

        // answered additional questions, so back to first additional questions page
        expect(page.url()).toContain("/qualifications/check-additional-questions/EYQ-240/1");
        await page.locator("#back-button").click();

        // qualifications page
        expect(page.url()).toContain("/qualifications");
        await page.locator("#back-button").click();
        
        expect(page.url()).toContain("/questions/what-is-the-awarding-organisation");
        await page.locator("#back-button").click();
        
        expect(page.url()).toContain("/questions/what-level-is-the-qualification");
        await page.locator("#back-button").click();
        
        expect(page.url()).toContain("/questions/when-was-the-qualification-started");
        await page.locator("#back-button").click();

        expect(page.url()).toContain("/questions/where-was-the-qualification-awarded");
        await page.locator("#back-button").click();

        expect(page.url()).toBe(process.env.WEBAPP_URL + "/");
    });

    test.describe("back buttons should all navigate to the appropriate pages in the main journey", async () => {
        test.beforeEach(async ({page, context}) => {
            await context.addCookies([
                { name: 'auth-secret', value: process.env.AUTH_SECRET, path: '/', domain: process.env.WEBAPP_URL }
            ]);
        });
        
        test("the back button on the accessibility statement page navigates back to the home page", async ({ page, context }) => {
            await page.goto("/accessibility-statement");
            await page.locator("#back-button").click();
            expect(page.url()).toBe(process.env.WEBAPP_URL + "/");
        });

        test("the back button on the cookies preference page navigates back to the home page", async ({ page, context }) => {
            await page.goto("/cookies");
            await page.locator("#back-button").click();
            expect(page.url()).toBe(process.env.WEBAPP_URL + "/");
        });
    });
});
