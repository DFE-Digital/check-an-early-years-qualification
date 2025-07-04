﻿import {test} from '@playwright/test';
import {
    startJourney,
    checkUrl,
    checkText,
    clickBackButton,
    refineQualificationSearch,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    whatIsTheAwardingOrganisation,
    checkYourAnswersPage,
    selectQualification,
    confirmQualification,
    processAdditionalRequirement,
    confirmAdditonalRequirementsAnswers,
    checkDetailsPage,
    checkEmptyValue,
    inputText,
    isVisible
} from '../../_shared/playwrightWrapper';

test.describe('A spec used to test the various routes through the journey', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("should redirect user to the help confirmation page when form is submitted on the help page", async ({page}) => {
        await page.goto("/advice/help");

        await page.click("#Option\\ 1");
        await inputText(page, "#EmailAddress", "test@test.com");
        await inputText(page, "#AdditionalInformationMessage", "This is the message");
        await page.click("#help-form-submit");
        
        await checkUrl(page, "/advice/help/confirmation");
        await isVisible(page, "#success-message");
        await isVisible(page, "#help-confirmation-body");
    });

    test("should redirect the user when they select qualification was awarded outside the UK", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#outside-uk");
        await checkUrl(page, "/advice/qualification-outside-the-united-kingdom");
    });

    test("should redirect the user when they select qualification was awarded in Scotland", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#scotland");
        await checkUrl(page, "/advice/qualifications-achieved-in-scotland");
    });

    test("should redirect the user when they select qualification was awarded in Wales", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#wales");
        await checkUrl(page, "/advice/qualifications-achieved-in-wales");
    });

    test("should redirect the user when they select qualification was awarded in Northern Ireland", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#northern-ireland");
        await checkUrl(page, "/advice/qualifications-achieved-in-northern-ireland");
    });

    test("should redirect the user when they select qualification was awarded in England", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        await checkDetailsPage(page, "EYQ-240");
    });

    test("Selecting the 'Qualification is not on the list' link on the qualification list page should navigate to the correct advice page", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);

        // qualifications page - click a qualification in the list to move us on
        await checkUrl(page, "/select-a-qualification-to-check");

        // click not on the list link
        await page.locator('a[href="/advice/qualification-not-on-the-list"]').click();

        // qualification not on the list page
        await checkUrl(page, "/advice/qualification-not-on-the-list");
        await checkText(page, "#advice-page-heading", "This is the level 3 page");

        // check back button goes back to the qualifications list page
        await clickBackButton(page);
        await checkUrl(page, "/select-a-qualification-to-check");
    });

    test("Selecting qualification level 7 started after 1 Sept 2014 should navigate to the level 7 post 2014 advice page", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "8", "2015", "1", "2025");
        await whatLevelIsTheQualification(page, 7);
        await checkUrl(page, "/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        await clickBackButton(page);
        await checkUrl(page, "/questions/what-level-is-the-qualification");
    });


    test("Selecting qualification level 7 started after 1 Sept 2019 should navigate to the level 7 post 2019 advice page", async ({page}) => {

        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "8", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 7);
        await checkUrl(page, '/advice/level-7-qualification-after-aug-2019');
        await clickBackButton(page);
        await checkUrl(page, "/questions/what-level-is-the-qualification");
    })


    test("Should remove the search criteria when a user goes to the awarding organisation page and back again", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await checkUrl(page, "/select-a-qualification-to-check");
        await refineQualificationSearch(page, 'test');
        await checkUrl(page, "/select-a-qualification-to-check");
        await clickBackButton(page);
        await clickBackButton(page);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await checkUrl(page, "/select-a-qualification-to-check");
        await checkEmptyValue(page, "#refineSearch");
    });

    [
        ['09', '2014'],
        ['06', '2017'],
        ['08', '2019'],
    ].forEach((date) => {
        const [month, year] = date;

        test(`should redirect when qualification is level 2 and startMonth is ${month} and startYear is ${year}`, async ({page}) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, month, year, "1", "2025");
            await whatLevelIsTheQualification(page, 2);
            await checkUrl(page, "/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        });
    });

    test("should bypass remaining additional requirement question when answering yes to the Qts question", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 6);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-108");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-108", 1, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-108");
        await checkDetailsPage(page, "EYQ-108");
    });

    test("should not bypass remaining additional requirement question when answering no to the Qts question", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 6);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-108");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-108", 1, "#no");
        await processAdditionalRequirement(page, "EYQ-108", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-108");
        await checkDetailsPage(page, "EYQ-108");
    });
});