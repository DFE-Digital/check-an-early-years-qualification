import {test} from '@playwright/test';
import {
    authorise,
    checkDetailsInset,
    checkRatiosHeading,
    goToDetailsPageOfQualification,
    checkLevelRatioDetails,
    RatioStatus
} from '../../_shared/playwrightWrapper';

const threeFourFive = [3, 4, 5];
const sixSeven = [6, 7];

const beforeSeptember2014OrOnOrAfterSeptember2019 = [
    [1, 2013],
    [8, 2014],
    [9, 2019],
    [1, 2020],
];
const betweenSeptember2014AndAugust2019 = [
    [9, 2014],
    [8, 2019]
];
const betweenSeptember2014AndMay2016 = [
    [9, 2014],
    [5, 2016]
];
const level2RequirementsHeading = "Level 2 Requirements";
const level3RequirementsHeading = "Level 3 Requirements";
const l2ContactDfe = "Level 2 further action required text";
const l2MaybePFA = "Level 2 maybe PFA";
const l2MustPFA = "Level 2 must PFA";
const l3Ebr = "Level 3 EBR";
const l3MustEnglish = "Level 3 must English";
const l3MustEnglishMaybePFA = "Level 3 must English maybe PFA";
const l3MustEnglishMustPFA = "Level 3 must English must PFA";
const l6MustQTS = "Level 6 must QTS";
const defaultRatioSummaryContent = "Summary card default content";

