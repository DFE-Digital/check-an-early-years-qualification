import {test} from '@playwright/test';
import {
    startJourney,
    checkUrl,
    clickBackButton,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    whatIsTheAwardingOrganisation,
    selectQualification,
    confirmQualificiation,
    processAdditionalRequirement,
    confirmAdditonalRequirementsAnswers,
    checkDetailsPage
} from '../shared/playwrightWrapper';

test.describe("A spec used to test the main back button route through the journey", () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });
    test("back buttons should all navigate to the appropriate pages in the main journey", async ({page}) => {

        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "6", "2022");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await selectQualification(page, "EYQ-240");
        await confirmQualificiation(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#no");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        await checkDetailsPage(page, "EYQ-240");

        await clickBackButton(page);
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/confirm-answers");
        await clickBackButton(page);
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/2");
        await clickBackButton(page);
        await checkUrl(page, "/qualifications/check-additional-questions/EYQ-240/1");
        await clickBackButton(page);
        await checkUrl(page, "/qualifications");
        await clickBackButton(page);
        await checkUrl(page, "/questions/what-is-the-awarding-organisation");
        await clickBackButton(page);
        await checkUrl(page, "/questions/what-level-is-the-qualification");
        await clickBackButton(page);
        await checkUrl(page, "/questions/when-was-the-qualification-started");
        await clickBackButton(page);
        await checkUrl(page, "/questions/where-was-the-qualification-awarded");
        await clickBackButton(page);
        await checkUrl(page, process.env.WEBAPP_URL + "/");
    });

    test.describe("back buttons should all navigate to the appropriate pages in the main journey", async () => {
        test("the back button on the accessibility statement page navigates back to the home page", async ({page}) => {
            await page.goto("/accessibility-statement");
            await clickBackButton(page);
            await checkUrl(page, process.env.WEBAPP_URL + "/");
        });

        test("the back button on the cookies preference page navigates back to the home page", async ({page}) => {
            await page.goto("/cookies");
            await clickBackButton(page);
            await checkUrl(page, process.env.WEBAPP_URL + "/");
        });
    });
});
