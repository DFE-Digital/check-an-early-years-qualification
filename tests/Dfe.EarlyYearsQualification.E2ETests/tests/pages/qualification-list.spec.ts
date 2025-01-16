import {test, expect} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName} from '../shared/playwrightWrapper';

test.describe('A spec used to test the qualification list page', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the details are showing on the page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/qualifications");

        await checkText(page, "#your-search-header", "Your search");
        await checkText(page, "#filter-country", "England");
        await checkText(page, "#filter-start-date", "June 2022");
        await checkText(page, "#filter-level", "Level 3");
        await checkText(page, "#filter-org", "NCFE");
        await checkText(page, "#heading", "Test Header");
        await checkText(page, "#found-heading", "3 qualifications found");
        await checkText(page, "#pre-search-content", "Pre search box content");
        await checkText(page, "#post-list-content", "Link to not on list advice page");
        await checkText(page, "#post-filter-content", "Post search criteria content");
        await checkText(page, "#clear-search", "Clear search");
        await expect(page.locator("#no-result-content")).toHaveCount(0);
    });

    test("Shows the default headings when any level and no awarding organisation selected", async ({
                                                                                                       page,
                                                                                                       context
                                                                                                   }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/qualifications");

        await checkText(page, "#filter-level", "Any level");
        await checkText(page, "#filter-org", "Various awarding organisations");
    });

    test("Shows the correct no results content when there are no results in the search", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/qualifications");

        await checkText(page, "#found-heading", "No qualifications found");
        await checkText(page, "#no-result-content", "Test no qualifications text");
    });
});