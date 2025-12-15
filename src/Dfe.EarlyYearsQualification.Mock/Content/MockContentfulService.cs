using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using File = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulService : IContentService
{
    private const string WhereWasTheQualificationAwardedPath = "/questions/where-was-the-qualification-awarded";
    private const string WhatLevelIsTheQualificationPath = "/questions/what-level-is-the-qualification";
    private const string QualificationsPath = "/select-a-qualification-to-check";
    private const string HomePath = "/";
    private const string ThereIsAProblem = "There is a problem";
    private const string CtaButtonText = "Continue";

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
                                                              body, WhereWasTheQualificationAwardedPath, true)),
                   AdvicePages.QualificationsStartedBetweenSept2014AndAug2019 =>
                       await
                           Task.FromResult(CreateAdvicePage("Level 2 qualifications started between 1 September 2014 and 31 August 2019",
                                                            body, WhatLevelIsTheQualificationPath, false)),

                   AdvicePages.QualificationsAchievedInScotland =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Scotland",
                                                              body, WhereWasTheQualificationAwardedPath, true)),

                   AdvicePages.QualificationsAchievedInWales =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Wales",
                                                              body, WhereWasTheQualificationAwardedPath, true)),

                   AdvicePages.QualificationsAchievedInNorthernIreland =>
                       await Task.FromResult(CreateAdvicePage("Qualifications achieved in Northern Ireland",
                                                              body, WhereWasTheQualificationAwardedPath, true)),

                   AdvicePages.QualificationNotOnTheList =>
                       await Task.FromResult(CreateAdvicePage("Qualification not on the list",
                                                              body, QualificationsPath, true)),

                   AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019 =>
                       await
                           Task.FromResult(CreateAdvicePage("Level 7 qualifications started between 1 September 2014 and 31 August 2019",
                                                            body, WhatLevelIsTheQualificationPath, false)),
                   AdvicePages.Level7QualificationAfterAug2019 =>
                       await Task.FromResult(CreateAdvicePage("Level 7 qualification after aug 2019",
                                                              body, WhatLevelIsTheQualificationPath, false)),
                   AdvicePages.Help =>
                       await Task.FromResult(CreateAdvicePage("Help",
                                                              body, HomePath, false)),
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
    
    public async Task<QualificationDetailsPage?> GetQualificationDetailsPage(bool userIsCheckingOwnQualification, bool isFullAndRelevant, int level, int startMonth, int startYear, bool isDegreeSpecificPage, bool isApprovedAtL6SpecificPage)
    {
        var ratioTextForNotFullAndRelevant = ContentfulContentHelper.Paragraph("This is not F&R");
        var ratioTextL3PlusNotFullAndRelevantBetweenSep14AndAug19 =
            ContentfulContentHelper.Paragraph("This is not F&R for L3 between Sep14 & Aug19");
        var ratioTextL3Ebr = ContentfulContentHelper.Paragraph("This is the ratio text L3 EBR");
        var requirementsText = ContentfulContentHelper.Paragraph("This is the requirements text");

        var labels = new DetailsPageLabels
        {
            AwardingOrgLabel = "Awarding Org Label",
            DateOfCheckLabel = "Test Date Of Check Label",
            LevelLabel = "Test Level Label",
            MainHeader = "Test Main Heading",
            BackButton = new NavigationLink
            {
                DisplayText = "TEST",
                Href = "/confirm-qualification/eyq-240",
                OpenInNewTab = false
            },
            BackToConfirmAnswers = new NavigationLink
            {
                DisplayText = "TEST (back to additional questions)",
                Href = "/qualifications/check-additional-questions/$[qualification-id]$/confirm-answers",
                OpenInNewTab = false
            },
            RatiosHeading = "Test ratio heading",
            RatiosTextL3Ebr = ratioTextL3Ebr,
            RatiosTextNotFullAndRelevant = ratioTextForNotFullAndRelevant,
            RatiosTextL3PlusNotFrBetweenSep14Aug19 =
                                                 ratioTextL3PlusNotFullAndRelevantBetweenSep14AndAug19,
            CheckAnotherQualificationLink = new NavigationLink
            {
                DisplayText = "Check another qualification",
                Href = "/",
                OpenInNewTab = false
            },
            QualificationDetailsSummaryHeader = "Qualification details",
            QualificationNameLabel = "Qualification",
            QualificationStartDateLabel = "Qualification start date",
            QualificationAwardedDateLabel = "Qualification awarded date",
            QualificationResultHeading = "Qualification result heading",
            QualificationResultFrMessageHeading = "Full and relevant",
            QualificationResultFrMessageBody = "Full and relevant body",
            QualificationResultNotFrMessageHeading = "Not full and relevant",
            QualificationResultNotFrMessageBody = "Not full and relevant body",
            QualificationResultNotFrL3MessageHeading = "Not full and relevant L3",
            QualificationResultNotFrL3MessageBody = "Not full and relevant L3 body",
            QualificationResultNotFrL3OrL6MessageHeading = "Not full and relevant L3 or L6",
            QualificationResultNotFrL3OrL6MessageBody = "Not full and relevant L3 or L6 body",
            UpDownFeedback = GetUpDownFeedback(),
            PrintButtonText = "Print this page",
            PrintInformationBody = ContentfulContentHelper.Paragraph("Print information body"),
            PrintInformationHeading = "Print information heading",
            QualificationNumberLabel = "Qualification Number (QN)"
        };

        var qualificationDetailsPages = new List<QualificationDetailsPage>
        {
            new QualificationDetailsPage
            {
                IsPractitionerSpecificPage = false,
                IsDegreeSpecificPage = false,
                IsAutomaticallyApprovedAtLevel6 = false,
                IsFullAndRelevant = true,
                FromWhichYear = "Sep-14",
                ToWhichYear = "Aug-19",
                Level = "3",
                Name = "Test name",
                RequirementsHeading = "Test requirements heading",
                RequirementsText = requirementsText,
                Labels = labels
            },
            new QualificationDetailsPage
            {
                IsPractitionerSpecificPage = true,
                IsDegreeSpecificPage = false,
                IsAutomaticallyApprovedAtLevel6 = false,
                IsFullAndRelevant = true,
                FromWhichYear = "Sep-23",
                ToWhichYear = "Aug-24",
                Level = "3",
                Name = "Test name",
                RequirementsHeading = "This is F&R practitioner heading",
                RequirementsText = ContentfulContentHelper.Paragraph("This is F&R practitioner text"),
                Labels = labels
            },
            new QualificationDetailsPage
            {
                IsPractitionerSpecificPage = true,
                IsDegreeSpecificPage = false,
                IsAutomaticallyApprovedAtLevel6 = false,
                IsFullAndRelevant = false,
                FromWhichYear = "Sep-23",
                ToWhichYear = "Aug-24",
                Level = "3",
                Name = "Test name",
                RequirementsHeading = "This is NF&R practitioner heading",
                RequirementsText = ContentfulContentHelper.Paragraph("This is NF&R practitioner text"),
                Labels = labels
            },
            new QualificationDetailsPage
            {
                IsPractitionerSpecificPage = true,
                IsDegreeSpecificPage = false,
                IsAutomaticallyApprovedAtLevel6 = false,
                IsFullAndRelevant = true,
                FromWhichYear = "Sep-11",
                ToWhichYear = "Aug-17",
                Level = "6",
                Name = "Test name",
                RequirementsHeading = "This is NF&R practitioner heading",
                RequirementsText = ContentfulContentHelper.Paragraph("This is NF&R practitioner text"),
                Labels = labels
            }
        };

        if (userIsCheckingOwnQualification)
        {
            return await Task.FromResult(qualificationDetailsPages
                .FirstOrDefault(x => x.IsPractitionerSpecificPage == userIsCheckingOwnQualification &&
                x.IsFullAndRelevant == isFullAndRelevant && x.Level == level.ToString()));
        }

        return await Task.FromResult(qualificationDetailsPages.FirstOrDefault(x => !x.IsPractitionerSpecificPage));
    }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        var content = new Document
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

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        return entryId switch
               {
                   QuestionPages.WhatLevelIsTheQualification =>
                       await Task.FromResult(CreateWhatLevelIsTheQualificationPage()),
                   QuestionPages.WhereWasTheQualificationAwarded =>
                       await Task.FromResult(CreateWhereWasTheQualificationAwardedPage()),
                   QuestionPages.AreYouCheckingYourOwnQualification =>
                       await Task.FromResult(CreateAreYouCheckingYourOwnQualificationPage()),
                   _ => throw new NotImplementedException($"No radio question page mock for entry {entryId}")
               };
    }

    public async Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId)
    {
        return entryId switch
               {
                   QuestionPages.WhenWasTheQualificationStartedAndAwarded =>
                       await Task.FromResult(CreateDatesQuestionPage()),
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

    public async Task<QualificationListPage?> GetQualificationListPage()
    {
        return await Task.FromResult(new QualificationListPage
                                     {
                                         Header = "Test Header",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/questions/check-your-answers",
                                                          OpenInNewTab = false
                                                      },
                                         QualificationFoundPrefix = "We found",
                                         SearchButtonText = "Refine",
                                         SearchCriteriaHeading = "Your search",
                                         MultipleQualificationsFoundText = "matching qualifications",
                                         SingleQualificationFoundText = "matching qualification",
                                         PreSearchBoxContent =
                                             ContentfulContentHelper.Paragraph("Pre search box content"),
                                         Pre2014L6OrNotSureContentHeading = "Pre 2014 L6 or not sure heading",
                                         Pre2014L6OrNotSureContent = ContentfulContentHelper.Paragraph("Pre 2014 L6 or not sure content"),
                                         Post2014L6OrNotSureContentHeading = "Post 2014 L6 or not sure heading",
                                         Post2014L6OrNotSureContent = ContentfulContentHelper.Paragraph("Post 2014 L6 or not sure content"),
                                         PostQualificationListContentHeading = "Post qualification list header",
                                         PostQualificationListContent =
                                             ContentfulContentHelper.Link("Link to not on list advice page",
                                                                          "/advice/qualification-not-on-the-list"),
                                         AnyLevelHeading = "any level",
                                         AnyAwardingOrganisationHeading = "various awarding organisations",
                                         NoResultsText =
                                             ContentfulContentHelper.Paragraph("Test no qualifications text"),
                                         ClearSearchText = "Clear search",
                                         AwardedLocationPrefixText = "awarded in",
                                         StartDatePrefixText = "started in",
                                         AwardedDatePrefixText = "awarded in",
                                         LevelPrefixText = "level",
                                         AwardedByPrefixText = "awarded by",
                                         QualificationNumberLabel = "Qualification Number (QN)"
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
                                                          Href = QualificationsPath
                                                      },
                                         ErrorText = "Test error text",
                                         ButtonText = "Test button text",
                                         LevelLabel = "Test level label",
                                         DateAddedLabel = "Test date added label",
                                         Heading = "Test heading",
                                         PostHeadingContent =
                                             ContentfulContentHelper.Paragraph("The post heading content"),
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
                                         VariousAwardingOrganisationsExplanation =
                                             ContentfulContentHelper
                                                 .Paragraph("Various awarding organisation explanation text"),
                                         AnswerDisclaimerText = "Answer disclaimer text",
                                         NoAdditionalRequirementsButtonText = "Get result",
                                         QualificationNumberLabel = "Qualification Number (QN)"
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
                                                          Href = QualificationsPath,
                                                          OpenInNewTab = false
                                                      },
                                         PreviousQuestionBackButton = new NavigationLink
                                                                      {
                                                                          DisplayText = "Previous",
                                                                          Href =
                                                                              "/qualifications/check-additional-questions",
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

    public async Task<ChallengePage?> GetChallengePage()
    {
        return await Task.FromResult(new ChallengePage
                                     {
                                         ErrorHeading = "Test Error Heading",
                                         FooterContent = ContentfulContentHelper.Paragraph("Test Footer Content"),
                                         InputHeading = "Test Input Heading",
                                         MainContent = ContentfulContentHelper.Paragraph("Test Main Content"),
                                         MainHeading = "Test Main Heading",
                                         IncorrectPasswordText = "Test Incorrect Password Text",
                                         MissingPasswordText = "Test Missing Password Text",
                                         SubmitButtonText = "Test Submit Button Text",
                                         ShowPasswordButtonText = "Test Show Password Button Text"
                                     });
    }

    public Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(int level, int startMonth, int startYear, bool isUserCheckingTheirOwnQualification)
    {
        var backButton = new NavigationLink
                         {
                             DisplayText = "TEST",
                             Href = QualificationsPath,
                             OpenInNewTab = false
                         };

        var upDownFeedback = GetUpDownFeedback();

        return (level switch
                {
                    3 when isUserCheckingTheirOwnQualification => Task.FromResult(new CannotFindQualificationPage
                                         {
                                             Heading = "This is the practitioner level 3 page",
                                             Body = ContentfulContentHelper.Paragraph("This is the practitioner body text"),
                                             FromWhichYear = "Sep-14",
                                             ToWhichYear = "Aug-19",
                                             BackButton = backButton,
                                             UpDownFeedback = upDownFeedback,
                                             RightHandSideContent = GetFeedbackComponent(),
                                             IsPractitionerSpecificPage = true
                                         }),
                    3 => Task.FromResult(new CannotFindQualificationPage
                                         {
                                             Heading = "This is the level 3 page",
                                             Body = ContentfulContentHelper.Paragraph("This is the body text"),
                                             FromWhichYear = "Sep-14",
                                             ToWhichYear = "Aug-19",
                                             BackButton = backButton,
                                             UpDownFeedback = upDownFeedback,
                                             RightHandSideContent = GetFeedbackComponent(),
                                             IsPractitionerSpecificPage = false
                                         }),
                    4 => Task.FromResult(new CannotFindQualificationPage
                                         {
                                             Heading = "This is the level 4 page",
                                             Body = ContentfulContentHelper.Paragraph("This is the body text"),
                                             FromWhichYear = "Sep-19",
                                             ToWhichYear = string.Empty,
                                             BackButton = backButton,
                                             UpDownFeedback = upDownFeedback,
                                             RightHandSideContent = GetFeedbackComponent(),
                                             IsPractitionerSpecificPage = false
                                         }),
                    _ => Task.FromResult<CannotFindQualificationPage>(null!)
                })!;
    }

    public async Task<CheckAdditionalRequirementsAnswerPage?> GetCheckAdditionalRequirementsAnswerPage()
    {
        return await Task.FromResult(new CheckAdditionalRequirementsAnswerPage
                                     {
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Test display text",
                                                          OpenInNewTab = false,
                                                          Href = "/qualifications/check-additional-questions"
                                                      },
                                         ButtonText = "Test button text",
                                         PageHeading = "Test page heading",
                                         AnswerDisclaimerText = "Test answer disclaimer text",
                                         ChangeAnswerText = "Test change answer text"
                                     });
    }

    public async Task<OpenGraphData?> GetOpenGraphData()
    {
        return await Task.FromResult(new OpenGraphData
                                     {
                                         Title = "OG Title",
                                         Description = "OG Description",
                                         Domain = "OG Domain",
                                         Image = new Asset
                                                 {
                                                     File = new File
                                                            {
                                                                Url = "test/url/og-image.png"
                                                            }
                                                 }
                                     });
    }

    public async Task<CheckYourAnswersPage?> GetCheckYourAnswersPage()
    {
        return await Task.FromResult(new CheckYourAnswersPage
                                     {
                                         PageHeading = "Check your answers",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "TEST",
                                                          Href = "/questions/what-is-the-awarding-organisation",
                                                          OpenInNewTab = false
                                                      },
                                         CtaButtonText = CtaButtonText,
                                         ChangeAnswerText = "Change",
                                         QualificationAwardedText = "Awarded in",
                                         QualificationStartedText = "Started in",
                                         AnyAwardingOrganisationText = "Various awarding organisations",
                                         AnyLevelText = "Any level"
                                     });
    }

    public async Task<PreCheckPage?> GetPreCheckPage()
    {
        return await Task.FromResult(new PreCheckPage
                                     {
                                         Header = "Get ready to start the qualification check",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Back",
                                                          Href = "/",
                                                          OpenInNewTab = false
                                                      },
                                         PostHeaderContent =
                                             ContentfulContentHelper.Paragraph("This is the post header content"),
                                         Question = "Do you have all the information you need to complete the check?",
                                         Options =
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
                                         InformationMessage = "You need all the information listed above to get a result. If you do not have it, you will not be able to complete this check.",
                                         CtaButtonText = CtaButtonText,
                                         ErrorBannerHeading = ThereIsAProblem,
                                         ErrorMessage = "Confirm if you have all the information you need to complete the check"
                                     });
    }

    public async Task<Footer?> GetFooter()
    {
        return await Task.FromResult(new Footer
                                     {
                                         LeftHandSideFooterSection = new FooterSection
                                                                         {
                                                                             Body =
                                                                                 ContentfulContentHelper
                                                                                     .Paragraph("This is the left hand side footer content"),
                                                                             Heading = "Left section"
                                                                         },
                                         RightHandSideFooterSection = new FooterSection
                                                                          {
                                                                              Body =
                                                                                  ContentfulContentHelper
                                                                                      .Paragraph("This is the right hand side footer content"),
                                                                              Heading = "Right section"
                                                                          },
                                         NavigationLinks =
                                         [
                                             new NavigationLink
                                             {
                                                 DisplayText = "Privacy notice",
                                                 Href = "/link-to-privacy-notice"
                                             },
                                             new NavigationLink
                                             {
                                                 DisplayText = "Accessibility statement",
                                                 Href = "/link-to-accessibility-statement"
                                             }
                                         ]
                                     });
    }

    public async Task<FeedbackFormPage?> GetFeedbackFormPage()
    {
        return await Task.FromResult(new FeedbackFormPage
                                     {
                                         Heading = "Give feedback",
                                         PostHeadingContent =
                                             ContentfulContentHelper.Paragraph("This is the post heading content"),
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Home",
                                                          Href = "/",
                                                          OpenInNewTab = false
                                                      },
                                         CtaButtonText = "Submit feedback",
                                         ErrorBannerHeading = ThereIsAProblem,
                                         Questions =
                                         [
                                             new FeedbackFormQuestionRadio
                                             {
                                                 Sys = new SystemProperties
                                                       {
                                                           Id = FeedbackFormQuestions
                                                               .WouldYouLikeToBeContactedAboutResearch
                                                       },
                                                 Question = "Did you get everything you needed today?",
                                                 IsTheQuestionMandatory = true,
                                                 ErrorMessage =
                                                     "Select whether you got everything you needed today",
                                                 Options =
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
                                             },
                                             new FeedbackFormQuestionTextArea
                                             {
                                                 Question = "Tell us about your experience (optional)",
                                                 HintText =
                                                     "Do not include personal information, for example the name of the qualification holder"
                                             },
                                             new FeedbackFormQuestionRadioAndInput
                                             {
                                                 Question =
                                                     "Would you like us to contact you about future user research?",
                                                 IsTheQuestionMandatory = true,
                                                 Options =
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
                                                 InputHeading = "Your email address",
                                                 InputHeadingHintText = "Input heading hint text",
                                                 ValidateInputAsAnEmailAddress = true,
                                                 ErrorMessage =
                                                     "Select whether you want to be contacted about future research",
                                                 ErrorMessageForInput = "Enter your email address",
                                                 ErrorMessageForInvalidEmailFormat =
                                                     "Enter an email address in the correct format, like name@example.com"
                                             }
                                         ]
                                     });
    }

    public async Task<FeedbackFormConfirmationPage?> GetFeedbackFormConfirmationPage()
    {
        return await Task.FromResult(new FeedbackFormConfirmationPage
                                     {
                                         SuccessMessage = "Your feedback has been successfully submitted",
                                         Body =
                                             ContentfulContentHelper
                                                 .Paragraph("Thank you for your feedback. We look at every piece of feedback and will use your comments to make the service better for everyone."),
                                         OptionalEmailHeading = "What happens next",
                                         OptionalEmailBody =
                                             ContentfulContentHelper
                                                 .Paragraph("As you agreed to be contacted about future research, someone from our research team may contact you by email."),
                                         ReturnToHomepageLink = new NavigationLink
                                                                {
                                                                    DisplayText = "Home",
                                                                    Href = "/",
                                                                    OpenInNewTab = false
                                                                }
                                     });
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

    public async Task<GetHelpPage?> GetGetHelpPage()
    {
        return await Task.FromResult(
            new GetHelpPage
            {
                Heading = "Get help with the Check an early years qualification service",
                PostHeadingContent = ContentfulContentHelper.Paragraph("Use this form to ask a question about a qualification or report a problem with the service or the information it provides.\r\nWe aim to respond to all queries within 5 working days. Complex cases may take longer.\r\n"),
                ReasonForEnquiryHeading = "Why are you contacting us?",
                CtaButtonText = CtaButtonText,
                BackButton = new NavigationLink
                {
                    DisplayText = "Home",
                    Href = HomePath,
                    OpenInNewTab = false
                },
                ErrorBannerHeading = ThereIsAProblem,
                NoEnquiryOptionSelectedErrorMessage = "Select one option",
                EnquiryReasons =
                [
                    new EnquiryOption
                    { Label = "I have a question about a qualification", Value = "QuestionAboutAQualification" },
                    new EnquiryOption
                    { Label = "I am experiencing an issue with the service", Value = "IssueWithTheService" }
                ]
            }
        );
    }

    public async Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPage()
    {
        return await Task.FromResult(
            new HelpQualificationDetailsPage
            {
                Heading = "What are the qualification details?",
                PostHeadingContent = "We need to know the following qualification details to quickly and accurately respond to any questions you may have.",
                CtaButtonText = CtaButtonText,
                BackButton = new NavigationLink
                {
                    DisplayText = "Back to get help with the Check an early years qualification service",
                    Href = "/help/get-help",
                    OpenInNewTab = false
                },
                QualificationNameHeading = "Qualification name",
                QualificationNameErrorMessage = "Enter the qualification name",
                AwardingOrganisationHeading = "Awarding organisation",
                AwardingOrganisationErrorMessage = "Enter the awarding organisation",
                ErrorBannerHeading = ThereIsAProblem,
                AwardedDateIsAfterStartedDateErrorText = "The awarded date must be after the started date",
                StartDateQuestion = new DateQuestion
                {
                    MonthLabel = "Month",
                    YearLabel = "Year",
                    QuestionHeader = "Start date (optional)",
                    QuestionHint = "Enter the start date so we can check if the qualification is approved as full and relevant. For example 9 2013.",
                    ErrorBannerLinkText = "Enter the month and year that the qualification was started",
                    ErrorMessage = "Enter the month and year that the qualification was started",
                    FutureDateErrorBannerLinkText = "The date the qualification was started must be in the past",
                    FutureDateErrorMessage = "The date the qualification was started must be in the past",
                    MissingMonthErrorMessage = "Enter the month that the qualification was started",
                    MissingYearErrorMessage = "Enter the year that the qualification was started",
                    MissingMonthBannerLinkText = "Enter the month that the qualification was started",
                    MissingYearBannerLinkText = "Enter the year that the qualification was started",
                    MonthOutOfBoundsErrorLinkText = "The month the qualification was started must be between 1 and 12",
                    MonthOutOfBoundsErrorMessage = "The month the qualification was started must be between 1 and 12",
                    YearOutOfBoundsErrorLinkText = "The year the qualification was started must be between 1900 and $[actual-year]$",
                    YearOutOfBoundsErrorMessage = "The year the qualification was started must be between 1900 and $[actual-year]$"
                },
                AwardedDateQuestion = new DateQuestion
                {
                    MonthLabel = "Month",
                    YearLabel = "Year",
                    QuestionHeader = "Award date",
                    QuestionHint = "Enter the date the qualification was awarded so we can tell you if other requirements apply. For example 6 2015.",
                    ErrorBannerLinkText = "Enter the month and year that the qualification was awarded",
                    ErrorMessage = "Enter the date the qualification was awarded",
                    FutureDateErrorBannerLinkText = "The date the qualification was awarded must be in the past",
                    FutureDateErrorMessage = "The date the qualification was awarded must be in the past",
                    MissingMonthErrorMessage = "Enter the month that the qualification was awarded",
                    MissingYearErrorMessage = "Enter the year that the qualification was awarded",
                    MissingMonthBannerLinkText = "Enter the month that the qualification was awarded",
                    MissingYearBannerLinkText = "Enter the year that the qualification was awarded",
                    MonthOutOfBoundsErrorLinkText = "The month the qualification was awarded must be between 1 and 12",
                    MonthOutOfBoundsErrorMessage = "The month the qualification was awarded must be between 1 and 12",
                    YearOutOfBoundsErrorLinkText = "The year the qualification was awarded must be between 1900 and $[actual-year]$",
                    YearOutOfBoundsErrorMessage = "The year the qualification was awarded must be between 1900 and $[actual-year]$"
                }
            }
        );
    }

    public async Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage()
    {
        return await Task.FromResult(
            new HelpProvideDetailsPage
            {
                Heading = "How can we help you?",
                PostHeadingContent = "Give as much detail as you can. This helps us give you the right support.",
                CtaButtonText = CtaButtonText,
                BackButtonToGetHelpPage = new NavigationLink
                {
                    DisplayText = "Back to get help with the Check an early years qualification service",
                    Href = "/help/get-help",
                    OpenInNewTab = false
                },
                BackButtonToQualificationDetailsPage = new NavigationLink
                {
                    DisplayText = "Back to what are the qualification details",
                    Href = "/help/qualification-details",
                    OpenInNewTab = false
                },
                AdditionalInformationWarningText = "Do not include any personal information",
                AdditionalInformationErrorMessage = "Provide information about how we can help you",
                ErrorBannerHeading = ThereIsAProblem
            }
        );
    }

    public async Task<HelpEmailAddressPage?> GetHelpEmailAddressPage()
    {
        return await Task.FromResult(
            new HelpEmailAddressPage
            {
                Heading = "What is your email address?",
                InvalidEmailAddressErrorMessage = "Enter an email address in the correct format, for example name@example.com",
                NoEmailAddressEnteredErrorMessage = "Enter an email address",
                BackButton = new NavigationLink
                {
                    DisplayText = "Back to how can we help you",
                    Href = "/help/provide-details",
                    OpenInNewTab = false
                },
                CtaButtonText = "Send message",
                ErrorBannerHeading = ThereIsAProblem,
                PostHeadingContent = "We will only use this email address to reply to your message"
            }
        );
    }

    public async Task<HelpConfirmationPage?> GetHelpConfirmationPage()
    {
        return await Task.FromResult(
            new HelpConfirmationPage
            {
                SuccessMessage = "Message sent",
                BodyHeading = "What happens next",
                Body = ContentfulContentHelper.Paragraph("The Check an early years qualification team will reply to your message within 5 working days. Complex cases may take longer.\r\nWe may need to contact you for more information before we can respond.\r\n"),
                FeedbackComponent = GetFeedbackComponent(),
                SuccessMessageFollowingText = "Your message was successfully sent to the Check an early years qualification team.",
                ReturnToHomepageLink = new NavigationLink
                {
                    DisplayText = "Return to the homepage",
                    Href = "/"
                }
            }
        );
    }
    
    private static RadioQuestionPage CreateAreYouCheckingYourOwnQualificationPage()
    {
        var options = new List<IOptionItem>
                      {
                          new Option
                          {
                              Label = "Yes, I am checking my own qualification", Value = "yes"
                          },
                          new Option
                          {
                              Label = "No, I am checking someone else's qualification", Value = "no"
                          }
                      };

        return CreateRadioQuestionPage("Are you checking your qualification or someone else's?", options, "/");
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
                              Label = "Level 4", Value = "4"
                          },
                          new Option
                          {
                              Label = "Level 5", Value = "5"
                          },
                          new Option
                          {
                              Label = "Level 6", Value = "6", Hint = "Some hint text"
                          },
                          new Option
                          {
                              Label = "Level 7", Value = "7"
                          },
                          new Option
                          {
                              Label = "Not Sure", Value ="0"
                          }
                      };
        return CreateRadioQuestionPage("What level is the qualification?", options,
                                       "/questions/when-was-the-qualification-started-and-awarded");
    }

    private static RadioQuestionPage CreateRadioQuestionPage(string question, List<IOptionItem> options,
                                                             string backButtonUrl)
    {
        return new RadioQuestionPage
               {
                   Question = question,
                   Options = options,
                   CtaButtonText = CtaButtonText,
                   ErrorMessage = "Test error message",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = backButtonUrl,
                                    OpenInNewTab = false
                                },
                   ErrorBannerHeading = ThereIsAProblem,
                   ErrorBannerLinkText = "Test error banner link text",
                   AdditionalInformationBody =
                       ContentfulContentHelper.Paragraph("This is the additional information body"),
                   AdditionalInformationHeader = "This is the additional information header"
               };
    }

    private static DatesQuestionPage CreateDatesQuestionPage()
    {
        return new DatesQuestionPage
               {
                   Question = "Test Dates Questions",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = WhereWasTheQualificationAwardedPath,
                                    OpenInNewTab = false
                                },
                   CtaButtonText = CtaButtonText,
                   ErrorBannerHeading = ThereIsAProblem,
                   AwardedDateIsAfterStartedDateErrorText = "Error- AwardedDateIsAfterStartedDateErrorText",
                   StartedQuestion = CreateDatesQuestionPage("started- "),
                   AwardedQuestion = CreateDatesQuestionPage("awarded- ")
               };
    }

    private static DateQuestion CreateDatesQuestionPage(string prefix)
    {
        return new DateQuestion
               {
                   MonthLabel = prefix + "Test Month Label",
                   YearLabel = prefix + "Test Year Label",
                   QuestionHeader = prefix + "Test Question Hint Header",
                   QuestionHint = prefix + "Test Question Hint",
                   ErrorBannerLinkText = prefix + "Test error banner link text",
                   ErrorMessage = prefix + "Test Error Message",
                   FutureDateErrorBannerLinkText = prefix + "Future date error message banner link",
                   FutureDateErrorMessage = prefix + "Future date error message",
                   MissingMonthErrorMessage = prefix + "Missing Month Error Message",
                   MissingYearErrorMessage = prefix + "Missing Year Error Message",
                   MissingMonthBannerLinkText = prefix + "Missing Month Banner Link Text",
                   MissingYearBannerLinkText = prefix + "Missing Year Banner Link Text",
                   MonthOutOfBoundsErrorLinkText = prefix + "Month Out Of Bounds Error Link Text",
                   MonthOutOfBoundsErrorMessage = prefix + "Month Out Of Bounds Error Message",
                   YearOutOfBoundsErrorLinkText = prefix + "Year Out Of Bounds Error Link Text",
                   YearOutOfBoundsErrorMessage = prefix + "Year Out Of Bounds Error Message"
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
                   ErrorBannerHeading = ThereIsAProblem,
                   ErrorBannerLinkText = "Test error banner link text",
                   AdditionalInformationBody =
                       ContentfulContentHelper.Paragraph("This is the additional information body"),
                   AdditionalInformationHeader = "This is the additional information header"
               };
    }

    private static AdvicePage CreateAdvicePage(string heading, Document body, string backButtonUrl, bool hasUpDownFeedback)
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
                                },
                   RightHandSideContent = GetFeedbackComponent(),
                   UpDownFeedback = hasUpDownFeedback ? GetUpDownFeedback() : null
               };
    }

    private static UpDownFeedback GetUpDownFeedback()
    {
        return new UpDownFeedback
               {
                   Question = "Did you get everything you needed today?",
                   YesButtonText = "Yes",
                   YesButtonSubText = "this service is useful",
                   NoButtonText = "No",
                   NoButtonSubText = " this service is not useful",
                   HelpButtonText = "Get help with this page",
                   HelpButtonLink = "/help/get-help",
                   CancelButtonText = "Cancel",
                   FeedbackComponent = GetFeedbackComponent()
               };
    }

    private static FeedbackComponent GetFeedbackComponent()
    {
        return new FeedbackComponent
               {
                   Header = "Give feedback",
                   Body =
                       ContentfulContentHelper.Paragraph("Your feedback matters and will help us improve the service.")
               };
    }
}