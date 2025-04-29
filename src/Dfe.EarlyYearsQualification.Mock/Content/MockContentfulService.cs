using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Helpers;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using File = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulService : IContentService
{
    private const string WhereWasTheQualificationAwardedPath = "/questions/where-was-the-qualification-awarded";
    private const string WhatLevelIsTheQualificationPath = "/questions/what-level-is-the-qualification";
    private const string QualificationsPath = "/select-a-qualification-to-check";
    private const string HomePath = "/";
    private const string ThereIsAProblem = "There is a problem";

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

    public async Task<DetailsPage?> GetDetailsPage()
    {
        var ratioText = ContentfulContentHelper.Paragraph("This is the ratio text");
        var ratioTextForNotFullAndRelevant = ContentfulContentHelper.Paragraph("This is not F&R");
        var ratioTextL3PlusNotFullAndRelevantBetweenSep14AndAug19 =
            ContentfulContentHelper.Paragraph("This is not F&R for L3 between Sep14 & Aug19");
        var requirementsText = ContentfulContentHelper.Paragraph("This is the requirements text");
        return await Task.FromResult(new DetailsPage
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
                                                                    DisplayText =
                                                                        "TEST (back to additional questions)",
                                                                    Href =
                                                                        "/qualifications/check-additional-questions/$[qualification-id]$/confirm-answers",
                                                                    OpenInNewTab = false
                                                                },
                                         RatiosHeading = "Test ratio heading",
                                         RatiosText = ratioText,
                                         RatiosTextNotFullAndRelevant = ratioTextForNotFullAndRelevant,
                                         RatiosTextL3PlusNotFrBetweenSep14Aug19 =
                                             ratioTextL3PlusNotFullAndRelevantBetweenSep14AndAug19,
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
                                         QualificationStartDateLabel = "Qualification start date",
                                         QualificationAwardedDateLabel = "Qualification awarded date",
                                         FeedbackBanner = new FeedbackBanner
                                                          {
                                                              Body = ContentfulContentHelper.Paragraph("Test body"),
                                                              BannerTitle = "Test banner title",
                                                              Heading = "Test heading"
                                                          },
                                         QualificationResultHeading = "Qualification result heading",
                                         QualificationResultFrMessageHeading = "Full and relevant",
                                         QualificationResultFrMessageBody = "Full and relevant body",
                                         QualificationResultNotFrMessageHeading = "Not full and relevant",
                                         QualificationResultNotFrMessageBody = "Not full and relevant body",
                                         QualificationResultNotFrL3MessageHeading = "Not full and relevant L3",
                                         QualificationResultNotFrL3MessageBody = "Not full and relevant L3 body",
                                         UpDownFeedback = GetUpDownFeedback()
                                     });
    }

    public async Task<List<NavigationLink>> GetNavigationLinks()
    {
        return await Task.FromResult(new List<NavigationLink>
                                     {
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
                                     });
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
                                                          Href = "/questions/what-is-the-awarding-organisation",
                                                          OpenInNewTab = false
                                                      },
                                         QualificationFoundPrefix = "We found",
                                         SearchButtonText = "Refine",
                                         SearchCriteriaHeading = "Your search",
                                         MultipleQualificationsFoundText = "matching qualifications",
                                         SingleQualificationFoundText = "matching qualification",
                                         PreSearchBoxContent =
                                             ContentfulContentHelper.Text("Pre search box content"),
                                         PostQualificationListContent =
                                             ContentfulContentHelper.Link("Link to not on list advice page",
                                                                          "/advice/qualification-not-on-the-list"),
                                         AnyLevelHeading = "any level",
                                         AnyAwardingOrganisationHeading = "various awarding organisations",
                                         NoResultsText =
                                             ContentfulContentHelper.ParagraphWithBold("Test no qualifications text"),
                                         ClearSearchText = "Clear search",
                                         AwardedLocationPrefixText = "awarded in",
                                         StartDatePrefixText = "started in",
                                         AwardedDatePrefixText = "awarded in",
                                         LevelPrefixText = "level",
                                         AwardedByPrefixText = "awarded by"
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
                                         NoAdditionalRequirementsButtonText = "Get result"
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

    public Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(int level, int startMonth, int startYear)
    {
        var backButton = new NavigationLink
                         {
                             DisplayText = "TEST",
                             Href = QualificationsPath,
                             OpenInNewTab = false
                         };

        var feedbackBanner = new FeedbackBanner
                             {
                                 Heading = "Feedback banner heading",
                                 BannerTitle = "Banner title",
                                 Body = ContentfulContentHelper.Paragraph("Banner body text")
                             };

        var upDownFeedback = GetUpDownFeedback();

        return (level switch
                {
                    3 => Task.FromResult(new CannotFindQualificationPage
                                         {
                                             Heading = "This is the level 3 page",
                                             Body = ContentfulContentHelper.Paragraph("This is the body text"),
                                             FromWhichYear = "Sep-14",
                                             ToWhichYear = "Aug-19",
                                             BackButton = backButton,
                                             FeedbackBanner = feedbackBanner,
                                             UpDownFeedback = upDownFeedback
                                         }),
                    4 => Task.FromResult(new CannotFindQualificationPage
                                         {
                                             Heading = "This is the level 4 page",
                                             Body = ContentfulContentHelper.Paragraph("This is the body text"),
                                             FromWhichYear = "Sep-19",
                                             ToWhichYear = string.Empty,
                                             BackButton = backButton,
                                             FeedbackBanner = feedbackBanner,
                                             UpDownFeedback = upDownFeedback
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
                                                          Href = WhereWasTheQualificationAwardedPath,
                                                          OpenInNewTab = false
                                                      },
                                         CtaButtonText = "Continue",
                                         ChangeAnswerText = "Change",
                                         QualificationAwardedText = "Awarded in",
                                         QualificationStartedText = "Started in",
                                         AnyAwardingOrganisationText = "Various awarding organisations",
                                         AnyLevelText = "Any level"
                                     });
    }

    public async Task<HelpPage?> GetHelpPage()
    {
        return await Task.FromResult(new HelpPage
                                     {
                                         Heading = "Help Page Heading",
                                         PostHeadingContent =
                                             ContentfulContentHelper.Paragraph("This is the post heading text"),
                                         EmailAddressHeading = "Enter your email address (optional)",
                                         EmailAddressHintText =
                                             "If you do not enter your email address we will not be able to contact you in relation to your enquiry",
                                         ReasonForEnquiryHeading = "Choose the reason of your enquiry",
                                         ReasonForEnquiryHintText = "Select one option",
                                         EnquiryReasons =
                                         [
                                             new EnquiryOption
                                             { Label = "Option 1", Value = "Option 1" },
                                             new EnquiryOption
                                             { Label = "Option 2", Value = "Option 2" },
                                             new EnquiryOption
                                             { Label = "Option 3", Value = "Option 3" }
                                         ],
                                         AdditionalInformationHeading =
                                             "Provide further information about your enquiry",
                                         AdditionalInformationHintText =
                                             "Provide details about the qualification you are checking for or the specific issue you are experiencing with the service.",
                                         AdditionalInformationWarningText =
                                             "Do not include personal information, for example the name of the qualification holder",
                                         CtaButtonText = "Send message",
                                         BackButton = new NavigationLink
                                                      {
                                                          DisplayText = "Home",
                                                          Href = HomePath,
                                                          OpenInNewTab = false
                                                      },
                                         ErrorBannerHeading = ThereIsAProblem,
                                         NoEmailAddressEnteredErrorMessage = "Enter an email address",
                                         InvalidEmailAddressErrorMessage = "Enter a valid email address",
                                         NoEnquiryOptionSelectedErrorMessage = "Select one option",
                                         FurtherInformationErrorMessage = "Enter further information about your enquiry"
                                     });
    }

    public async Task<HelpConfirmationPage?> GetHelpConfirmationPage()
    {
        return await Task.FromResult(new HelpConfirmationPage
                                     {
                                         SuccessMessage = "This is the success message",
                                         BodyHeading = "Body heading",
                                         Body = ContentfulContentHelper.Paragraph("This is the body")
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
                          },
                          new Option
                          {
                              Label = "Not Sure", Value = "0"
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
                   CtaButtonText = "Continue",
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
                   CtaButtonText = "Continue",
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

    private static AdvicePage CreateAdvicePage(string heading, Document body, string backButtonUrl,
                                               bool hasUpDownFeedback)
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
                   FeedbackBanner = new FeedbackBanner
                                    {
                                        Heading = "Feedback heading",
                                        Body = ContentfulContentHelper.Paragraph("This is the body text"),
                                        BannerTitle = "Test banner title"
                                    },
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
                   HelpButtonLink = "/advice/help",
                   CancelButtonText = "Cancel",
                   UsefulResponse = "Thank you for your feedback",
                   ImproveServiceContent = ContentfulContentHelper.Paragraph("This is the improve service content")
               };
    }
}