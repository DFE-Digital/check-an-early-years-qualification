import {test, expect} from '@playwright/test';
import {
    startJourney,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    selectNotOnTheListAsTheAwardingOrganisation,
    checkYourAnswersPage,
    selectQualification
} from '../_shared/playwrightWrapper';

test.describe("A spec used to smoke test the environment once a deployment has happened", {tag: "@smoke"}, () => {
    test("should return search results", async ({page, context}) => {

        await startJourney(page, context);
        await whereWasTheQualificationAwarded(page, "#england");
        await whenWasQualificationStarted(page, '7', '2015', '9', '2019');
        await whatLevelIsTheQualification(page, 0);
        await selectNotOnTheListAsTheAwardingOrganisation(page);
        await checkYourAnswersPage(page);
        await selectQualification(page);
        await expect(page.locator("#no-result-content")).not.toBeVisible();
    });
});
