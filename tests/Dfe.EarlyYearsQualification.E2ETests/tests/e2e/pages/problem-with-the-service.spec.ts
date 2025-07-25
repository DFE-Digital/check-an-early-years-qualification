﻿import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    hasAttribute,
    hasCount,
    isVisible,
    doesNotExist
} from '../../_shared/playwrightWrapper';

test.describe('A spec used to test the not found page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the page contains the relevant components", async ({page}) => {
        await page.goto("/error");

        await checkText(page, "#problem-with-service-heading", "Sorry, there is a problem with the service");
        await checkText(page, "#problem-with-service-body", "Please try again later. In the meantime, you can download the early years qualifications list (EYQL) spreadsheet. This document is regularly updated by DfE to add new or updated qualifications as they become approved. You should refresh the page before downloading the EYQL spreadsheet to be sure that you are using the latest version.");
        await hasAttribute(page, "#problem-with-service-link", "href", "https://www.gov.uk/government/publications/early-years-qualifications-achieved-in-england");

        await hasCount(page, "govuk-footer__inline-list", 0);
        await doesNotExist(page, ".govuk-phase-banner");
    });
});