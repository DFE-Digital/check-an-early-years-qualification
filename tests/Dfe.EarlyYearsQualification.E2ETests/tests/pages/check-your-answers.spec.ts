import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    checkUrl
} from '../shared/playwrightWrapper';

test.describe('A spec used to test the check your answers page', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        /* {"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"7/2015","WhenWasQualificationAwarded":"9/2017","LevelOfQualification":"3","WhatIsTheAwardingOrganisation":"NCFE","SelectedAwardingOrganisationNotOnTheList":false} */
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%229%2F2017%22%2C%22LevelOfQualification%22%3A%223%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22NCFE%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Afalse%7D', journeyCookieName);
    });

    test("Checks page contains the correct content", async ({page}) => {
        await page.goto("/questions/check-your-answers");

        await checkText(page, ".govuk-heading-xl", "Check your answers");
        await checkText(page, "#question-1-question", "Where was the qualification awarded?");
        await checkText(page, "#question-1-answer", "England");
        await checkText(page, "#question-2-question", "Test Dates Questions");
        await checkText(page, "#question-2-answer > p", "Started in July 2015", 0);
        await checkText(page, "#question-2-answer > p", "Awarded in September 2017", 1);
        await checkText(page, "#question-3-question", "What level is the qualification?");
        await checkText(page, "#question-3-answer", "Level 3");
        await checkText(page, "#question-4-question", "Test Dropdown Question");
        await checkText(page, "#question-4-answer", "NCFE");
        await checkText(page, "#cta-button", "Continue");
    });

    test("Checks change link for question 1 is correct", async ({page}) => {
        await page.goto("/questions/check-your-answers");

        await checkText(page, "#question-1-change", "Change Where was the qualification awarded?");
        await checkText(page, "#question-1-change-hidden", "Where was the qualification awarded?");
        
        await page.click("#question-1-change a");
        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
    });

    test("Checks change link for question 2 is correct", async ({page}) => {
        await page.goto("/questions/check-your-answers");

        await checkText(page, "#question-2-change", "Change Test Dates Questions");
        await checkText(page, "#question-2-change-hidden", "Test Dates Questions");
        
        await page.click("#question-2-change a");
        await checkUrl(page, "/questions/when-was-the-qualification-started-and-awarded");
    });

    test("Checks change link for question 3 is correct", async ({page}) => {
        await page.goto("/questions/check-your-answers");

        await checkText(page, "#question-3-change", "Change What level is the qualification?");
        await checkText(page, "#question-3-change-hidden", "What level is the qualification?");
        
        await page.click("#question-3-change a");
        await checkUrl(page, "/questions/what-level-is-the-qualification");
    });

    test("Checks change link for question 4 is correct", async ({page}) => {
        await page.goto("/questions/check-your-answers");

        await checkText(page, "#question-4-change", "Change Test Dropdown Question");
        await checkText(page, "#question-4-change-hidden", "Test Dropdown Question");

        await page.click("#question-4-change a");
        await checkUrl(page, "/questions/what-is-the-awarding-organisation");
    });

    test("Checks the answer test is correct when a user selects not sure and not in the list for previous questions", async ({page, context}) => {

        /* {"WhereWasQualificationAwarded":"england","WhenWasQualificationStarted":"7/2015","WhenWasQualificationAwarded":"9/2017","LevelOfQualification":"0","WhatIsTheAwardingOrganisation":"","SelectedAwardingOrganisationNotOnTheList":true} */
        await setCookie(context, '%7B%22WhereWasQualificationAwarded%22%3A%22england%22%2C%22WhenWasQualificationStarted%22%3A%227%2F2015%22%2C%22WhenWasQualificationAwarded%22%3A%229%2F2017%22%2C%22LevelOfQualification%22%3A%220%22%2C%22WhatIsTheAwardingOrganisation%22%3A%22%22%2C%22SelectedAwardingOrganisationNotOnTheList%22%3Atrue%7D', journeyCookieName);
        
        await page.goto("/questions/check-your-answers");
        
        await checkText(page, "#question-3-question", "What level is the qualification?");
        await checkText(page, "#question-3-answer", "Any level");
        await checkText(page, "#question-4-question", "Test Dropdown Question");
        await checkText(page, "#question-4-answer", "Various awarding organisations");
    });
});