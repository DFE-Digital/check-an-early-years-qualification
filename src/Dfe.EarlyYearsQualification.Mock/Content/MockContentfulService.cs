using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulService : IContentService
{
    private const string WhereWasTheQualificationAwardedPath = "/questions/where-was-the-qualification-awarded";
    private const string WhatLevelIsTheQualificationPath = "/questions/what-level-is-the-qualification";

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        var body = ContentfulContentHelper.Paragraph("Test Accessibility Statement Body");

        return await Task.FromResult(new AccessibilityStatementPage
                                     {
                                         Heading = "Test Accessibility Statement Heading",
                                         Body = body,
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/",
                                                          OpenInNewTab = false
                                                      }
                                     });
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        var body = ContentfulContentHelper.Paragraph("Test Advice Page Body");

        return entryId switch
               {
                   AdvicePages.QualificationsAchievedOutsideTheUk =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved outside the United Kingdom",
                                                              body, WhereWasTheQualificationAwardedPath)),
                   AdvicePages.QualificationsStartedBetweenSept2014AndAug2019 =>
                       await
                           Task.FromResult(CreateAdvicePage("Level 2 qualifications started between 1 September 2014 and 31 August 2019",
                                                            body, WhatLevelIsTheQualificationPath)),

                   AdvicePages.QualificationsAchievedInScotland =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Scotland",
                                                              body, WhereWasTheQualificationAwardedPath)),

                   AdvicePages.QualificationsAchievedInWales =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Wales",
                                                              body, WhereWasTheQualificationAwardedPath)),

                   AdvicePages.QualificationsAchievedInNorthernIreland =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Northern Ireland",
                                                              body, WhereWasTheQualificationAwardedPath)),

                   AdvicePages.QualificationNotOnTheList =>
                       await Task.FromResult(CreateAdvicePage("Qualification not on the list",
                                                              body, "/qualifications")),

                   AdvicePages.QualificationLevel7 =>
                       await Task.FromResult(CreateAdvicePage("Qualification at Level 7",
                                                              body,
                                                              WhatLevelIsTheQualificationPath)),

                   AdvicePages.Level6QualificationPre2014 =>
                       await Task.FromResult(CreateAdvicePage("Level 6 qualification pre 2014",
                                                              body, WhatLevelIsTheQualificationPath)),

                   AdvicePages.Level6QualificationPost2014 =>
                       await Task.FromResult(CreateAdvicePage("Level 6 qualification post 2014",
                                                              body, WhatLevelIsTheQualificationPath)),
                   _ => null
               };
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        var body = ContentfulContentHelper.Paragraph("Test Cookies Page Body");
        var successBannerContent = ContentfulContentHelper.Paragraph("Test Success Banner Content");
        return await Task.FromResult(new CookiesPage
                                     {
                                         Heading = "Test Cookies Heading",
                                         ButtonText = "Test Cookies Button",
                                         Options =
                                         [
                                             new Option
                                             {
                                                 Label = "Test Option Label 1",
                                                 Value = "test-option-value-1"
                                             },

                                             new Option
                                             {
                                                 Label = "Test Option Label 2",
                                                 Value = "test-option-value-2"
                                             }
                                         ],
                                         ErrorText = "Test Error Text",
                                         SuccessBannerHeading = "Test Success Banner Heading",
                                         Body = body,
                                         SuccessBannerContent = successBannerContent,
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/",
                                                          OpenInNewTab = false
                                                      },
                                         FormHeading = "Test Form Heading"
                                     });
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        var checkAnotherQualificationText = ContentfulContentHelper.Paragraph("Test Check Another Qualification Text");
        var furtherInfoText = ContentfulContentHelper.Paragraph("Test Further Info Text");
        var ratioText = ContentfulContentHelper.Paragraph("This is the ratio text");
        var requirementsText = ContentfulContentHelper.Paragraph("This is the requirements text");
        return await Task.FromResult(new DetailsPage
                                     {
                                         AwardingOrgLabel = "Awarding Org Label",
                                         BookmarkHeading = "Test Bookmark Heading",
                                         BookmarkText = "Test Bookmark Text",
                                         CheckAnotherQualificationHeading = "Test Check Another Qualification Heading",
                                         DateAddedLabel = "Test Date Added Label",
                                         DateOfCheckLabel = "Test Date Of Check Label",
                                         FurtherInfoHeading = "Test Further Info Heading",
                                         LevelLabel = "Test Level Label",
                                         MainHeader = "Test Main Heading",
                                         QualificationNumberLabel = "Test Qualification Number Label",
                                         CheckAnotherQualificationText = checkAnotherQualificationText,
                                         FurtherInfoText = furtherInfoText,
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/confirm-qualification/eyq-240",
                                                          OpenInNewTab = false
                                                      },
                                         BackToAdditionalQuestionsLink = new NavigationLink
                                                                         {
                                                                             DisplayText =
                                                                                 "TEST (back to additional questions)",
                                                                             Href =
                                                                                 "/qualifications/check-additional-questions/EYQ-240",
                                                                             OpenInNewTab = false
                                                                         },
                                         BackToLevelSixAdvice = new NavigationLink
                                                                {
                                                                    DisplayText =
                                                                        "TEST (back to level 6 advice post 2014)",
                                                                    Href =
                                                                        "/advice/level-6-qualification-post-2014",
                                                                    OpenInNewTab = false
                                                                },
                                         BackToLevelSixAdviceBefore2014 = new NavigationLink
                                                                          {
                                                                              DisplayText =
                                                                                  "TEST (back to level 6 advice pre 2014)",
                                                                              Href =
                                                                                  "/advice/level-6-qualification-pre-2014",
                                                                              OpenInNewTab = false
                                                                          },
                                         RatiosHeading = "Test ratio heading",
                                         RatiosText = ratioText,
                                         RequirementsHeading = "Test requirements heading",
                                         RequirementsText = requirementsText,
                                         CheckAnotherQualificationLink = new NavigationLink
                                                                         {
                                                                             DisplayText =
                                                                                 "Check another qualification",
                                                                             Href = "/",
                                                                             OpenInNewTab = false
                                                                         },
                                         QualificationDetailsSummaryHeader = "Qualification details",
                                         QualificationNameLabel = "Qualification",
                                         QualificationStartDateLabel = "Qualification start date"
                                     });
    }

    public async Task<List<NavigationLink>> GetNavigationLinks()
    {
        return await Task.FromResult(new List<NavigationLink>
                                     {
                                         new()
                                         {
                                             DisplayText = "Privacy notice",
                                             Href = "/link-to-privacy-notice"
                                         },
                                         new()
                                         {
                                             DisplayText = "Accessibility statement",
                                             Href = "/link-to-accessibility-statement"
                                         }
                                     });
    }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        var content = new Document()
                      {
                          Content =
                          [
                              ContentfulContentHelper.ParagraphWithEmbeddedLink("Some Text", "Link Text",
                                                                                "LinkHref")
                          ]
                      };

        return await Task.FromResult(new PhaseBanner
                                     {
                                         Content = content,
                                         PhaseName = "Test phase banner name",
                                         Show = true
                                     });
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        return qualificationId.ToLower() switch
               {
                   "eyq-250" => await Task.FromResult(CreateQualification("EYQ-250", "BTEC",
                                                                          AwardingOrganisations.Various)),
                   _ => await Task.FromResult(CreateQualification("EYQ-240",
                                                                  "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)",
                                                                  AwardingOrganisations.Ncfe))
               };
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        return entryId switch
               {
                   QuestionPages.WhatLevelIsTheQualification =>
                       await Task.FromResult(CreateWhatLevelIsTheQualificationPage()),
                   QuestionPages.WhereWasTheQualificationAwarded =>
                       await Task.FromResult(CreateWhereWasTheQualificationAwardedPage()),
                   _ => throw new NotImplementedException($"No radio question page mock for entry {entryId}")
               };
    }

    public async Task<DateQuestionPage?> GetDateQuestionPage(string entryId)
    {
        return entryId switch
               {
                   QuestionPages.WhenWasTheQualificationStarted =>
                       await Task.FromResult(CreateDateQuestionPage()),
                   _ => throw new NotImplementedException($"No date question page mock for entry {entryId}")
               };
    }

    public async Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId)
    {
        return entryId switch
               {
                   QuestionPages.WhatIsTheAwardingOrganisation =>
                       await Task.FromResult(CreateDropdownPage()),
                   _ => throw new NotImplementedException($"No dropdown question page mock for entry {entryId}")
               };
    }

    public Task<List<Qualification>> GetQualifications()
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
                                       AdditionalRequirementQuestions = new List<AdditionalRequirementQuestion>
                                                                        {
                                                                            new()
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
                                                                        },
                                       QualificationNumber = "Q/22/2427"
                                   }
                               });
    }

    public async Task<QualificationListPage?> GetQualificationListPage()
    {
        return await Task.FromResult(new QualificationListPage
                                     {
                                         Header = "Test Header",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/questions/what-is-the-awarding-organisation",
                                                          OpenInNewTab = false
                                                      },
                                         LevelHeading = "Level",
                                         AwardingOrganisationHeading = "Awarding organisation",
                                         SearchButtonText = "Refine",
                                         SearchCriteriaHeading = "Your search",
                                         MultipleQualificationsFoundText = "qualifications found",
                                         SingleQualificationFoundText = "qualification found",
                                         PreSearchBoxContent =
                                             ContentfulContentHelper.Text("Pre search box content"),
                                         PostQualificationListContent =
                                             ContentfulContentHelper.Link("Link to not on list advice page",
                                                                          "/advice/qualification-not-on-the-list"),
                                         PostSearchCriteriaContent =
                                             ContentfulContentHelper.Text("Post search criteria content"),
                                         AnyLevelHeading = "Any level",
                                         AnyAwardingOrganisationHeading = "Various awarding organisations",
                                         NoResultsText = ContentfulContentHelper.ParagraphWithBold("Test no qualifications text"),
                                         ClearSearchText = "Clear search",
                                         NoQualificationsFoundText = "No"
                                     });
    }

    public async Task<ConfirmQualificationPage?> GetConfirmQualificationPage()
    {
        return await Task.FromResult(new ConfirmQualificationPage
                                     {
                                         QualificationLabel = "Test qualification label",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Test back button",
                                                          OpenInNewTab = false,
                                                          Href = "/qualifications"
                                                      },
                                         ErrorText = "Test error text",
                                         ButtonText = "Test button text",
                                         LevelLabel = "Test level label",
                                         DateAddedLabel = "Test date added label",
                                         Heading = "Test heading",
                                         PostHeadingContent = ContentfulContentHelper.Paragraph("The post heading content"),
                                         Options =
                                         [
                                             new Option
                                             {
                                                 Label = "yes",
                                                 Value = "yes"
                                             },
                                             new Option
                                             {
                                                 Label = "no",
                                                 Value = "no"
                                             }
                                         ],
                                         RadioHeading = "Test radio heading",
                                         AwardingOrganisationLabel = "Test awarding organisation label",
                                         ErrorBannerHeading = "Test error banner heading",
                                         ErrorBannerLink = "Test error banner link",
                                         VariousAwardingOrganisationsExplanation = ContentfulContentHelper.Paragraph("Various awarding organisation explanation text")
                                     });
    }

    public async Task<CheckAdditionalRequirementsPage?> GetCheckAdditionalRequirementsPage()
    {
        return await Task.FromResult(new CheckAdditionalRequirementsPage
                                     {
                                         Heading = "Check the additional requirements",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Back",
                                                          Href = "/qualifications",
                                                          OpenInNewTab = false
                                                      },
                                         CtaButtonText = "Get result",
                                         AwardingOrganisationLabel = "Awarding organisation",
                                         QualificationLabel = "Qualification",
                                         QualificationLevelLabel = "Qualification level",
                                         InformationMessage =
                                             "Your result is dependent on the accuracy of the answers you have provided",
                                         ErrorMessage = "This is a test error message",
                                         ErrorSummaryHeading = "There was a problem",
                                         QuestionSectionHeading = "This is the question section heading"
                                     });
    }

    public Task<ChallengePage?> GetChallengePage()
    {
        throw new NotImplementedException();
    }

    public async Task<StartPage?> GetStartPage()
    {
        var preCtaButtonContent =
            ContentfulContentHelper.Paragraph("This is the pre cta content");

        var postCtaButtonContent =
            ContentfulContentHelper.Paragraph("This is the post cta content");

        var rightHandSideContent =
            ContentfulContentHelper.Paragraph("This is the right hand content");

        return await Task.FromResult(new StartPage
                                     {
                                         Header = "Test Header",
                                         PreCtaButtonContent = preCtaButtonContent,
                                         CtaButtonText = "Start Button Text",
                                         PostCtaButtonContent = postCtaButtonContent,
                                         RightHandSideContentHeader = "Related content",
                                         RightHandSideContent = rightHandSideContent
                                     });
    }

    public async Task<CookiesBanner?> GetCookiesBannerContent()
    {
        var acceptedCookiesContent =
            ContentfulContentHelper.Paragraph("This is the accepted cookie content");

        var cookiesBannerContent =
            ContentfulContentHelper.Paragraph("This is the cookies banner content");

        var rejectedCookieContent =
            ContentfulContentHelper.Paragraph("This is the rejected cookie content");

        return await Task.FromResult(new CookiesBanner
                                     {
                                         AcceptButtonText = "Test Accept Button Text",
                                         AcceptedCookiesContent = acceptedCookiesContent,
                                         CookiesBannerContent = cookiesBannerContent,
                                         CookiesBannerTitle = "Test Cookies Banner Title",
                                         CookiesBannerLinkText = "Test Cookies Banner Text",
                                         HideCookieBannerButtonText = "Test Hide Cookie Banner Button Text",
                                         RejectButtonText = "Test Reject Button Text",
                                         RejectedCookiesContent = rejectedCookieContent
                                     });
    }

    private static RadioQuestionPage CreateWhereWasTheQualificationAwardedPage()
    {
        var options = new List<IOptionItem>
                      {
                          new Option
                          {
                              Label = "England", Value = "england"
                          },
                          new Option
                          {
                              Label = "Scotland", Value = "scotland"
                          },
                          new Option
                          {
                              Label = "Wales", Value = "wales"
                          },
                          new Option
                          {
                              Label = "Northern Ireland", Value = "northern-ireland"
                          },
                          new Divider
                          {
                              Text = "or"
                          },
                          new Option
                          {
                              Label = "Outside the United Kingdom",
                              Value = "outside-uk"
                          }
                      };
        return CreateRadioQuestionPage("Where was the qualification awarded?", options, "/");
    }

    private static RadioQuestionPage CreateWhatLevelIsTheQualificationPage()
    {
        var options = new List<IOptionItem>
                      {
                          new Option
                          {
                              Label = "Level 2", Value = "2"
                          },
                          new Option
                          {
                              Label = "Level 3", Value = "3"
                          },
                          new Option
                          {
                              Label = "Level 6", Value = "6", Hint = "Some hint text"
                          },
                          new Option
                          {
                              Label = "Level 7", Value = "7"
                          }
                      };
        return CreateRadioQuestionPage("What level is the qualification?", options,
                                       "/questions/when-was-the-qualification-started");
    }

    private static RadioQuestionPage CreateRadioQuestionPage(string question, List<IOptionItem> options,
                                                             string backButtonUrl)
    {
        return new RadioQuestionPage
               {
                   Question = question,
                   Options = options,
                   CtaButtonText = "Continue",
                   ErrorMessage = "Test error message",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = backButtonUrl,
                                    OpenInNewTab = false
                                },
                   ErrorBannerHeading = "There is a problem",
                   ErrorBannerLinkText = "Test error banner link text",
                   AdditionalInformationBody =
                       ContentfulContentHelper.Paragraph("This is the additional information body"),
                   AdditionalInformationHeader = "This is the additional information header"
               };
    }

    private static DateQuestionPage CreateDateQuestionPage()
    {
        return new DateQuestionPage
               {
                   Question = "Test Date Question",
                   CtaButtonText = "Continue",
                   MonthLabel = "Test Month Label",
                   YearLabel = "Test Year Label",
                   QuestionHint = "Test Question Hint",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = WhereWasTheQualificationAwardedPath,
                                    OpenInNewTab = false
                                },
                   AdditionalInformationBody =
                       ContentfulContentHelper.Paragraph("This is the additional information body"),
                   AdditionalInformationHeader = "This is the additional information header",
                   ErrorBannerHeading = "There is a problem",
                   ErrorBannerLinkText = "Test error banner link text",
                   ErrorMessage = "Test Error Message",
                   FutureDateErrorBannerLinkText = "Future date error message banner link",
                   FutureDateErrorMessage = "Future date error message",
                   IncorrectFormatErrorBannerLinkText = "Enter a month between 1 and 12 and a year between 1900 and $[actual-year]$",
                   IncorrectFormatErrorMessage = "Incorrect format error message banner link"
               };
    }

    private static DropdownQuestionPage CreateDropdownPage()
    {
        return new DropdownQuestionPage
               {
                   Question = "Test Dropdown Question",
                   ErrorMessage = "Test Error Message",
                   CtaButtonText = "Test Button Text",
                   DefaultText = "Test Default Dropdown Text",
                   DropdownHeading = "Test Dropdown Heading",
                   NotInListText = "Test Not In The List",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = "/questions/what-level-is-the-qualification",
                                    OpenInNewTab = false
                                },
                   ErrorBannerHeading = "There is a problem",
                   ErrorBannerLinkText = "Test error banner link text",
                   AdditionalInformationBody =
                       ContentfulContentHelper.Paragraph("This is the additional information body"),
                   AdditionalInformationHeader = "This is the additional information header"
               };
    }

    private static AdvicePage CreateAdvicePage(string heading, Document body, string backButtonUrl)
    {
        return new AdvicePage
               {
                   Body = body,
                   Heading = heading,
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = backButtonUrl,
                                    OpenInNewTab = false
                                }
               };
    }

    private static Qualification CreateQualification(string qualificationId, string qualificationName,
                                                     string awardingOrganisation)
    {
        return new Qualification(qualificationId,
                                 qualificationName,
                                 awardingOrganisation,
                          3)
        {
            FromWhichYear = "2020",
            ToWhichYear = "2021",
            QualificationNumber = "603/5829/4",
            AdditionalRequirements =
                "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change.",
            AdditionalRequirementQuestions =
                new List<AdditionalRequirementQuestion>
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
                    new()
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
                        AnswerToBeFullAndRelevant = false
                    }
                },
            RatioRequirements = new List<RatioRequirement>
                                {
                                    new()
                                    {
                                        RatioRequirementName =
                                            RatioRequirements
                                                .Level2RatioRequirementName,
                                        FullAndRelevantForLevel3After2014 = true
                                    },
                                    new()
                                    {
                                        RatioRequirementName =
                                            RatioRequirements
                                                .Level3RatioRequirementName,
                                        FullAndRelevantForLevel3After2014 = true
                                    },
                                    new()
                                    {
                                        RatioRequirementName = RatioRequirements
                                            .Level6RatioRequirementName
                                    },
                                    new()
                                    {
                                        RatioRequirementName =
                                            RatioRequirements
                                                .UnqualifiedRatioRequirementName,
                                        FullAndRelevantForLevel3After2014 = true
                                    }
                                }
        };
        
    }
}