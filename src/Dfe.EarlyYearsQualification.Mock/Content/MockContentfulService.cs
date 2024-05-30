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
                                         Body = body
                                     });
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        var body = ContentfulContentHelper.Paragraph("Test Advice Page Body");

        return await Task.FromResult(new AdvicePage
                                     {
                                         Heading = "Qualifications achieved outside the United Kingdom",
                                         Body = body
                                     });
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
                                         SuccessBannerContent = successBannerContent
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
                                         FurtherInfoText = furtherInfoText
                                     });
    }

    public async Task<List<NavigationLink>?> GetNavigationLinks()
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
                                                       "The course must be assessed within the EYFS in an Early Years setting in England. Please note that the name of this qualification changed in February 2023. Qualifications achieved under either name are full and relevant provided that the start date for the qualification aligns with the date of the name change.",
                                                       "Additional notes"
                                                      ));
    }

    public async Task<QuestionPage?> GetQuestionPage(string entryId)
    {
        switch (entryId)
        {
            case QuestionPages.WhatLevelIsTheQualification: 
                return await Task.FromResult(CreateWhatLevelIsTheQualificationPage());
            case QuestionPages.WhereWasTheQualificationAwarded:
                return await Task.FromResult(CreateWhereWasTheQualificationAwardedPage());
            default:
                throw new NotImplementedException($"No question page mock for entry {entryId}");
        }
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

    private QuestionPage? CreateWhereWasTheQualificationAwardedPage()
    {
        var options = new List<Option>
                       {
                            new Option
                            {
                                Label = "England", Value = "england"
                            },
                            new Option
                            {
                                Label = "Outside the United Kingdom",
                                Value = "outside-uk"
                            }
                        };
        return CreateQuestionPage("Where was the qualification awarded?", options);
    }

    private QuestionPage? CreateWhatLevelIsTheQualificationPage()
    {
        var options = new List<Option>
                       {
                            new Option
                            {
                                Label = "Level 2", Value = "2"
                            },
                            new Option
                            {
                                Label = "Level 3", Value = "3"
                            }
                        };
        return CreateQuestionPage("What level is the qualification?", options);
    }

    private QuestionPage CreateQuestionPage(string question, List<Option> options)
    {
        return new QuestionPage { 
            Question = question, 
            Options = options, 
            CtaButtonText = "Continue", 
            ErrorMessage = "Test error message" };
    }

    public async Task<CookiesBanner?> GetCookiesBannerContent()
    {
      var AcceptedCookiesContent = 
            ContentfulContentHelper.Paragraph("This is the accepted cookie content");

      var cookiesBannerContent =
            ContentfulContentHelper.Paragraph("This is the cookies banner content");

      var RejectedCookieContent = 
            ContentfulContentHelper.Paragraph("This is the rejected cookie content");

      return await Task.FromResult(new CookiesBanner
                                     {
                                         AcceptButtonText = "Test Accept Button Text",
                                         AcceptedCookiesContent = AcceptedCookiesContent,
                                         CookiesBannerContent = cookiesBannerContent,
                                         CookiesBannerTitle = "Test Cookies Banner Title",
                                         CookiesBannerLinkText = "Test Cookies Banner Text",
                                         HideCookieBannerButtonText = "Test Hide Cookie Banner Button Text",
                                         RejectButtonText = "Test Reject Button Text",
                                         RejectedCookiesContent = RejectedCookieContent
                                     });
    }
}