import {test} from '@playwright/test';
import {
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    whatIsTheAwardingOrganisation,
    checkYourAnswersPage,
    selectQualification,
    confirmQualification,
    processAdditionalRequirement,
    confirmAdditonalRequirementsAnswers,
    goToStartPage,
    checkSnapshot,
    clickSubmitAndCheckSnapshot,
    precheckPage
} from '../../_shared/playwrightWrapper';

test.describe('Snapshots', {tag: "@snapshot"}, () => {
    test.beforeEach(async ({page, context}) => {
        await goToStartPage(page, context);
    });

    test("Start page", async ({page}) => {
        await checkSnapshot(page);
    });

    test("Pre check page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await checkSnapshot(page);
        await page.locator('#pre-check-submit').click();
        await checkSnapshot(page);
    });

    test("Location page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await checkSnapshot(page);
        await clickSubmitAndCheckSnapshot(page);
    });

    test.describe('Advice pages', () => {
        ["scotland", "wales", "northern-ireland", "outside-uk"].forEach((location) => {
            test(`${location}`, async ({page}) => {
                await page.locator('#start-now-button').click();
                await precheckPage(page, '#yes');
                await whereWasTheQualificationAwarded(page, "#" + location);
                await checkSnapshot(page);
            });
        });
    });

    test("When page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await checkSnapshot(page);
        await clickSubmitAndCheckSnapshot(page);
        await whenWasQualificationStarted(page, "0", "2020", "", "2019");
        await checkSnapshot(page);
    });

    test("Level page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await checkSnapshot(page);
        await clickSubmitAndCheckSnapshot(page);
    });

    test("Awarding Organisation page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await checkSnapshot(page);
        await clickSubmitAndCheckSnapshot(page);
    });

    test("Check answers page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkSnapshot(page);
    });

    test("Qualification select page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await checkSnapshot(page);
    });

    test("Qualification not on list page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await page.locator('a[href="/advice/qualification-not-on-the-list"]').click();
        await checkSnapshot(page);
    });

    test("Qualification confirm page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await checkSnapshot(page);
    });

    test("Additional questions page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await confirmQualification(page, "#yes");
        await checkSnapshot(page);
        await page.locator("#additional-requirement-button").click();
        await checkSnapshot(page);
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await checkSnapshot(page);
        await processAdditionalRequirement(page, "EYQ-240", 2, "#no");
        await checkSnapshot(page);
    });

    test("Qualification details page", async ({page}) => {
        await page.locator('#start-now-button').click();
        await precheckPage(page, '#yes');
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "12", "2020", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#no");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        await checkSnapshot(page);
    });
});