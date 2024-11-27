import { test, expect } from '@playwright/test';

test.beforeEach(async ({ page, context }) => {
    await context.addCookies([
        { name: 'auth-secret', value: 'CX', path: '/', domain: 'localhost' }
    ]);
});

// Mock details found in Dfe.EarlyYearsQualification.Mock.Content.MockContentfulService. 

test.describe("A spec that tests question pages", () => {
    test("Checks the content on where-was-the-qualification-awarded page", async ({ page }) => {
        await page.goto('/questions/where-was-the-qualification-awarded');

        await expect(page.locator("#question")).toHaveText("Where was the qualification awarded?");
        await expect(page.locator("#england")).toBeVisible();
        await expect(page.locator("#scotland")).toBeVisible();
        await expect(page.locator("#wales")).toBeVisible();
        await expect(page.locator("#northern-ireland")).toBeVisible();
        await expect(page.locator(".govuk-radios__divider")).toHaveText("or");
        await expect(page.locator("#outside-uk")).toBeVisible();
    });

    test("Checks additional information on the where-was-the-qualification-awarded page", async ({ page }) => {
        await page.goto('/questions/where-was-the-qualification-awarded');

        await expect(page.locator(".govuk-details")).not.toHaveAttribute("open");
        await expect(page.locator(".govuk-details__summary-text")).toHaveText("This is the additional information header");
        await expect(page.locator(".govuk-details__text")).toHaveText("This is the additional information body");

        await page.locator(".govuk-details__summary-text").click();
        await expect(page.locator(".govuk-details")).toHaveAttribute("open");
    });

    test("Shows an error message when a user doesnt select an option on the where-was-the-qualification-awarded page", async ({ page }) => {
        await page.goto('/questions/where-was-the-qualification-awarded');

        await expect(page.locator(".govuk-error-summary")).not.toBeVisible();
        await expect(page.locator("#option-error")).not.toBeVisible();
        await expect(page.getByTestId("main-form-group")).not.toHaveClass("govuk-form-group--error");
        
        await page.locator("#question-submit").click();
        await page.waitForURL("/questions/where-was-the-qualification-awarded");

        await expect(page.getByTestId("main-form-group")).toBeVisible();
        await expect(page.getByTestId("error-title")).toBeVisible();
        await expect(page.getByTestId("error-title")).toContainText("There is a problem");
        await expect(page.locator("#error-banner-link")).toContainText("Test error banner link text");

        await expect(page.locator("#option-error")).toBeVisible();
        await expect(page.locator("#option-error")).toContainText("Test error message");
        await expect(page.getByTestId("main-form-group")).toHaveClass(/govuk-form-group--error/);
    });
});