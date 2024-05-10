using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.Mock.Content;

public class MockContentfulService : IContentService
{
    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        return await Task.FromResult(new AccessibilityStatementPage()
        {
            Heading = "Test Accessibility Statement Heading",
            BodyHtml = "Test Accessibility Statement Body",
        });
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        return await Task.FromResult(new AdvicePage
        {
            Heading = "Qualifications achieved outside the United Kingdom",
            BodyHtml = "<p id='outside-uk-body'>This is the body of the page</p>"
        });
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        return await Task.FromResult(new CookiesPage()
        {
            Heading = "Test Cookies Heading",
            BodyHtml = "Test Cookies Body",
            ButtonText = "Test Cookies Button",
            Options = new List<Option>()
                           {
                                new Option()
                                {
                                Label = "Test Option Label 1",
                                Value = "test-option-value-1"
                                },
                                new Option()
                                {
                                Label = "Test Option Label 2",
                                Value = "test-option-value-2"
                                }
                           },
            ErrorText = "Test Error Text",
            SuccessBannerHeading = "Test Banner Heading",
            SuccessBannerContentHtml = "Test Banner Content"
        });
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        return await Task.FromResult(new DetailsPage
        {
            AwardingOrgLabel = "Awarding Org Label",
            BookmarkHeading = "Test Bookmark Heading",
            BookmarkText = "Test Bookmark Text",
            CheckAnotherQualificationHeading = "Test Check Another Qualification Heading",
            CheckAnotherQualificationTextHtml = "<p id='check-another-qualification-text'>Test Check Another Qualification Text</p>",
            DateAddedLabel = "Test Date Added Label",
            DateOfCheckLabel = "Test Date Of Check Label",
            FurtherInfoTextHtml = "<p id='further-info-text'>Test Further Info Text</p>",
            FurtherInfoHeading = "Test Further Info Heading",
            LevelLabel = "Test Level Label",
            MainHeader = "Test Main Heading",
            QualificationNumberLabel = "Test Qualification Number Label"
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
        return await Task.FromResult(new PhaseBanner()
            {
                ContentHtml = "Test phase banner content",
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
        return await Task.FromResult(new QuestionPage
                         {
                           Question = "Where was the qualification awarded?",
                           Options =
                                           [
                                               new Option
                                                   {
                                                       Label = "England", Value = "england",
                                                   },

                                                   new Option
                                                   {
                                                       Label = "Outside the United Kingdom",
                                                       Value = "outside-uk"
                                                   }
                                           ],
                           CtaButtonText = "Continue",
                           ErrorMessage = "Test error message"
                         });
    }

    public async Task<StartPage?> GetStartPage()
    {
        return await Task.FromResult(new StartPage
                         {
                           Header = "Test Header",
                           PreCtaButtonContentHtml =
                                               "<p id='pre-cta-content'>This is the pre cta content</p>",
                           CtaButtonText = "Start Button Text",
                           PostCtaButtonContentHtml =
                                               "<p id='post-cta-content'>This is the post cta content</p>",
                           RightHandSideContentHeader =
                                               "Related content",
                           RightHandSideContentHtml =
                                               "<p id='right-hand-content'>This is the right hand content</p>"
                         });
    }
}