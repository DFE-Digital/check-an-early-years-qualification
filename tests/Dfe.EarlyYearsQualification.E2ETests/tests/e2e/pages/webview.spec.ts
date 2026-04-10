import {test, expect} from '@playwright/test';
import {
    checkText,
    inputText,
    checkTextContains,
    checkUrl,
    goToStartPage,
    attributeContains,
    doesNotExist
} from '../../_shared/playwrightWrapper';

test.describe("A spec that tests the webview page", {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await goToStartPage(page, context);
        await page.goto("/early-years-qualification-list");
    });

    test("Checks the content on early-years-qualification-list page", async ({page}) => {
        await attributeContains(page, "#back-button", 'href', '/');
        await checkText(page, "#heading", "Early Years Qualification List");
        await checkTextContains(page, "#post-heading-content > p", "This list shows all the qualifications that are approved by the Department for Education as full and relevant");
        await checkText(page, "#qualification-webview-results > h2", "Showing all the qualifications");
        await checkText(page, ".filter__header > h2", "Filter");
        await checkText(page, ".filter__selected-filters h2.govuk-heading-m", "Selected filters");
        await checkText(page, ".filter__selected-filters h2.govuk-heading-m ~ p", "No filters selected.");
        await checkText(page, "#apply-filter-form label[for='SearchTermFilter']", "Keywords");
        await checkText(page, "#apply-filter-form div:nth-child(2) legend", "Qualification start date");
        await checkText(page, "input[value='Pre-September 2014'] ~ label", "Before September 2014");
        await checkText(page, "input[value='Post-September 2014'] ~ label","On or after September 2014");
        await checkText(page, "input[value='Post-September 2024'] ~ label", "On or after September 2024");
        await checkText(page, "#apply-filter-form div:nth-child(3) legend", "Qualification level");
        await checkText(page, "input[value='2'] ~ label", "Level 2");
        await checkText(page, "input[value='3'] ~ label", "Level 3");
        await checkText(page, "input[value='4'] ~ label", "Level 4");
        await checkText(page, "input[value='5'] ~ label", "Level 5");
        await checkText(page, "input[value='6'] ~ label", "Level 6");
        await checkText(page, "input[value='7'] ~ label", "Level 7");
        await checkText(page, "#apply-filter-form button", "Apply filters");
    });

     test("Keyword filter returns expected qualification", async ({page}) => {
        await inputText(page, "#SearchTermFilter", "Education and Childcare (Specialism - Early Years Educator)");
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkTextContains(page, "button[value^='search-term']", "Education and Childcare (Specialism - Early Years Educator)");
        await checkText(page, "#qualification-webview-results > h2", "1 qualification found");
        await checkText(page, "#qualification-webview-results .govuk-summary-card h2", "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)");
    }); 

    test("Filter Pre-September 2014 is selected, qualifications started before Sept 2014 are returned", async ({page}) => {
        await page.locator("input[value='Pre-September 2014']").click();
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkTextContains(page, "button[value^='start-date']", "Before September 2014");

        const rows = page.locator('.govuk-summary-card .govuk-summary-card__content > dl > div:nth-child(3) dd');
        const allRows = await rows.all();
        for (const row of allRows) {
            let content = await row.innerText();

            if (content != "-") {
                let date = new Date(content);
                let year = date.getFullYear();
                expect(year).toBeLessThanOrEqual(2014);
                if (year == 2014) {
                    expect(date.getMonth()).toBeLessThanOrEqual(8);
                }
            }
        }
    });

    [
        {
            yearStarted: 'Post-September 2014',
            yearToAssert: 2014,
            monthToAssert: 8,
            expectedLabel: 'On or after September 2014',
        },
        {
            yearStarted: 'Post-September 2024',
            yearToAssert: 2024,
            monthToAssert: 8,
            expectedLabel: 'On or after September 2024',
        },
    ].forEach((scenario) => {
        test(`Check qualifications meet criteria based on start date filter ${scenario.yearStarted}`, async ({ page }) => {
            await page.locator(`input[value='${scenario.yearStarted}']`).click();
            await page.locator("#apply-filter-form button").click();
            await checkUrl(page, "/early-years-qualification-list");
            await checkTextContains(page, "button[value^='start-date']", scenario.expectedLabel);

            const rows = page.locator('.govuk-summary-card .govuk-summary-card__content > dl > div:nth-child(3) dd');
            const allRows = await rows.all();
            for (const row of allRows) {
                let content = await row.innerText();

                if (content != "-") {
                    let date = new Date(content);
                    let year = date.getFullYear();
                    expect(year).toBeGreaterThanOrEqual(scenario.yearToAssert);
                    if (year == scenario.yearToAssert) {
                        expect(date.getMonth()).toBeGreaterThanOrEqual(scenario.monthToAssert);
                    }
                }
            }
        });
    });

    [2,3,4,5,6,7].forEach((level) => {
        test(`Filter level ${level} is selected, qualifications that equals the level are returned`, async ({ page }) => {
            await page.locator(`[value='${level}']`).click();
            await page.locator("#apply-filter-form button").click();
            await checkUrl(page, "/early-years-qualification-list");
            await checkTextContains(page, "button[value^='qualification-level']", `Level ${level}`);

            const rows = page.locator('.govuk-summary-list div:nth-child(1) > dd.govuk-summary-list__value');
            const allRows = await rows.all();
            for (const row of allRows) {
                expect(await row.innerText()).toBe(`${level}`);
            }
        });
    });

    test(`All filters are removed when the remove filters link is selected`, async ({ page }) => {
        await inputText(page, "#SearchTermFilter", "Qualification");
        await page.locator(`input[value='Post-September 2014']`).click();
        await page.locator(`[value='5']`).click();
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkTextContains(page, "button[value^='search-term']", "Qualification");
        await checkTextContains(page, "button[value^='start-date']", "On or after September 2014");
        await checkTextContains(page, "button[value^='qualification-level']", `5`);
        await checkText(page, "#qualification-webview-results > h2", "1 qualification found");
        await page.locator(`a[href='/clear-filters']`).click();
        await checkText(page, "#qualification-webview-results > h2", "Showing all the qualifications");
        await doesNotExist(page, `button[value^='search-term']`);
        await doesNotExist(page, `button[value^='start-date']`);
        await doesNotExist(page, `button[value^='qualification-level']`);
    });

    test(`No qualifications found based on filters`, async ({ page }) => {
        await inputText(page, "#SearchTermFilter", "This search will return no results");
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkText(page, "#qualification-webview-results > h2", "0 qualifications found");
        await checkText(page, "#qualification-webview-results > p:nth-child(2)", "No qualifications match the filters you selected.");
    });

    test(`Individual filters are removed when the active filter is selected`, async ({ page }) => {
        await inputText(page, "#SearchTermFilter", "Qualification");
        await page.locator(`input[value='Post-September 2014']`).click();
        await page.locator(`[value='5']`).click();
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkTextContains(page, "button[value^='search-term']", "Qualification");
        await checkTextContains(page, "button[value^='start-date']", "On or after September 2014");
        await checkTextContains(page, "button[value^='qualification-level']", `5`);
        await page.locator("button[value^='search-term']").click();
        await doesNotExist(page, `button[value^='search-term']`);
        await checkTextContains(page, "button[value^='start-date']", "On or after September 2014");
        await checkTextContains(page, "button[value^='qualification-level']", `5`);
        await page.locator("button[value^='start-date']").click();
        await doesNotExist(page, `button[value^='start-date']`);
        await checkTextContains(page, "button[value^='qualification-level']", `5`);
        await page.locator("button[value^='qualification-level']").click();
        await doesNotExist(page, `button[value^='qualification-level']`);
    });
});