import { test, expect } from '@playwright/test';

import {
    startJourney,
    checkText,
    checkUrl,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    selectNotOnTheListAsTheAwardingOrganisation,
    checkYourAnswersPage,
    checkingOwnQualificationOrSomeoneElsesPage,
    selectICannotFindTheQualification
} from '../_shared/playwrightWrapper';

type Scenario = {
    scenarioId: number,
    isCheckingOwnQualification: boolean,
    monthStarted: string,
    yearStarted: string,
    monthAwarded: string,
    yearAwarded: string,
    selectedLevel: number,
    expectedText: string,
    expectedUrl: string
}

const expectedTexts: Record<string, string> = {
    before2014: `before 1 September 2014.`,
    between2014Sep1And2019Mar31: `between 1 September 2014 and 31 March 2019.`,
    between2014Sep1And2019Aug31: `between 1 September 2014 and 31 August 2019.`,
    between2019Apr1And2019Aug31: `between 1 April 2019 and 31 August 2019.`,
    between2019Sep1And2024Mar31: `between 1 September 2019 and 31 March 2024.`,
    onOrAfter2019Sep1: `on or after 1 September 2019.`,
    onOrAfter2024Apr1: `on or after 1 April 2024.`,
};

const getLevelText = (level) => `You are seeing this page because you are checking a ${level == 0 ? "" : `level ${level}`} qualification that was started `;

test.describe('A spec used to validate variants for qualification results and “Cannot find qualification"', {tag: "@validation"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    const createScenario = (overrides: Partial<Scenario>): Scenario => ({
        scenarioId: 0,
        isCheckingOwnQualification: false,
        monthStarted: '01',
        yearStarted: '2001',
        monthAwarded: '02',
        yearAwarded: '2002',
        selectedLevel: 0,
        expectedText: '',
        expectedUrl: '',
        ...overrides
    });

    const managerScenarios: Scenario[] = [
        // Before 1 September 2014
        createScenario({
            scenarioId: 1,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 2,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 2,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 3,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 3,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 4,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 4,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 5,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 5,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 6,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 6,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 7,
            expectedText: expectedTexts.before2014
        }),
        createScenario({
            scenarioId: 7,
            monthStarted: '01',
            yearStarted: '2001',
            monthAwarded: '02',
            yearAwarded: '2002',
            selectedLevel: 0,
            expectedText: expectedTexts.before2014
        }),

        // Between 1 September 2014 and 31 March 2019
        createScenario({
            scenarioId: 8,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 0,
            expectedText: expectedTexts.between2014Sep1And2019Mar31
        }),
        createScenario({
            scenarioId: 9,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 3,
            expectedText: expectedTexts.between2014Sep1And2019Mar31
        }),
        createScenario({
            scenarioId: 10,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 4,
            expectedText: expectedTexts.between2014Sep1And2019Aug31
        }),
        createScenario({
            scenarioId: 11,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 5,
            expectedText: expectedTexts.between2014Sep1And2019Aug31
        }),
        createScenario({
            scenarioId: 12,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 6,
            expectedText: expectedTexts.between2014Sep1And2019Aug31
        }),

        // Between 1 April 2019 and 31 August 2019
        createScenario({
            scenarioId: 13,
            monthStarted: '04',
            yearStarted: '2019',
            monthAwarded: '05',
            yearAwarded: '2019',
            selectedLevel: 0,
            expectedText: expectedTexts.between2019Apr1And2019Aug31
        }),
        createScenario({
            scenarioId: 14,
            monthStarted: '04',
            yearStarted: '2019',
            monthAwarded: '05',
            yearAwarded: '2019',
            selectedLevel: 3,
            expectedText: expectedTexts.between2019Apr1And2019Aug31
        }),

        // Between 1 September 2019 and 31 March 2024
        createScenario({
            scenarioId: 15,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 0,
            expectedText: expectedTexts.between2019Sep1And2024Mar31
        }),
        createScenario({
            scenarioId: 16,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 3,
            expectedText: expectedTexts.between2019Sep1And2024Mar31
        }),

        // On or after 1 September 2019
        createScenario({
            scenarioId: 17,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 2,
            expectedText: expectedTexts.onOrAfter2019Sep1
        }),
        createScenario({
            scenarioId: 18,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 4,
            expectedText: expectedTexts.onOrAfter2019Sep1
        }),
        createScenario({
            scenarioId: 19,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 5,
            expectedText: expectedTexts.onOrAfter2019Sep1
        }),
        createScenario({
            scenarioId: 20,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 6,
            expectedText: expectedTexts.onOrAfter2019Sep1
        }),

        // On or after 1 April 2024
        createScenario({
            scenarioId: 21,
            monthStarted: '04',
            yearStarted: '2024',
            monthAwarded: '05',
            yearAwarded: '2024',
            selectedLevel: 0,
            expectedText: expectedTexts.onOrAfter2024Apr1
        }),
        createScenario({
            scenarioId: 22,
            monthStarted: '04',
            yearStarted: '2024',
            monthAwarded: '05',
            yearAwarded: '2024',
            selectedLevel: 3,
            expectedText: expectedTexts.onOrAfter2024Apr1
        })
    ];

    // pracitioner scenarios have the same data as manager scenarios, but isCheckingOwnQualification is true
    const practitionerScenarios = managerScenarios.map(obj => ({
        ...obj,
        isCheckingOwnQualification: true
    }));

    [...managerScenarios, ...practitionerScenarios].forEach((scenario) => {
        test(`Check ${scenario.isCheckingOwnQualification ? "practitioner " : "manager"} scenario ${scenario.scenarioId}`, async ({ page }) => {
            await checkingOwnQualificationOrSomeoneElsesPage(page, scenario.isCheckingOwnQualification ? "#yes" : "#no");
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await selectNotOnTheListAsTheAwardingOrganisation(page);
            await checkYourAnswersPage(page);
            await selectICannotFindTheQualification(page);
            await checkText(page, "#advice-page-body > div.govuk-inset-text > p", getLevelText(scenario.selectedLevel) + scenario.expectedText);

            if (scenario.isCheckingOwnQualification) {
                await expect(page.getByText('If you need more help')).toBeVisible();
                await expect(page.getByText('Related content')).not.toBeAttached();
            }
            else {

                await expect(page.getByText('Related content')).toBeVisible();
                await expect(page.getByText('If you need more help')).not.toBeAttached();
            }
        });
    });
});

test.describe('A spec used to validate the static level 7 versions of the “Cannot find qualification page', { tag: "@validation" }, () => {
    test.beforeEach(async ({ page, context }) => {
        await startJourney(page, context);
    });

    var staticLevel7Scenario = [
        {
            scenarioId: 1,
            monthStarted: '09',
            yearStarted: '2014',
            monthAwarded: '10',
            yearAwarded: '2014',
            selectedLevel: 7,
            expectedUrl: "/advice/level-7-qualifications-started-between-1-sept-2014-and-31-aug-2019",
        } as Scenario,
        {
            scenarioId: 2,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '10',
            yearAwarded: '2019',
            selectedLevel: 7,
            expectedUrl: "/advice/level-7-qualification-after-aug-2019",
        } as Scenario
    ];

    [...staticLevel7Scenario].forEach((scenario) => {
        test(`Static level 7 versions for scenario ${scenario.scenarioId}`, async ({ page }) => {
            await checkingOwnQualificationOrSomeoneElsesPage(page, "#no");
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await checkUrl(page, scenario.expectedUrl);
        });
    });
});