import {test} from '@playwright/test';
import {
    goToStartPage
} from '../../_shared/playwrightWrapper';

test.describe("A spec that tests the webview page", {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await goToStartPage(page, context);
    });

    test("Checks the content on early-years-qualification-list page", async ({page}) => {
        await page.goto("/early-years-qualification-list");
    });
});