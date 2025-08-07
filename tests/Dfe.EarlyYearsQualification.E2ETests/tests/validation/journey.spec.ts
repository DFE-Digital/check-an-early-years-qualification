import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    whereWasTheQualificationAwarded,
    whenWasQualificationStarted,
    whatLevelIsTheQualification,
    whatIsTheAwardingOrganisationValue,
    selectNotOnTheListAsTheAwardingOrganisation,
    checkYourAnswersPage,
    checkNumberOfMatchingQualifications,
    selectQualification,
    confirmQualification,
    processAdditionalRequirement,
    confirmAdditonalRequirementsAnswers,
    checkDetailsPage
} from '../_shared/playwrightWrapper';

type Scenario = {
    scenarioId: number,
    monthStarted: string,
    yearStarted: string,
    monthAwarded: string,
    yearAwarded: string,
    selectedLevel: number,
    selectedAwardingOrganisation: string,
    noOfMatchingQualifications: number,
    qualificationToSelect: string,
    ratioForUnqualified: string
    ratioForLevel2: string,
    ratioForLevel3: string,
    ratioForLevel6: string,
    additionalRequirements: AdditionalRequirement[]
}

type AdditionalRequirement = {
    index: number,
    answer: string
}

const noAtQuestionOne: AdditionalRequirement = {index: 1, answer: '#no'};
const yesAtQuestionOne: AdditionalRequirement = {index: 1, answer: '#yes'};
const noAtQuestionTwo: AdditionalRequirement = {index: 2, answer: '#no'};
const yesAtQuestionTwo: AdditionalRequirement = {index: 2, answer: '#yes'};

const approved = "Approved";
const notApproved = "Not approved";
const possibleRouteAvailable = "Possible route available";

