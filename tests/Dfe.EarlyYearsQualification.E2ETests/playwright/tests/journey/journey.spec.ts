import {
    test,
    expect
} from '@playwright/test';

import {
    startJourney,
    checkUrl,
    checkValue,
    checkText,
    clickBackButton,
    refineQualificationSearch,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    whatIsTheAwardingOrganisation,
    selectQualification,
    confirmQualificiation,
    processAdditionalRequirement,
    confirmAdditonalRequirementsAnswers,
    checkDetailsPage
} from '../shared/processLogic';

test.describe('A spec used to test the various routes through the journey', () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    test("should redirect the user when they select qualification was awarded outside the UK", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#outside-uk");
        checkUrl(page, "/advice/qualification-outside-the-united-kingdom");
    });

    test("should redirect the user when they select qualification was awarded in Scotland", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#scotland");
        checkUrl(page, "/advice/qualifications-achieved-in-scotland");
    });

    test("should redirect the user when they select qualification was awarded in Wales", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#wales");
        checkUrl(page, "/advice/qualifications-achieved-in-wales");
    });

    test("should redirect the user when they select qualification was awarded in Northern Ireland", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#northern-ireland");
        checkUrl(page, "/advice/qualifications-achieved-in-northern-ireland");
    });

    test("should redirect the user when they select qualification was awarded in England", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await selectQualification(page, "EYQ-240");
        await confirmQualificiation(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        checkDetailsPage(page, "EYQ-240");
    });

    test("Selecting the 'Qualification is not on the list' link on the qualification list page should navigate to the correct advice page", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);

        // qualifications page - click a qualification in the list to move us on
        checkUrl(page, "/qualifications");

        // click not on the list link
        await page.locator('a[href="/advice/qualification-not-on-the-list"]').click();

        // qualification not on the list page
        checkUrl(page, "/advice/qualification-not-on-the-list");
        checkText(page, "#advice-page-heading", "This is the level 3 page");

        // check back button goes back to the qualifications list page
        await clickBackButton(page);
        checkUrl(page, "/qualifications");
    });

    test("Selecting qualification level 7 started after 1 Sept 2014 should navigate to the level 7 post 2014 advice page", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "8", "2015");
        await whatLevelIsTheQualification(page, 7);
        checkUrl(page, "/advice/level-7-qualification-post-2014");
        await clickBackButton(page);
        checkUrl(page, "/questions/what-level-is-the-qualification");
    });

    test("Should remove the search criteria when a user goes to the awarding organisation page and back again", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        checkUrl(page, "/qualifications");
        refineQualificationSearch(page, 'test');
        checkUrl(page, "/qualifications");
        await clickBackButton(page);
        await whatIsTheAwardingOrganisation(page, 1);
        checkUrl(page, "/qualifications");
        checkValue(page, "#refineSearch", '');
    });

    [
        ['09', '2014'],
        ['06', '2017'],
        ['08', '2019'],
    ].forEach((date) => {
        const [month, year] = date;

        test(`should redirect when qualification is level 2 and startMonth is ${month} and startYear is ${year}`, async ({page}) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, month, year);
            await whatLevelIsTheQualification(page, 2);
            checkUrl(page, "/advice/level-2-qualifications-started-between-1-sept-2014-and-31-aug-2019");
        });
    });

    test("should bypass remaining additional requirement question when answering yes to the Qts question", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 6);
        await whatIsTheAwardingOrganisation(page, 1);
        await selectQualification(page, "EYQ-108");
        await confirmQualificiation(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-108", 1, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-108");
        checkDetailsPage(page, "EYQ-108");
    });

    test("should not bypass remaining additional requirement question when answering no to the Qts question", async ({page}) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 6);
        await whatIsTheAwardingOrganisation(page, 1);
        await selectQualification(page, "EYQ-108");
        await confirmQualificiation(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-108", 1, "#no");
        await processAdditionalRequirement(page, "EYQ-108", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-108");
        checkDetailsPage(page, "EYQ-108");
    });
});