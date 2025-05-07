using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockQualificationsRepository : IQualificationsRepository
{
    public async Task<Qualification?> GetById(string qualificationId)
    {
        return qualificationId.ToLower() switch
               {
                   "eyq-250" => await Task.FromResult(CreateQualification("EYQ-250", "BTEC",
                                                                          AwardingOrganisations.Various, 3)),
                   "eyq-108" => await Task.FromResult(CreateQtsQualification("EYQ-108", "BTEC",
                                                                             AwardingOrganisations.Various, 6)),
                   "eyq-115" => await Task.FromResult(CreateQualification("EYQ-115", "NCFE",
                                                                          AwardingOrganisations.Various, 3, false)),
                   "eyq-114" => await Task.FromResult(CreateLevel2FurtherActionRequiredQualification("EYQ-114",
                                                           "Level 2 Further Action Qualification",
                                                           AwardingOrganisations.Ncfe, 3)),

                   "eyq-241" => await Task.FromResult(CreateQualification("EYQ-241", "BTEC",
                                                                          AwardingOrganisations.Various, 2)),
                   _ => await Task.FromResult(CreateQualification("EYQ-240",
                                                                  "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)",
                                                                  AwardingOrganisations.Ncfe, 3))
               };
    }

    public Task<List<Qualification>> Get()
    {
        return Task.FromResult(new List<Qualification>
                               {
                                   new("1", "TEST",
                                       "A awarding organisation", 123),
                                   new("2", "TEST",
                                       "B awarding organisation", 123),
                                   new("3", "TEST",
                                       "C awarding organisation", 123),
                                   new("4", "TEST",
                                       "D awarding organisation", 123),
                                   new("5", "TEST with additional requirements",
                                       "E awarding organisation", 123)
                                   {
                                       AdditionalRequirements = "Additional requirements",
                                       AdditionalRequirementQuestions =
                                       [
                                           new AdditionalRequirementQuestion
                                           {
                                               Question =
                                                   "Answer 'yes' for this to be full and relevant",
                                               AnswerToBeFullAndRelevant = true,
                                               Answers =
                                               [
                                                   new Option
                                                   {
                                                       Label = "Yes",
                                                       Value = "yes"
                                                   },

                                                   new Option
                                                   {
                                                       Label = "No",
                                                       Value = "no"
                                                   }
                                               ]
                                           }
                                       ],
                                       QualificationNumber = "Q/22/2427"
                                   }
                               });
    }

    public Task<List<Qualification>> Get(int? level, int? startDateMonth, int? startDateYear,
                                         string? awardingOrganisation, string? qualificationName)
    {
        const string startDate = "Sep-14";
        const string endDate = "Aug-19";

        var qualifications =
            new List<Qualification>
            {
                CreateQualification("EYQ-100", AwardingOrganisations.Cache, 2, null, endDate),
                CreateQualification("EYQ-101", AwardingOrganisations.Ncfe, 2, startDate, endDate),
                CreateQualification("EYQ-240", AwardingOrganisations.Pearson, 3, startDate, endDate),
                CreateQualification("EYQ-103", AwardingOrganisations.Ncfe, 3, startDate, endDate),
                CreateQualification("EYQ-104", "City & Guilds", 4, startDate, endDate),
                CreateQualification("EYQ-105", "Montessori Centre International", 4, startDate, endDate),
                CreateQualification("EYQ-106", AwardingOrganisations.Various, 5, startDate, endDate),
                CreateQualification("EYQ-107", AwardingOrganisations.Edexcel, 5, startDate, endDate),
                CreateQualification("EYQ-108", "Kent Sussex Montessori Centre", 6, startDate, endDate),
                CreateQualification("EYQ-109", "NNEB National Nursery Examination Board", 6, startDate, endDate),
                CreateQualification("EYQ-110", AwardingOrganisations.Various, 7, startDate, endDate),
                CreateQualification("EYQ-111", "City & Guilds", 7, startDate, endDate),
                CreateQualification("EYQ-112", AwardingOrganisations.Pearson, 8, startDate, endDate),
                CreateQualification("EYQ-113", AwardingOrganisations.Cache, 8, startDate, endDate),
                CreateQualification("EYQ-114", "BA (Hons) Childhood Studies", AwardingOrganisations.Edexcel, 6),
                CreateQualification("EYQ-115", "BA (Hons) Childhood Studies", AwardingOrganisations.Ncfe, 6),
                CreateQualificationWithAdditionalRequirements("EYQ-909", AwardingOrganisations.Ncfe, 3, startDate,
                                                              endDate)
            };

        // For now, inbound parameters startDateMonth and startDateYear are ignored
        return Task.FromResult(qualifications.Where(x => x.QualificationLevel == level).ToList());
    }

    private static Qualification CreateQualificationWithAdditionalRequirements(
        string qualificationId,
        string awardingOrganisation,
        int level,
        string? startDate,
        string endDate)
    {
        return new Qualification(qualificationId,
                                 $"{qualificationId}-test",
                                 awardingOrganisation,
                                 level)
               {
                   FromWhichYear = startDate,
                   ToWhichYear = endDate,
                   QualificationNumber = "ghi/456/123",
                   AdditionalRequirements = "Additional requirements",
                   AdditionalRequirementQuestions =
                   [
                       new AdditionalRequirementQuestion
                       {
                           Question =
                               "Answer 'yes' for this to be full and relevant",
                           AnswerToBeFullAndRelevant = true,
                           Answers =
                           [
                               new Option
                               {
                                   Label = "Yes",
                                   Value = "yes"
                               },

                               new Option
                               {
                                   Label = "No",
                                   Value = "no"
                               }
                           ]
                       }
                   ]
               };
    }

    private static Qualification CreateQualification(string qualificationId, string awardingOrganisation,
                                                     int level, string? startDate, string endDate)
    {
        return new Qualification(qualificationId,
                                 $"{qualificationId}-test",
                                 awardingOrganisation,
                                 level)
               {
                   FromWhichYear = startDate,
                   ToWhichYear = endDate,
                   QualificationNumber = "ghi/456/951",
                   AdditionalRequirements = "additional requirements"
               };
    }

    private static Qualification CreateQualification(string qualificationId, string qualificationName,
                                                     string awardingOrganisation, int qualificationLevel,
                                                     bool includeAdditionalRequirementQuestions = true)
    {
        var additionalRequirementQuestions = includeAdditionalRequirementQuestions
                                                 ? new List<AdditionalRequirementQuestion>
                                                   {
                                                       new()
                                                       {
                                                           Question = "Test question",
                                                           HintText =
                                                               "This is the hint text: answer yes for full and relevant",
                                                           DetailsHeading =
                                                               "This is the details heading",
                                                           DetailsContent =
                                                               ContentfulContentHelper
                                                                   .Paragraph("This is the details content"),
                                                           Answers =
                                                           [
                                                               new Option
                                                               {
                                                                   Label = "Yes",
                                                                   Value = "yes"
                                                               },

                                                               new Option
                                                               {
                                                                   Label = "No",
                                                                   Value = "no"
                                                               }
                                                           ],
                                                           ConfirmationStatement =
                                                               "This is the confirmation statement 1",
                                                           AnswerToBeFullAndRelevant = true
                                                       },
                                                       CreateSecondAdditionalRequirementQuestion(false)
                                                   }
                                                 : null;

        return new Qualification(qualificationId,
                                 qualificationName,
                                 awardingOrganisation,
                                 qualificationLevel)
               {
                   FromWhichYear = "2020",
                   ToWhichYear = "2021",
                   QualificationNumber = "603/5829/4",
                   AdditionalRequirements =
                       "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change.",
                   AdditionalRequirementQuestions = additionalRequirementQuestions,
                   RatioRequirements =
                   [
                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level2RatioRequirementName,
                           FullAndRelevantForLevel3After2014 = true,
                           RequirementForLevel2BetweenSept14AndAug19 =
                               ContentfulContentHelper.Paragraph("Level 2 further action required text")
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level3RatioRequirementName,
                           FullAndRelevantForLevel3After2014 = true
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName = RatioRequirements
                               .Level6RatioRequirementName
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .UnqualifiedRatioRequirementName,
                           FullAndRelevantForLevel3After2014 = true
                       }
                   ]
               };
    }

    private static Qualification CreateQtsQualification(string qualificationId, string qualificationName,
                                                        string awardingOrganisation, int qualificationLevel)
    {
        return new Qualification(qualificationId,
                                 qualificationName,
                                 awardingOrganisation,
                                 qualificationLevel)
               {
                   FromWhichYear = "2020",
                   ToWhichYear = "2021",
                   QualificationNumber = "603/5829/4",
                   AdditionalRequirements =
                       "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change.",
                   AdditionalRequirementQuestions =
                   [
                       new AdditionalRequirementQuestion
                       {
                           Sys = new SystemProperties
                                 { Id = AdditionalRequirementQuestions.QtsQuestion },
                           Question = "This is the Qts question",
                           HintText =
                               "This is the hint text: answer yes for full and relevant",
                           DetailsHeading =
                               "Qts question heading",
                           DetailsContent =
                               ContentfulContentHelper
                                   .Paragraph("Qts question content"),
                           Answers =
                           [
                               new Option
                               {
                                   Label = "Yes",
                                   Value = "yes"
                               },

                               new Option
                               {
                                   Label = "No",
                                   Value = "no"
                               }
                           ],
                           ConfirmationStatement =
                               "This is the confirmation statement 1",
                           AnswerToBeFullAndRelevant = true
                       },
                       CreateSecondAdditionalRequirementQuestion(true)
                   ],
                   RatioRequirements =
                   [
                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level2RatioRequirementName,
                           FullAndRelevantForQtsEtcAfter2014 = true,
                           FullAndRelevantForLevel6After2014 = true
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level3RatioRequirementName,
                           FullAndRelevantForQtsEtcAfter2014 = true,
                           FullAndRelevantForLevel6After2014 = true
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName = RatioRequirements
                               .Level6RatioRequirementName,
                           FullAndRelevantForQtsEtcAfter2014 = true,
                           FullAndRelevantForLevel6After2014 = false
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .UnqualifiedRatioRequirementName,
                           FullAndRelevantForQtsEtcAfter2014 = true,
                           FullAndRelevantForLevel6After2014 = true
                       }
                   ],
                   IsAutomaticallyApprovedAtLevel6 = false
               };
    }

    private static AdditionalRequirementQuestion CreateSecondAdditionalRequirementQuestion(
        bool answerToBeFullAndRelevant)
    {
        return new AdditionalRequirementQuestion
               {
                   Question = "Test question 2",
                   HintText =
                       "This is the hint text: answer no for full and relevant",
                   DetailsHeading =
                       "This is the details heading",
                   DetailsContent =
                       ContentfulContentHelper
                           .Paragraph("This is the details content"),
                   Answers =
                   [
                       new Option
                       {
                           Label = "Yes",
                           Value = "yes"
                       },

                       new Option
                       {
                           Label = "No",
                           Value = "no"
                       }
                   ],
                   ConfirmationStatement =
                       "This is the confirmation statement 2",
                   AnswerToBeFullAndRelevant = answerToBeFullAndRelevant
               };
    }

    private static Qualification CreateLevel2FurtherActionRequiredQualification(
        string qualificationId, string qualificationName,
        string awardingOrganisation, int qualificationLevel)
    {
        return new Qualification(qualificationId,
                                 qualificationName,
                                 awardingOrganisation,
                                 qualificationLevel)
               {
                   FromWhichYear = "2014",
                   ToWhichYear = "2019",
                   QualificationNumber = "603/5829/5",
                   RatioRequirements =
                   [
                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level2RatioRequirementName,
                           FullAndRelevantForLevel2After2014 = false,
                           FullAndRelevantForLevel3After2014 = false,
                           FullAndRelevantForLevel4After2014 = false,
                           FullAndRelevantForLevel5After2014 = false,
                           FullAndRelevantForLevel6After2014 = false,
                           FullAndRelevantForLevel7After2014 = false,
                           RequirementForLevel2BetweenSept14AndAug19 =
                               ContentfulContentHelper.Paragraph("Level 2 further action required text")
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .Level3RatioRequirementName,
                           FullAndRelevantForLevel2After2014 = false,
                           FullAndRelevantForLevel3After2014 = false,
                           FullAndRelevantForLevel4After2014 = false,
                           FullAndRelevantForLevel5After2014 = false,
                           FullAndRelevantForLevel6After2014 = false,
                           FullAndRelevantForLevel7After2014 = false
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName = RatioRequirements
                               .Level6RatioRequirementName,
                           FullAndRelevantForLevel2After2014 = false,
                           FullAndRelevantForLevel3After2014 = false,
                           FullAndRelevantForLevel4After2014 = false,
                           FullAndRelevantForLevel5After2014 = false,
                           FullAndRelevantForLevel6After2014 = false,
                           FullAndRelevantForLevel7After2014 = false
                       },

                       new RatioRequirement
                       {
                           RatioRequirementName =
                               RatioRequirements
                                   .UnqualifiedRatioRequirementName,
                           FullAndRelevantForLevel2After2014 = true,
                           FullAndRelevantForLevel3After2014 = true,
                           FullAndRelevantForLevel4After2014 = true,
                           FullAndRelevantForLevel5After2014 = true,
                           FullAndRelevantForLevel6After2014 = true,
                           FullAndRelevantForLevel7After2014 = true
                       }
                   ],
                   IsAutomaticallyApprovedAtLevel6 = false
               };
    }
}