import {test} from '@playwright/test';
import {
    startJourney,
    checkText,
    setCookie,
    journeyCookieName,
    goToDetailsPageOfQualification,
    doesNotExist,
    checkLevelRatioDetails,
    RatioStatus
} from '../shared/playwrightWrapper';

const threeFourFive = [3,
   // 4, 5
];
const sixSeven = [6,
    //7
];
const onOrAfterSeptember2014 = [
    [9, 2014],
    //[10, 2014]
];
const beforeSeptember2014OrOnOrAfterSeptember2019 = [
    [1, 2013],
   // [1, 2014],
   // [8, 2014],
   // [9, 2019],
   // [1, 2020],
];
const betweenSeptember2014AndAugust2019 = [
    [9, 2014],
    //[1, 2018],
    //[8, 2019]
];
const betweenSeptember2014AndMay2016 = [
    [9, 2014],
    //[1, 2015],
    //[5, 2016]
];
const level2RequirementsHeading = "Level 2 Requirements";
const level3RequirementsHeading = "Level 3 Requirements";
const l2ContactDfe = "L2 contact DFE"; //EXISTS - Requirement For Level 2 Between Sept 14 And Aug 19
const l2MaybePFA = "L2 maybe PFA"; //NEW CONTENT
const l2MustPFA = "L2 must PFA"; //NEW CONTENT
const l3Ebr = "Level 3 EBR";
const l3MustEnglish = "L3 must English";
const l3MustEnglishMaybePFA = "L3 must English maybe PFA";
const l3MustEnglishMustPFA = "L3 must English must PFA";
const l6MustQTS = "L6 must QTS"; //EXISTS -  Requirement for Level 6/7 Before/After 2014


