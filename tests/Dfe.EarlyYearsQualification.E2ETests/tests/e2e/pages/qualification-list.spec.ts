import {test} from '@playwright/test';
import {startJourney, checkText, setCookie, journeyCookieName, doesNotExist, exists} from '../../shared/playwrightWrapper';

test.describe('A spec used to test the qualification list page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the details are showing on the page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#your-search-header", "Your search");
        await checkText(page, "#filter-country", "awarded in England");
        await checkText(page, "#filter-start-date", "started in June 2022");
        await checkText(page, "#filter-awarded-date", "awarded in January 2025");
        await checkText(page, "#filter-level", "level 3");
        await checkText(page, "#filter-org", "awarded by NCFE");
        await checkText(page, "#heading", "Test Header");
        await checkText(page, "#found-heading", "We found 3 matching qualifications");
        await checkText(page, "#pre-search-content", "Pre search box content");
        await checkText(page, "#post-list-content", "Link to not on list advice page");
        await checkText(page, "#clear-search", "Clear search");
        await doesNotExist(page, "#no-result-content");
    });

    test("Shows the default headings when any level and no awarding organisation selected", async ({
                                                                                                       page,
                                                                                                       context
                                                                                                   }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#filter-level", "any level");
        await checkText(page, "#filter-org", "awarded by various awarding organisations");
    });

    test("Doesnt show the awarding organisation next to the qualification name if there are no matches", async ({
                                                                                                       page,
                                                                                                       context
                                                                                                   }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await doesNotExist(page, "#ao-text-EYQ-240");
        await doesNotExist(page, "#ao-text-EYQ-103");
    });

    test("Shows the awarding organisation next to the qualification name when multiple qualifications share the same name", async ({
                                                                                                       page,
                                                                                                       context
                                                                                                   }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%226%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await exists(page, "#ao-text-EYQ-114");
        await checkText(page, "#ao-text-EYQ-114", "(Edexcel (now Pearson Education Ltd))");
        await exists(page, "#ao-text-EYQ-115");
        await checkText(page, "#ao-text-EYQ-115", "(NCFE)");
    });

    test("Shows the correct no results content when there are no results in the search", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#found-heading", "We found 0 matching qualifications");
        await checkText(page, "#no-result-content", "Test no qualifications text");
    });
});