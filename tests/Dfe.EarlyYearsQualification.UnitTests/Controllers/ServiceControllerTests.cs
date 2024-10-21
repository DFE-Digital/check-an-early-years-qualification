using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ServiceControllerTests
{
    [TestMethod]
    public void MapToNavigationLinkModel_PassInNull_ReturnNull()
    {
        var result = DummyController.Map(null);
        result.Should().BeNull();
    }
    
    [TestMethod]
    public void MapToNavigationLinkModel_PassInNavigationLink_ReturnModel()
    {
        var navLink = new NavigationLink
                      {
                          Sys = new SystemProperties
                                {
                                    ContentType = new ContentType
                                                  {
                                                      SystemProperties = new SystemProperties
                                                                         { Id = "externalNavigationLink" }
                                                  }
                                },
                          OpenInNewTab = true,
                          DisplayText = "DisplayText",
                          Href = "/"
                      };
        var result = DummyController.Map(navLink);
        result.Should().NotBeNull();
        result.Should().BeOfType<NavigationLinkModel>();
        result!.Href.Should().BeSameAs(navLink.Href);
        result.DisplayText.Should().BeSameAs(navLink.DisplayText);
        result.OpenInNewTab.Should().Be(navLink.OpenInNewTab);
    }

    [TestMethod]
    public async Task GetFeedbackBannerBodyToHtml_PassInNullFeedbackBanner_ReturnsNull()
    {
        var mockGovUkContentParser = new Mock<IGovUkContentParser>();
        var result = await DummyController.GetFeedbackBannerBody(null, mockGovUkContentParser.Object);
        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task GetFeedbackBannerBodyToHtml_PassInFeedbackBanner_ReturnsContent()
    {
        const string feedbackBannerContent = "This is the feedback banner content";
        var feedbackBannerContentDocument = ContentfulContentHelper.Paragraph(feedbackBannerContent);
        var mockGovUkContentParser = new Mock<IGovUkContentParser>();
        mockGovUkContentParser.Setup(x => x.ToHtml(feedbackBannerContentDocument)).ReturnsAsync(feedbackBannerContent);

        var feedbackBanner = new FeedbackBanner
                             {
                                 Heading = "Heading",
                                 BannerTitle = "Banner title",
                                 Body = feedbackBannerContentDocument
                             };
        
        var result = await DummyController.GetFeedbackBannerBody(feedbackBanner, mockGovUkContentParser.Object);
        
        result.Should().NotBeNull();
        result.Should().BeSameAs(feedbackBannerContent);
    }
}

public class DummyController : ServiceController
{
    public static NavigationLinkModel? Map(NavigationLink? navigationLink)
    {
        return MapToNavigationLinkModel(navigationLink);
    }

    public static async Task<string?> GetFeedbackBannerBody(FeedbackBanner? feedbackBanner, IGovUkContentParser contentParser)
    {
        return await GetFeedbackBannerBodyToHtml(feedbackBanner, contentParser);
    }
}