using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Entities;
using Moq;

namespace Dfe.EarlyYearsQualification.Web.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddMockContentful(this IServiceCollection services)
  {

    var mockContentfulService = new Mock<IContentService>();

    mockContentfulService.Setup(x => x.GetStartPage()).ReturnsAsync(new StartPage
    {
      Header = "Test Header",
      PreCtaButtonContentHtml = "<p id='pre-cta-content'>This is the pre cta content</p>",
      CtaButtonText = "Start Button Text",
      PostCtaButtonContentHtml = "<p id='post-cta-content'>This is the post cta content</p>",
      RightHandSideContentHeader = "Related content",
      RightHandSideContentHtml = "<p id='right-hand-content'>This is the right hand content</p>"
    });

    mockContentfulService.Setup(x => x.GetNavigationLinks()).ReturnsAsync(new List<NavigationLink>
    {
      new() { DisplayText = "Privacy notice", Href="#"}
    });

    mockContentfulService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage
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

    mockContentfulService.Setup(x => x.GetQualificationById(It.IsAny<string>())).ReturnsAsync(new Qualification(
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

    services.AddSingleton(mockContentfulService.Object);
    return services;
  }
}