test.describe("A spec used to test the qualification details page ratios", {tag: "@e2e"}, () => {
    test.beforeEach(async ({context}) => {
        await authorise(context);
    });

    test('Checks level 2 F&R awarded before June 2016 sees expected result', async ({
                                                                                        page,
                                                                                        context
                                                                                    }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [5, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });


    test('Checks level 2 F&R awarded in June 2016 sees expected result', async ({
                                                                                    page,
                                                                                    context
                                                                                }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [6, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {
            detailText: l2MaybePFA
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    test('Checks level 2 F&R awarded after June 2016 sees expected result', async ({
                                                                                       page,
                                                                                       context
                                                                                   }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [1, 2013],
            awardDate: [7, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {
            detailText: l2MustPFA
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    test('Checks level 2 not F&R sees expected content', async ({
                                                                    page,
                                                                    context
                                                                }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [1, 2015],
            awardDate: [6, 2020],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await checkDetailsInset(page, "Qualification result heading", "Not full and relevant", "Not full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading", "This is not F&R");

        await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.NotApproved, {});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });


    threeFourFive.forEach((level) => {
        test(`Checks level ${level} F&R awarded before September 2014 sees expected result`, async ({
                                                                                                        page,
                                                                                                        context
                                                                                                    }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2013],
                awardDate: [8, 2014],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });


        betweenSeptember2014AndMay2016.forEach((awardDate) => {
            test(`Checks level ${level} F&R awarded between September 2014 and May 2016 sees expected result (${awardDate})`, async ({
                                                                                                                                         page,
                                                                                                                                         context
                                                                                                                                     }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2013],
                    awardDate: awardDate,
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
                await checkRatiosHeading(page, "Test ratio heading");

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    detailText: l3MustEnglish
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });

        test(`Checks level ${level} F&R awarded in June 2016 sees expected result`, async ({
                                                                                               page,
                                                                                               context
                                                                                           }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2013],
                awardDate: [6, 2016],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                detailText: l3MustEnglishMaybePFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2MaybePFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });


        test(`Checks level ${level} F&R awarded after June 2016 sees expected result`, async ({
                                                                                                  page,
                                                                                                  context
                                                                                              }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2013],
                awardDate: [7, 2016],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                detailText: l3MustEnglishMustPFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2MustPFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });

        beforeSeptember2014OrOnOrAfterSeptember2019.forEach((startDate) => {
            test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 sees expected result (${startDate})`, async ({
                                                                                                                                                             page,
                                                                                                                                                             context
                                                                                                                                                         }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: startDate,
                    awardDate: [12, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Not full and relevant", "Not full and relevant body");
                await checkRatiosHeading(page, "Test ratio heading", "This is not F&R", "This is the ratio text L3 EBR");

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });


        betweenSeptember2014AndAugust2019.forEach((startDate) => {
            test(`Checks level ${level} not F&R started between September 2014 and August 2019 sees expected result (${startDate})`, async ({
                                                                                                                                                page,
                                                                                                                                                context
                                                                                                                                            }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: startDate,
                    awardDate: [1, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Not full and relevant L3", "Not full and relevant L3 body");
                await checkRatiosHeading(page, "Test ratio heading", "This is not F&R for L3 between Sep14 & Aug19");

                await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, { detailText: l2ContactDfe });
                await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, { detailText: l3Ebr });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
    });

    sixSeven.forEach((level) => {
        test(`Checks level ${level} F&R (QTS) sees expected result`, async ({
                                                                                page,
                                                                                context
                                                                            }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2013],
                awardDate: [8, 2014],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "yes"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 6", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 1, "Level 3", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 2, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
        });

        test(`Checks level ${level} F&R (not QTS) awarded before September 2014 sees expected result`, async ({
                                                                                                                  page,
                                                                                                                  context
                                                                                                              }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2013],
                awardDate: [8, 2014],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });

        betweenSeptember2014AndMay2016.forEach((awardDate) => {
            test(`Checks level ${level} F&R (not QTS) awarded between September 2014 and May 2016 sees expected result (${awardDate})`, async ({
                                                                                                                                                   page,
                                                                                                                                                   context
                                                                                                                                               }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2012],
                    awardDate: awardDate,
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
                await checkRatiosHeading(page, "Test ratio heading");

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    detailText: l3MustEnglish
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
            });
        });

        test(`Checks level ${level} F&R (not QTS) awarded in June 2016 sees expected result`, async ({
                                                                                                         page,
                                                                                                         context
                                                                                                     }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2012],
                awardDate: [6, 2016],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                detailText: l3MustEnglishMaybePFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2MaybePFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });

        test(`Checks level ${level} F&R (not QTS) awarded after June 2016 sees expected result`, async ({
                                                                                                            page,
                                                                                                            context
                                                                                                        }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2012],
                awardDate: [7, 2016],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            }, page);

            await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
            await checkRatiosHeading(page, "Test ratio heading");

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                detailText: l3MustEnglishMustPFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2MustPFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });

        beforeSeptember2014OrOnOrAfterSeptember2019.forEach((startDate) => {
            test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 sees expected result (${startDate})`, async ({
                                                                                                                                                             page,
                                                                                                                                                             context
                                                                                                                                                         }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: startDate,
                    awardDate: [12, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Not full and relevant", "Not full and relevant body");
                await checkRatiosHeading(page, "Test ratio heading", "This is not F&R", "This is the ratio text L3 EBR");

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
            });
        });

        betweenSeptember2014AndAugust2019.forEach((startDate) => {
            test(`Checks level ${level} not F&R started between September 2014 and August 2019 sees expected result (${startDate})`, async ({
                                                                                                                                                page,
                                                                                                                                                context
                                                                                                                                            }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: startDate,
                    awardDate: [12, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkDetailsInset(page, "Qualification result heading", "Not full and relevant L3 or L6", "Not full and relevant L3 or L6 body");
                await checkRatiosHeading(page, "Test ratio heading", "This is not F&R for L3 between Sep14 & Aug19");

                await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, { detailText: l2ContactDfe });
                await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent });
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, { detailText: l3Ebr });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, { detailText: l6MustQTS });
            });
        });
    });

    test('Checks level 6 degree not approved shows EYITT content', async ({
                                                                              page,
                                                                              context
                                                                          }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [1, 2012],
            awardDate: [7, 2016],
            level: 6,
            organisation: "NCFE",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
            selectedFromList: true,
            qualificationId: 'eyq-321'
        }, page);

        await checkDetailsInset(page, "Qualification result heading", "Full and relevant", "Full and relevant body");
        await checkRatiosHeading(page, "Test ratio heading");

        await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
            detailText: l3MustEnglishMustPFA
        });
        await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
            detailText: l2MustPFA
        });
        await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: defaultRatioSummaryContent  });
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.PossibleRouteAvailable, { detailText: 'This is the EYITT content' });
    });
});