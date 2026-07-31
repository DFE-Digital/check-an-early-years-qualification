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

const l2FandR = "Level 2 ratio requirement - F&R";
const l2NotFandR = "Level 2 ratio requirement - not F&R";
const l3FandR = "Level 3 ratio requirement - F&R";
const l3NotFandR = "Level 3 ratio requirement - not F&R";
const l6FandR = "Level 6 ratio requirement - F&R";
const l6NotFandR = "Level 6 ratio requirement - not F&R";
const UnqualifiedFandR = "Unqualified ratio requirement - F&R";
const UnqualifiedNotFandR = "Unqualified ratio requirement - not F&R";

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

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3FandR});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
            detailText: l2FandR
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3FandR});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
            detailText: l2FandR
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3FandR});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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

        await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {detailText: UnqualifiedNotFandR});
        await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {detailText: l2NotFandR});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.NotApproved, {detailText: l3NotFandR});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6NotFandR});
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

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, { detailText: l3FandR });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                    detailText: l3FandR
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                detailText: l3FandR
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2FandR
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                detailText: l3FandR
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2FandR
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {detailText: UnqualifiedNotFandR});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {detailText: l2NotFandR});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3NotFandR});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6NotFandR});
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

            await checkLevelRatioDetails(page, 0, "Level 6", RatioStatus.Approved, { detailText: l6FandR });
            await checkLevelRatioDetails(page, 1, "Level 3", RatioStatus.Approved, { detailText: l3FandR });
            await checkLevelRatioDetails(page, 2, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
            await checkLevelRatioDetails(page, 3, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
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

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, { detailText: l3FandR });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                    detailText: l3FandR
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, { detailText: l2FandR });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                detailText: l3FandR
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2FandR
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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
                detailText: l3FandR
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                detailText: l2FandR
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedFandR });
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6FandR});
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

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, { detailText: UnqualifiedNotFandR });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {detailText: l2NotFandR});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3NotFandR});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6NotFandR});
            });
        });
    });
});