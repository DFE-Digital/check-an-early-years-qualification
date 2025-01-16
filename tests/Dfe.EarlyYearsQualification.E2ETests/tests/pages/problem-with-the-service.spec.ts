import {test, expect} from '@playwright/test';
import {startJourney, checkText} from '../shared/playwrightWrapper';
test.describe('A spec used to test the not found page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the page contains the relevant components", async ({page}) => {
        await page.goto("/error");

        await checkText(page, "#problem-with-service-heading", "Sorry, there is a problem with the service");
        await checkText(page, "#problem-with-service-body", "Please try again later.");
        await checkText(page, "#problem-with-service-body", "In the meantime, you can download the early years qualifications list (EYQL) spreadsheet. This document is regularly updated by DfE to add new or updated qualifications as they become approved. You should refresh the page before downloading the EYQL spreadsheet to be sure that you are using the latest version.");

        await expect(page.locator("#problem-with-service-link")).toHaveAttribute("href", "https://www.gov.uk/government/publications/early-years-qualifications-achieved-in-england");
    });
});