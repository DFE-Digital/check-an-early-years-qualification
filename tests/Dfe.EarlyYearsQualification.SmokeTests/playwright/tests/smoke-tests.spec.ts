import { test, expect } from '@playwright/test';

test.describe("A spec used to smoke test the environment once a deployment has happened", () => {
  test.beforeEach(async ({ page , context}) => {
    await context.addCookies([
      { name: 'auth-secret', value: process.env.AUTH_SECRET, path: '/', domain: 'localhost' }
    ]);
    await page.goto(process.env.WEBAPP_URL + "/");
    await expect(page.getByTestId("start-button")).toBeVisible();
  });
  
  test("should return search results", async ({ page }) => {
    // home page
    await page.getByTestId("start-button").click();
    await page.waitForURL("/questions/where-was-the-qualification-awarded");

    // where-was-the-qualification-awarded page
    await page.locator("#england").click();
    await page.locator("#question-submit").click();
    await page.waitForURL("/questions/when-was-the-qualification-started");

    // when-was-the-qualification-started page
    await page.locator("#date-started-month").type("7");
    await page.locator("#date-started-year").type("2015");
    await page.locator("#question-submit").click();
    await page.waitForURL("/questions/what-level-is-the-qualification");

    // what-level-is-the-qualification page
    await page.locator("#0").click();
    await page.locator("#question-submit").click();
    await page.waitForURL("/questions/what-is-the-awarding-organisation");

    // what-is-the-awarding-organisation page
    await page.locator("#awarding-organisation-not-in-list").click();
    await page.locator("#question-submit").click();
    await page.waitForURL("/qualifications");

    // qualifications page
    // If this shows then no qualifications are getting returned indicating possible issue
    await expect(page.locator("#no-result-content")).not.toBeVisible();
  });
});
