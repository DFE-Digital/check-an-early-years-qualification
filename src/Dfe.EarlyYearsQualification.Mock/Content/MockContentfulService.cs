using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulService : IContentService
{
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
                                                              body, "/questions/where-was-the-qualification-awarded")),
                   AdvicePages.QualificationsStartedBetweenSept2014AndAug2019 =>
                       await
                           Task.FromResult(CreateAdvicePage("Level 2 qualifications started between 1 September 2014 and 31 August 2019",
                                                            body, "/questions/what-level-is-the-qualification")),
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
                                                      }
                                     });
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        var checkAnotherQualificationText = ContentfulContentHelper.Paragraph("Test Check Another Qualification Text");
        var furtherInfoText = ContentfulContentHelper.Paragraph("Test Further Info Text");
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
                                                      }
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
        var content = ContentfulContentHelper.Text("Test phase banner content");

        return await Task.FromResult(new PhaseBanner
                                     {
                                         Content = content,
                                         PhaseName = "Test phase banner name",
                                         Show = true
                                     });
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        return await Task.FromResult(new Qualification(
                                                       "EYQ-240",
                                                       "T Level Technical Qualification in Education and Childcare (Specialism - Early Years Educator)",
                                                       "NCFE",
                                                       3,
                                                       "2020",
                                                       "2021",
                                                       "603/5829/4",
                                                       "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change."
                                                      ));
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
                                       "A awarding organisation", 123, null,
                                       null, null, null),
                                   new("2", "TEST",
                                       "B awarding organisation", 123, null,
                                       null, null, null),
                                   new("3", "TEST",
                                       "C awarding organisation", 123, null,
                                       null, null, null),
                                   new("4", "TEST",
                                       "D awarding organisation", 123, null,
                                       null, null, null),
                                   new("5", "TEST",
                                       "E awarding organisation", 123, null,
                                       null, null, null)
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
                                         PreSearchBoxContent = ContentfulContentHelper.Text("Pre search box content"),
                                         PostQualificationListContent = ContentfulContentHelper.Text("Post qualification list content"),
                                         PostSearchCriteriaContent = ContentfulContentHelper.Text("Post search box content")
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
                                         ErrorBannerLink = "Test error banner link"
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
        var options = new List<Option>
                      {
                          new()
                          {
                              Label = "England", Value = "england"
                          },
                          new()
                          {
                              Label = "Outside the United Kingdom",
                              Value = "outside-uk"
                          }
                      };
        return CreateRadioQuestionPage("Where was the qualification awarded?", options, "/");
    }

    private static RadioQuestionPage CreateWhatLevelIsTheQualificationPage()
    {
        var options = new List<Option>
                      {
                          new()
                          {
                              Label = "Level 2", Value = "2"
                          },
                          new()
                          {
                              Label = "Level 3", Value = "3"
                          }
                      };
        return CreateRadioQuestionPage("What level is the qualification?", options,
                                       "/questions/when-was-the-qualification-started");
    }

    private static RadioQuestionPage CreateRadioQuestionPage(string question, List<Option> options,
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
                                }
               };
    }

    private static DateQuestionPage CreateDateQuestionPage()
    {
        return new DateQuestionPage
               {
                   Question = "Test Date Question",
                   CtaButtonText = "Continue",
                   ErrorMessage = "Test Error Message",
                   MonthLabel = "Test Month Label",
                   YearLabel = "Test Year Label",
                   QuestionHint = "Test Question Hint",
                   BackButton = new NavigationLink
                                {
                                    DisplayText = "TEST",
                                    Href = "/questions/where-was-the-qualification-awarded",
                                    OpenInNewTab = false
                                }
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
                                }
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
}