test.describe("A spec used to test the qualification details page ratios", () => {
    test.beforeEach(async ({page, context}) => {
        await startJourney(page, context);
        await setCookie(context, '%7B%22WhenWasQualificationStarted%22%3A%227%2F2015%22%7D', journeyCookieName);
    });

    /*
        test("Checks the qualification result inset shows correctly when full and relevant level 2", async ({
                                                                                                                page,
                                                                                                                context
                                                                                                            }) => {
    
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [10, 2019],
                awardDate: [1, 2025],
                level: 2,
                organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);
    
            await checkText(page, "#qualification-result-heading", "Qualification result heading");
            await checkText(page, "#qualification-result-message-heading", "Full and relevant");
            await checkText(page, "#qualification-result-message-body", "Full and relevant body");
        });
    
    
        test("Checks the qualification result inset shows correctly when not full and relevant level 2", async ({
                                                                                                                    page,
                                                                                                                    context
                                                                                                                }) => {
    
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [10, 2019],
                awardDate: [1, 2025],
                level: 2,
                organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            }, page);
    
            await checkText(page, "#qualification-result-heading", "Qualification result heading");
            await checkText(page, "#qualification-result-message-heading", "Not full and relevant");
            await checkText(page, "#qualification-result-message-body", "Not full and relevant body");
        });
        threeFourFive.forEach((level) => {
            test(`Checks the qualification result inset shows not full and relevant at level 3 qual started sept 2014 -> aug 2019 (level ${level})`, async ({
                                                                                                                                                                page,
                                                                                                                                                                context
                                                                                                                                                            }) => {
    
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2018],
                    awardDate: [1, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "yes"]],
                    selectedFromList: true
                }, page);
    
                await checkText(page, "#qualification-result-heading", "Qualification result heading");
                await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3");
                await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 body");
            });
        });
    
        test('Checks the qualification result inset shows not full and relevant at level 3 or level 6 qual started sept 2014 -> aug 2019 (level 6)', async ({
                                                                                                                                                                page,
                                                                                                                                                                context
                                                                                                                                                            }) => {
    
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2018],
                awardDate: [1, 2020],
                level: 6,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);
    
            await checkText(page, "#qualification-result-heading", "Qualification result heading");
            await checkText(page, "#qualification-result-message-heading", "Not full and relevant L3 or L6");
            await checkText(page, "#qualification-result-message-body", "Not full and relevant L3 or L6 body");
        });
    
        test('Checks level 2 not F&R sees not full and relevant ratio detail', async ({
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
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);
    
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
            await doesNotExist(page, "#ratio-additional-info");
        });
    
        test('Checks level 2 F&R awarded before June 2016 sees no content under ratio header', async ({
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
    
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
            await doesNotExist(page, "#ratio-additional-info");
        });
    
        test('Checks level 2 F&R awarded in June 2016 sees additional requirement maybe content', async ({
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
    
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text maybe requirements");
            await doesNotExist(page, "#ratio-additional-info");
        });
    
        test('Checks level 2 F&R awarded after June 2016 sees additional requirement will content', async ({
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
    
            await checkText(page, "#ratio-heading", "Test ratio heading");
            await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text requirements");
            await doesNotExist(page, "#ratio-additional-info");
        });
        threeFourFive.forEach((level) => {
            test(`Checks level ${level} F&R awarded before September 2014 sees no content under ratio header`, async ({
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
    
                await checkText(page, "#ratio-heading", "Test ratio heading");
                await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
                await doesNotExist(page, "#ratio-additional-info");
            });
            onOrAfterSeptember2014.forEach((awardDate) => {
            test(`Checks level ${level} F&R awarded on or after September 2014 sees additional requirement will content (${awardDate})`, async ({
                                                                                                                                     page,
                                                                                                                                     context
                                                                                                                                 }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2013],
                    awardDate: awardDate,
                    level: level,
                    organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);
    
                await checkText(page, "#ratio-heading", "Test ratio heading");
                await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text requirements");
                await doesNotExist(page, "#ratio-additional-info");
            });
            });
        });
    
        sixSeven.forEach((level) => {
            test(`Checks level ${level} F&R (all levels) sees no content under ratio header`, async ({
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
    
                await checkText(page, "#ratio-heading", "Test ratio heading");
                await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
                await doesNotExist(page, "#ratio-additional-info");
            });
    
    
            test(`Checks level ${level} F&R (all but L6) awarded before September 2014 sees no content under ratio header`, async ({
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
    
                await checkText(page, "#ratio-heading", "Test ratio heading");
                await doesNotExist(page, "#ratio-heading + p[class='govuk-body']");
                await doesNotExist(page, "#ratio-additional-info");
            });
    
            onOrAfterSeptember2014.forEach((awardDate) => {
                test(`Checks level ${level} F&R (all but L6) awarded on or after September 2014 sees additional requirement will content (${awardDate})}`, async ({
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
                        additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                        selectedFromList: true
                    }, page);
    
                    await checkText(page, "#ratio-heading", "Test ratio heading");
                    await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is the ratio text requirements");
                    await doesNotExist(page, "#ratio-additional-info");
                });
            });
        });
    
       beforeSeptember2014OrOnOrAfterSeptember2019.forEach((startDate) => {
            threeFourFive.forEach((level) => {
                test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text (${startDate})`, async ({
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
    
                    await checkText(page, "#ratio-heading", "Test ratio heading");
                    await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
                    await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
                });
            });
            sixSeven.forEach((level) => {
                test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 sees not F&R ratios text with L3 EBR text (${startDate})`, async ({
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
    
                    await checkText(page, "#ratio-heading", "Test ratio heading");
                    await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R");
                    await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
                });
            });
        });
    
        betweenSeptember2014AndAugust2019.forEach((startDate) => {
            threeFourFive.forEach((level) => {
                test(`Checks level ${level} not F&R started between September 2014 and August 2019 sees correct content (${startDate})`, async ({
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
    
                    await checkText(page, "#ratio-heading", "Test ratio heading");
                    await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
                    await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
                });
            });
    
            sixSeven.forEach((level) => {
                test(`(${startDate}) Checks level ${level} not F&R started between September 2014 and August 2019 sees correct content`, async ({
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
    
                    await checkText(page, "#ratio-heading", "Test ratio heading");
                    await checkText(page, "#ratio-heading + p[class='govuk-body']", "This is not F&R for L3 between Sep14 & Aug19");
                    await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
                });
            });
        });
    
        test('Checks that level 2 F&R sees EBR ratio details (no paragraph)', async ({
                                                                                         page,
                                                                                         context
                                                                                     }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [10, 2019],
                awardDate: [1, 2025],
                level: 2,
                organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);
    
            await doesNotExist(page, "#ratio-additional-info");
    
            await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });
    
        test('Checks that level 2 not F&R sees no EBR ratio details', async ({
                                                                                 page,
                                                                                 context
                                                                             }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [10, 2019],
                awardDate: [1, 2025],
                level: 2,
                organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                selectedFromList: true
            }, page);
    
            await doesNotExist(page, "#ratio-additional-info");
    
            await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
            await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.NotApproved, {});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });
    
        threeFourFive.forEach((level) => {
            test(`Checks that level ${level} F&R sees no EBR ratio details`, async ({
                                                                                        page,
                                                                                        context
                                                                                    }) => {
    
    
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2014],
                    awardDate: [1, 2015],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);
    
                await doesNotExist(page, "#ratio-additional-info");
    
                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
        sixSeven.forEach((level) => {
            test(`Checks that level ${level} F&R sees no EBR ratio details`, async ({
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
    
                await doesNotExist(page, "#ratio-additional-info");
    
                await checkLevelRatioDetails(page, 0, "Level 6", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 3", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 2, "Level 2", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Unqualified", RatioStatus.Approved, {});
            });
        });
    
        threeFourFive.forEach((level) => {
            test(`Checks that level ${level} not F&R sees EBR ratio details (has paragraph)`, async ({
                                                                                                         page,
                                                                                                         context
                                                                                                     }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2014],
                    awardDate: [1, 2015],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);
    
                await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
    
                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
    
        sixSeven.forEach((level) => {
            test(`Checks that level ${level} not F&R sees EBR ratio details (has paragraph)`, async ({
                                                                                                         page,
                                                                                                         context
                                                                                                     }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2013],
                    awardDate: [8, 2015],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);
    
    
                await checkText(page, "#ratio-additional-info", "This is the ratio text L3 EBR");
    
                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: ""});
            });
        });
        */

    test("Checks that level 2 F&R qualification awarded before June 2016 sees no content under any levels (except EBR under Level 3)", async ({
                                                                                                                                                  page,
                                                                                                                                                  context
                                                                                                                                              }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [5, 2014],
            awardDate: [5, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await doesNotExist(page, "#ratio-additional-info");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {});
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, {});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    test("Checks that level 2 F&R qualification awarded in June 2016 sees Level 2 maybe PFA and Level 3 EBR", async ({
                                                                                                                         page,
                                                                                                                         context
                                                                                                                     }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [10, 2014],
            awardDate: [6, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await doesNotExist(page, "#ratio-additional-info");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {
            summaryText: level2RequirementsHeading,
            detailText: l2MaybePFA
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, {});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    test("Checks that level 2 F&R qualification awarded after June 2016 sees l2MustPFA and Level 3 EBR", async ({
                                                                                                                    page,
                                                                                                                    context
                                                                                                                }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [10, 2014],
            awardDate: [7, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "yes"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await doesNotExist(page, "#ratio-additional-info");

        await checkLevelRatioDetails(page, 0, "Level 2", RatioStatus.Approved, {
            summaryText: level2RequirementsHeading,
            detailText: l2MustPFA
        });
        await checkLevelRatioDetails(page, 1, "Unqualified", RatioStatus.Approved, {});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    test("Checks that level 2 not F&R qualification see no additional info under levels", async ({
                                                                                                     page,
                                                                                                     context
                                                                                                 }) => {
        await goToDetailsPageOfQualification({
            context: context,
            location: "england",
            startDate: [10, 2014],
            awardDate: [7, 2016],
            level: 2,
            organisation: "CACHE%20Council%20for%20Awards%20in%20Care%20Health%20and%20Education",
            organisationNotOnList: false,
            searchCriteria: '',
            additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
            selectedFromList: true
        }, page);

        await doesNotExist(page, "#ratio-additional-info");

        await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
        await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
        await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.NotApproved, {});
        await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
    });

    threeFourFive.forEach((level) => {
        test(`Checks level ${level} F&R awarded before September 2014 sees no additional info under levels`, async ({
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

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
        });
        betweenSeptember2014AndMay2016.forEach((awardDate) => {
            test(`Checks level ${level} F&R awarded between September 2014 and May 2016 sees l3MustEnglish (${awardDate})`, async ({
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

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    summaryText: level3RequirementsHeading,
                    detailText: l3MustEnglish
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
            test(`Checks level ${level} F&R awarded in June 2016 sees l2MaybePFA and l3MustEnglishMaybePFA (${awardDate})`, async ({
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

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    summaryText: level3RequirementsHeading,
                    detailText: l3MustEnglishMaybePFA
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                    summaryText: level2RequirementsHeading,
                    detailText: l2MaybePFA
                });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });

            test(`Checks level ${level} F&R awarded after June 2016 sees l2MustPFA and l3MustEnglishMustPFA (${awardDate})`, async ({
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

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    summaryText: level3RequirementsHeading,
                    detailText: l3MustEnglishMustPFA
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                    summaryText: level2RequirementsHeading,
                    detailText: l2MustPFA
                });
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
    });

    beforeSeptember2014OrOnOrAfterSeptember2019.forEach((startDate) => {
        threeFourFive.forEach((level) => {
            test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 see no additional info under levels except l3Ebr (${startDate})`, async ({
                                                                                                                                                                                         page,
                                                                                                                                                                                         context
                                                                                                                                                                                     }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2013],
                    awardDate: [1, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });

        sixSeven.forEach((level) => {
            test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 see no additional info under levels except l3Ebr (${startDate})`, async ({
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
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.NotApproved, {});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
    });

    betweenSeptember2014AndAugust2019.forEach((startDate) => {
        threeFourFive.forEach((level) => {
            test(`Checks level ${level} not F&R started between September 2014 and August 2019 sees l2ContactDfe and l3Ebr (${startDate})`, async ({
                                                                                                                                                       page,
                                                                                                                                                       context
                                                                                                                                                   }) => {
                await goToDetailsPageOfQualification({
                    context: context,
                    location: "england",
                    startDate: [1, 2013],
                    awardDate: [1, 2020],
                    level: level,
                    organisation: "NCFE",
                    organisationNotOnList: false,
                    searchCriteria: '',
                    additionalQuestions: [["Test%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.FurtherActionRequired, {detailText: l2ContactDfe});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });

        sixSeven.forEach((level) => {
            test(`Checks level ${level} not F&R started before September 2014 or on or after September 2019 sees l2ContactDfe and l3Ebr (${startDate})`, async ({
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
                    additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "no"]],
                    selectedFromList: true
                }, page);

                await checkLevelRatioDetails(page, 0, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.FurtherActionRequired, {detailText: l2ContactDfe});
                await checkLevelRatioDetails(page, 2, "Level 3", RatioStatus.PossibleRouteAvailable, {detailText: l3Ebr});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {});
            });
        });
    });

    sixSeven.forEach((level) => {
        test(`Checks level ${level} QTS sees approved at all levels with no additional info under levels`, async ({
                                                                                                                      page,
                                                                                                                      context
                                                                                                                  }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2016],
                awardDate: [1, 2020],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "yes"]],
                selectedFromList: true
            }, page);

            await checkLevelRatioDetails(page, 0, "Level 6", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 1, "Level 3", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 2, "Level 2", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 3, "Unqualified", RatioStatus.Approved, {});
        });

        test(`Checks level ${level} F&R (not QTS) awarded before September 2014 sees l6MustQTS`, async ({
                                                                                                            page,
                                                                                                            context
                                                                                                        }) => {
            await goToDetailsPageOfQualification({
                context: context,
                location: "england",
                startDate: [1, 2012],
                awardDate: [8, 2014],
                level: level,
                organisation: "NCFE",
                organisationNotOnList: false,
                searchCriteria: '',
                additionalQuestions: [["This%20is%20the%20Qts%20question", "no"], ["Test%20question%202", "yes"]],
                selectedFromList: true
            }, page);

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });

        betweenSeptember2014AndMay2016.forEach((awardDate) => {
            test(`Checks level ${level} F&R (not QTS) awarded between September 2014 and May 2016 sees l3MustEnglish and l6MustQTS (${awardDate})`, async ({
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

                await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                    summaryText: level3RequirementsHeading,
                    detailText: l3MustEnglish
                });
                await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
                await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
            });
        });

        test(`Checks level ${level} F&R (not QTS) awarded in June 2016 sees l3MustEnglishMaybePFA and l2MaybePFA and l6MustQTS`, async ({
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

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                summaryText: level3RequirementsHeading,
                detailText: l3MustEnglishMaybePFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                summaryText: level2RequirementsHeading,
                detailText: l2MaybePFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });

        test(`Checks level ${level} F&R (not QTS) awarded after June 2016 sees l3MustEnglishMustPFA and l2MustPFA and l6MustQTS`, async ({
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

            await checkLevelRatioDetails(page, 0, "Level 3", RatioStatus.Approved, {
                summaryText: level3RequirementsHeading,
                detailText: l3MustEnglishMustPFA
            });
            await checkLevelRatioDetails(page, 1, "Level 2", RatioStatus.Approved, {
                summaryText: level2RequirementsHeading,
                detailText: l2MustPFA
            });
            await checkLevelRatioDetails(page, 2, "Unqualified", RatioStatus.Approved, {});
            await checkLevelRatioDetails(page, 3, "Level 6", RatioStatus.NotApproved, {detailText: l6MustQTS});
        });
    });
});