test.describe('A spec used to validate changes to the journey against actual data', {tag: "@validation"}, () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
    });

    // Various AO scenarios
    [
        {
            scenarioId: 1,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 2,
            noOfMatchingQualifications: 10,
            qualificationToSelect: 'EYQ-212',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 2,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 3,
            noOfMatchingQualifications: 26,
            qualificationToSelect: 'EYQ-222',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 3,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 4,
            noOfMatchingQualifications: 6,
            qualificationToSelect: 'EYQ-252',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 4,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 5,
            noOfMatchingQualifications: 19,
            qualificationToSelect: 'EYQ-264',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
    ].forEach((scenario) => {
        test(`Various AO qualification check for scenario ${scenario.scenarioId} and qualificationId ${scenario.qualificationToSelect}`, async ({page}) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await selectNotOnTheListAsTheAwardingOrganisation(page);
            await checkYourAnswersPage(page);
            await checkNumberOfMatchingQualifications(page, scenario.noOfMatchingQualifications);
            await selectQualification(page, scenario.qualificationToSelect);
            await confirmQualification(page, "#yes");
            await checkDetailsPage(page, scenario.qualificationToSelect);
            await checkText(page, '#ratio-Unqualified-tag > .govuk-tag', scenario.ratioForUnqualified);
            await checkText(page, '#ratio-Level2-tag > .govuk-tag', scenario.ratioForLevel2);
            await checkText(page, '#ratio-Level3-tag > .govuk-tag', scenario.ratioForLevel3);
            await checkText(page, '#ratio-Level6-tag > .govuk-tag', scenario.ratioForLevel6);
        });
    });

    // Selected AO scenarios
    [
        {
            scenarioId: 1,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 2,
            selectedAwardingOrganisation: 'Skillsfirst',
            noOfMatchingQualifications: 2,
            qualificationToSelect: 'EYQ-221',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 2,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 3,
            selectedAwardingOrganisation: 'Pearson Education Ltd',
            noOfMatchingQualifications: 4,
            qualificationToSelect: 'EYQ-242',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 3,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 4,
            selectedAwardingOrganisation: 'NCFE',
            noOfMatchingQualifications: 2,
            qualificationToSelect: 'EYQ-254',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 4,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 5,
            selectedAwardingOrganisation: 'University of Worcester',
            noOfMatchingQualifications: 3,
            qualificationToSelect: 'EYQ-276',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
        {
            scenarioId: 5,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 6,
            selectedAwardingOrganisation: 'Nottingham Trent University',
            noOfMatchingQualifications: 10,
            qualificationToSelect: 'EYQ-294',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: approved
        } as Scenario,
        {
            scenarioId: 6,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 0,
            selectedAwardingOrganisation: 'City & Guilds',
            noOfMatchingQualifications: 12,
            qualificationToSelect: 'EYQ-230',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved
        } as Scenario,
    ].forEach((scenario) => {
        test(`Selected AO qualification check for scenario ${scenario.scenarioId} and qualificationId ${scenario.qualificationToSelect}`, async ({page}) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await whatIsTheAwardingOrganisationValue(page, scenario.selectedAwardingOrganisation);
            await checkYourAnswersPage(page);
            await checkNumberOfMatchingQualifications(page, scenario.noOfMatchingQualifications);
            await selectQualification(page, scenario.qualificationToSelect);
            await confirmQualification(page, "#yes");
            await checkDetailsPage(page, scenario.qualificationToSelect);
            await checkText(page, '#ratio-Unqualified-tag > .govuk-tag', scenario.ratioForUnqualified);
            await checkText(page, '#ratio-Level2-tag > .govuk-tag', scenario.ratioForLevel2);
            await checkText(page, '#ratio-Level3-tag > .govuk-tag', scenario.ratioForLevel3);
            await checkText(page, '#ratio-Level6-tag > .govuk-tag', scenario.ratioForLevel6);
        });
    });

    // Various AO scenarios with Additional Questions
    [
        {
            scenarioId: 1,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 6,
            noOfMatchingQualifications: 12,
            qualificationToSelect: 'EYQ-282',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: possibleRouteAvailable,
            additionalRequirements: [noAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 2,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 7,
            noOfMatchingQualifications: 4,
            qualificationToSelect: 'EYQ-211',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved,
            additionalRequirements: [noAtQuestionOne, yesAtQuestionTwo]
        } as Scenario,
        {
            scenarioId: 3,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 7,
            noOfMatchingQualifications: 4,
            qualificationToSelect: 'EYQ-211',
            ratioForUnqualified: approved,
            ratioForLevel2: notApproved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved,
            additionalRequirements: [noAtQuestionOne, noAtQuestionTwo]
        } as Scenario,
        {
            scenarioId: 4,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 0,
            noOfMatchingQualifications: 177,
            qualificationToSelect: 'EYQ-147',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved,
            additionalRequirements: [yesAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 5,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 0,
            noOfMatchingQualifications: 73,
            qualificationToSelect: 'EYQ-287',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: possibleRouteAvailable,
            additionalRequirements: [noAtQuestionOne]
        } as Scenario,
    ].forEach((scenario) => {
        test(`Various AO qualification with additional questions check for scenario ${scenario.scenarioId} and qualificationId ${scenario.qualificationToSelect}`, async ({page}) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await selectNotOnTheListAsTheAwardingOrganisation(page);
            await checkYourAnswersPage(page);
            await checkNumberOfMatchingQualifications(page, scenario.noOfMatchingQualifications);
            await selectQualification(page, scenario.qualificationToSelect);
            await confirmQualification(page, "#yes");
            for (const x of scenario.additionalRequirements) {
                await processAdditionalRequirement(page, scenario.qualificationToSelect, x.index, x.answer);
            }
            await confirmAdditonalRequirementsAnswers(page, scenario.qualificationToSelect);
            await checkDetailsPage(page, scenario.qualificationToSelect);
            await checkText(page, '#ratio-Unqualified-tag > .govuk-tag', scenario.ratioForUnqualified);
            await checkText(page, '#ratio-Level2-tag > .govuk-tag', scenario.ratioForLevel2);
            await checkText(page, '#ratio-Level3-tag > .govuk-tag', scenario.ratioForLevel3);
            await checkText(page, '#ratio-Level6-tag > .govuk-tag', scenario.ratioForLevel6);
        });
    });

    // Selected AO scenarios with Additional Questions
    [
        {
            scenarioId: 1,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 7,
            selectedAwardingOrganisation: 'University of Gloucester',
            noOfMatchingQualifications: 2,
            qualificationToSelect: 'EYQ-205',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: notApproved,
            additionalRequirements: [noAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 2,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 7,
            selectedAwardingOrganisation: 'University of Gloucester',
            noOfMatchingQualifications: 2,
            qualificationToSelect: 'EYQ-205',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: approved,
            additionalRequirements: [yesAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 3,
            monthStarted: '07',
            yearStarted: '2013',
            monthAwarded: '08',
            yearAwarded: '2014',
            selectedLevel: 0,
            selectedAwardingOrganisation: 'CACHE Council for Awards in Care Health and Education',
            noOfMatchingQualifications: 76,
            qualificationToSelect: 'EYQ-311',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved,
            additionalRequirements: [yesAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 4,
            monthStarted: '07',
            yearStarted: '2020',
            monthAwarded: '12',
            yearAwarded: '2021',
            selectedLevel: 6,
            selectedAwardingOrganisation: 'Bath Spa University',
            noOfMatchingQualifications: 6,
            qualificationToSelect: 'EYQ-281',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: approved,
            ratioForLevel6: approved,
            additionalRequirements: [yesAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 5,
            monthStarted: '09',
            yearStarted: '2019',
            monthAwarded: '07',
            yearAwarded: '2021',
            selectedLevel: 2,
            selectedAwardingOrganisation: 'NCFE',
            noOfMatchingQualifications: 2,
            qualificationToSelect: 'EYQ-216',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved,
            additionalRequirements: [noAtQuestionOne]
        } as Scenario,
        {
            scenarioId: 6,
            monthStarted: '09',
            yearStarted: '2016',
            monthAwarded: '07',
            yearAwarded: '2018',
            selectedLevel: 3,
            selectedAwardingOrganisation: 'NCFE',
            noOfMatchingQualifications: 8,
            qualificationToSelect: 'EYQ-224',
            ratioForUnqualified: approved,
            ratioForLevel2: approved,
            ratioForLevel3: possibleRouteAvailable,
            ratioForLevel6: notApproved,
            additionalRequirements: [noAtQuestionOne]
        } as Scenario
    ].forEach((scenario) => {
        test(`Selected AO qualification with additional questions check for scenario ${scenario.scenarioId} and qualificationId ${scenario.qualificationToSelect}`, async ({
                                                                                                                                                                               page,
                                                                                                                                                                               context
                                                                                                                                                                           }) => {
            await whereWasTheQualificationAwarded(page, "#england");
            await whenWasQualificationStarted(page, scenario.monthStarted, scenario.yearStarted, scenario.monthAwarded, scenario.yearAwarded);
            await whatLevelIsTheQualification(page, scenario.selectedLevel);
            await whatIsTheAwardingOrganisationValue(page, scenario.selectedAwardingOrganisation);
            await checkYourAnswersPage(page);
            await checkNumberOfMatchingQualifications(page, scenario.noOfMatchingQualifications);
            await selectQualification(page, scenario.qualificationToSelect);
            await confirmQualification(page, "#yes");
            for (const x of scenario.additionalRequirements) {
                await processAdditionalRequirement(page, scenario.qualificationToSelect, x.index, x.answer);
            }
            await confirmAdditonalRequirementsAnswers(page, scenario.qualificationToSelect);
            await checkDetailsPage(page, scenario.qualificationToSelect);
            await checkText(page, '#ratio-Unqualified-tag > .govuk-tag', scenario.ratioForUnqualified);
            await checkText(page, '#ratio-Level2-tag > .govuk-tag', scenario.ratioForLevel2);
            await checkText(page, '#ratio-Level3-tag > .govuk-tag', scenario.ratioForLevel3);
            await checkText(page, '#ratio-Level6-tag > .govuk-tag', scenario.ratioForLevel6);
        });
    });
});