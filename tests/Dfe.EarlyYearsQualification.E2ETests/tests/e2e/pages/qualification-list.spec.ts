import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    doesNotExist,
    exists,
    checkTextContains,
    hasCount
} from '../../_shared/playwrightWrapper';

test.describe('A spec used to test the qualification list page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("Checks the details are showing on the page", async ({page, context}) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%2210%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#your-search-header", "Your search");
        await checkText(page, "#filter-country", "awarded in England");
        await checkText(page, "#filter-start-date", "started in June 2015");
        await checkText(page, "#filter-awarded-date", "awarded in October 2015");
        await checkText(page, "#filter-level", "level 3");
        await checkText(page, "#filter-org", "awarded by NCFE");
        await checkText(page, "#heading", "Test Header");
        await checkText(page, "#found-heading", "We found 8 matching qualifications");
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

    test("Shows the correct no results content when there are no results in the search", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2022%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await checkText(page, "#found-heading", "We found 0 matching qualifications");
        await checkText(page, "#no-result-content", "Test no qualifications text");
    });

    test("Shows additional information content next to qualification link", async ({
                                                                                                    page,
                                                                                                    context
                                                                                                }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%2210%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await exists(page, "#ao-text-EYQ-307");
        await checkText(page, "#ao-text-EYQ-307", "Select this if the degree covers just one subject, for example a BA (Hons) Early Childhood Studies.");
        await exists(page, "#ao-text-EYQ-308");
        await checkText(page, "#ao-text-EYQ-308", "Select this if the degree covers 2 or more subjects, for example BA (Hons) Early Childhood Studies and Psychology.");

    });

    test("Qualification is a duplicate, override the QN number and shows additional information next to qualification link", async ({
        page,
        context
    }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%2210%2F2015%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await exists(page, "#ao-text-EYQ-309");
        await checkText(page, "#ao-text-EYQ-309", "Content which replaces QN number");
        await exists(page, "#ao-text-EYQ-310");
        await checkText(page, "#ao-text-EYQ-310", "Content which replaces QN number");

    });

    test("Not on the list selected, returns qualifications which have various awarding organisations", async ({
        page,
        context
    }) => {
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%226%2F2018%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%7D', journeyCookieName);
        await page.goto("/select-a-qualification-to-check");

        await hasCount(page, "#main-content a.govuk-link.govuk-heading-m", 5);
        await checkText(page, "#main-content > div > div > div:nth-child(6) a.govuk-link.govuk-heading-m", "BTEC");
        await checkText(page, "#main-content > div > div > div:nth-child(7) a.govuk-link.govuk-heading-m", "BTEC");
        await checkText(page, "#main-content > div > div > div:nth-child(8) a.govuk-link.govuk-heading-m", "EYQ-106-test");
        await checkText(page, "#main-content > div > div > div:nth-child(9) a.govuk-link.govuk-heading-m", "EYQ-110-test");
        await checkText(page, "#main-content > div > div > div:nth-child(10) a.govuk-link.govuk-heading-m", "Qualification 304");
    });
});