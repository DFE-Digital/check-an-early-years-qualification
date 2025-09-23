import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    doesNotExist,
    exists,
    checkTextContains
} from '../../_shared/playwrightWrapper';

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
        await checkText(page, "#found-heading", "We found 6 matching qualifications");
        await checkText(page, "#pre-search-content", "Pre search box content");
        await checkText(page, "#post-list-heading", "Post qualification list header");
        await checkTextContains(page, "#post-list-content", "Link to not on list advice page");
        await checkText(page, "#clear-search", "Clear search");
        await doesNotExist(page, "#no-result-content");
        await doesNotExist(page, "#l6-or-not-sure-content");
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
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22WhenWasQualificationAwarded%22%3A%221%2F2025%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await exists(page, "#ao-text-EYQ-114");
        await checkText(page, "#ao-text-EYQ-114", "Qualification Number (QN) 123 / 345 / 678");
        await exists(page, "#ao-text-EYQ-115");
        await checkText(page, "#ao-text-EYQ-115", "Qualification Number (QN) 233 / 420 / 12");
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

    test("Shows pre 2014 content when there when the user searched for L6 which started before Sept 2014", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2014%22%2C%22LevelOfQualification%22%3A%226%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#l6-or-not-sure-heading", "Pre 2014 L6 or not sure heading");
        await checkTextContains(page, "#l6-or-not-sure-content", "Pre 2014 L6 or not sure content");
    });

    test("Shows post 2014 content when there when the user searched for L6 which started after Sept 2014", async ({
                                                                                                                      page,
                                                                                                                      context
                                                                                                                  }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2018%22%2C%22LevelOfQualification%22%3A%226%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#l6-or-not-sure-heading", "Post 2014 L6 or not sure heading");
        await checkTextContains(page, "#l6-or-not-sure-content", "Post 2014 L6 or not sure content");
    });

    test("Shows pre 2014 content when there when the user selected not sure for the level and started before Sept 2014", async ({
                                                                                                                      page,
                                                                                                                      context
                                                                                                                  }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2014%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#l6-or-not-sure-heading", "Pre 2014 L6 or not sure heading");
        await checkTextContains(page, "#l6-or-not-sure-content", "Pre 2014 L6 or not sure content");
    });

    test("Shows post 2014 content when there when the user selected not sure for the level and started after Sept 2014", async ({
                                                                                                                      page,
                                                                                                                      context
                                                                                                                  }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2018%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#l6-or-not-sure-heading", "Post 2014 L6 or not sure heading");
        await checkTextContains(page, "#l6-or-not-sure-content", "Post 2014 L6 or not sure content");
    });
});