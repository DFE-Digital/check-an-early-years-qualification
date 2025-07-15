import {test} from '@playwright/test';
import {authorise, checkText, isVisible} from '../../_shared/playwrightWrapper';

test.describe('A spec that tests the footer is showing correctly', {tag: "@e2e"}, () => {

    test.beforeEach(async ({context}) => {
        await authorise(context);
    });

    test('Checks that the footer section components are visable', async ({page}) => {
        await page.goto("/");

        await isVisible(page, "#left-hand-side-section");
        await isVisible(page, "#right-hand-side-section");

        await checkText(page, "#left-hand-side-section > h2", "Left section");
        await checkText(page, "#left-hand-side-section > ul > li", "This is the left hand side footer content");

        await checkText(page, "#right-hand-side-section > h2", "Right section");
        await checkText(page, "#right-hand-side-section > ul > li", "This is the right hand side footer content");
    })

    test('Checks that the footer contains the navigation links', async ({page}) => {
        await page.goto("/");

        await isVisible(page, ".govuk-footer__inline-list");

        await checkText(page, ".govuk-footer__inline-list-item", "Privacy notice", 0);
        await checkText(page, ".govuk-footer__inline-list-item", "Accessibility statement", 1);
    })
});