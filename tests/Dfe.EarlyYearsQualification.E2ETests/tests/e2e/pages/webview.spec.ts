import {test, expect} from '@playwright/test';
import {
    checkText,
    inputText,
    checkTextContains,
    checkUrl,
    goToStartPage
} from '../../_shared/playwrightWrapper';

const postyearData = [
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
];


test.describe("A spec that tests the webview page", {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await goToStartPage(page, context);
    });

    test("Checks the content on early-years-qualification-list page", async ({page}) => {
        await page.goto("/early-years-qualification-list");

        await hasAttribute(page, "#back-button", 'href');
        await attributeContains(page, "#back-button", 'href', '/');
        await checkText(page, ".govuk-heading-xl", "Early Years Qualification List");

        await checkText(page, ".govuk-body", "This list shows all the qualifications that are approved by the Department for Education as full and relevant. When you find a qualification on the list, make sure that:");
        await checkText(page, ".govuk-body", "If any of the qualification details do not match, the qualification is not approved as full and relevant.");
        
        await checkText(page, "Early Years Qualification List");
        await checkText(page, "Label[for=QualificationStartDateFilter-1]", "Before September 2014");
        await checkText(page, "Label[for=QualificationStartDateFilter-2]","On or after September 2014");
        await checkText(page, "Label[for=QualificationStartDateFilter-3]", "On or after September 2024");

        await checkText(page, "label[for=QualificationLevelFilter-1]", "Level 2");
        await checkText(page, "label[for=QualificationLevelFilter-2]", "Level 3");
        await checkText(page, "label[for=QualificationLevelFilter-3]", "Level 4");
        await checkText(page, "label[for=QualificationLevelFilter-4]", "Level 5");
        await checkText(page, "label[for=QualificationLevelFilter-5]", "Level 6");
        await checkText(page, "label[for=QualificationLevelFilter-6]", "Level 7");
        
        await checkText(page, "Label[for='Before September 2014']", "Before September 2014");
        await checkText(page, "Label[for='On or after September 2014']", "On or after September 2014");
        await checkText(page, "Label[for='On or after September 2024']", "On or after September 2024");
        await checkText(page, "#apply-filter-form button", "Apply filters");
    });

    /* test("Enters Keyword Returns Expeected Qualifications", async ({page}) => {
        await page.goto("/early-years-qualification-list");

        await inputText(page, "#SearchTermFilter", "qualification 302");
        await page.locator("#apply-filter-form button").click();
        await checkUrl(page, "/early-years-qualification-list");
        await checkTextContains(page, "button[value^='search-term']", "qualification 302");
        
        
        
    }); */

    test("Qualifications Started before 2014 and Returned", async ({page}) => {
        await page.goto("/early-years-qualification-list");

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
                let month = date.getMonth();
                let year = date.getFullYear();
                expect(year).toBeLessThanOrEqual(2014);
                if (year == 2014) {
                    expect(month).toBeLessThanOrEqual(8);
                }
            }
        }
        
        
        
    });

    postyearData.forEach((scenario) => {
        test(`Check qualifications meet criteria based on start date filter ${scenario.yearStarted}`, async ({ page }) => {
            await page.goto("/early-years-qualification-list");

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
                    let month = date.getMonth();
                    let year = date.getFullYear();
                    expect(year).toBeGreaterThanOrEqual(scenario.yearToAssert);
                    if (year == scenario.yearToAssert) {
                        expect(month).toBeGreaterThanOrEqual(scenario.monthToAssert);
                    }
                }
            }

        });
    });
    
});