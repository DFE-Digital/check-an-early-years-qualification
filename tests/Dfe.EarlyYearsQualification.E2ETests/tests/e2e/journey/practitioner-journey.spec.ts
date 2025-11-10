import {test} from '@playwright/test';
import {
    startJourney,
    checkUrl,
    checkText,
    clickBackButton,
    checkingOwnQualificationOrSomeoneElsesPage,
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
    checkDetailsInset,
    checkRatiosHeading,
    checkLevelRatioDetails,
    RatioStatus
} from '../../_shared/playwrightWrapper';

test.describe('A spec used to test the various routes through the practitioner journey', {tag: "@e2e"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await checkingOwnQualificationOrSomeoneElsesPage(page, "#yes");
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
        await checkText(page, "#advice-page-heading", "This is the practitioner level 3 page");
        await checkText(page, "#advice-page-body", "This is the practitioner body text");

        // check back button goes back to the qualifications list page
        await clickBackButton(page);
        await checkUrl(page, "/select-a-qualification-to-check");
    });

    test("Checking own qualification, qualification is not full and relevant returns expected content", async ({ page }) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "3", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#no");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        await checkDetailsPage(page, "EYQ-240");
        await checkText(page, "#requirements-heading", "This is NF&R practitioner heading", 0);
        await checkText(page, "#requirements-heading ~ p", "This is NF&R practitioner text", 0);
    });

    test("Checking own qualification, qualification is full and relevant returns expected content", async ({ page }) => {
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "3", "2022", "1", "2025");
        await whatLevelIsTheQualification(page, 3);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-240");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 1, "#yes");
        await processAdditionalRequirement(page, "EYQ-240", 2, "#no");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-240");
        await checkDetailsPage(page, "EYQ-240");
        await checkText(page, "#requirements-heading", "This is F&R practitioner heading", 0);
        await checkText(page, "#requirements-heading ~ p", "This is F&R practitioner text", 0);
    });

    test('Checks level 6 degree not approved shows EYITT content', async ({
                                                                              page,
                                                                              context
                                                                          }) => {

        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, "1", "2012", "7", "2016");
        await whatLevelIsTheQualification(page, 6);
        await whatIsTheAwardingOrganisation(page, 1);
        await checkYourAnswersPage(page);
        await selectQualification(page, "EYQ-321");
        await confirmQualification(page, "#yes");
        await processAdditionalRequirement(page, "EYQ-321", 1, "#no");
        await processAdditionalRequirement(page, "EYQ-321", 2, "#yes");
        await confirmAdditonalRequirementsAnswers(page, "EYQ-321");
        await checkDetailsPage(page, "EYQ-321");

        await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading");

        await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
            detailText: "Level 3 must English must PFA"
        });
        await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
            detailText: "Level 2 must PFA"
        });
        await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: "Summary card default content"  });
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.PossibleRouteAvailable, { detailText: 'This is the EYITT content' });
    });
});