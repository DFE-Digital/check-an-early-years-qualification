using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
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
    public async Task MapToFeedbackBannerModel_PassInNull_ReturnNull()
    {
        var mockContentParser = new Mock<IGovUkContentParser>();
        var result = await DummyController.Map(null, mockContentParser.Object);
        result.Should().BeNull();
    }
    
    [TestMethod]
    public async Task MapToFeedbackBannerModel_PassInFeedbackBanner_ReturnModel()
    {
        const string contentResult = "This is a test";
        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(contentResult);
        var feedbackBanner = new FeedbackBanner
                             {
                                 Heading = "Test",
                                 Body = new Document()
                             };
        var result = await DummyController.Map(feedbackBanner, mockContentParser.Object);
        result.Should().NotBeNull();
        result.Should().BeOfType<FeedbackBannerModel>();
        result!.Heading.Should().BeSameAs(feedbackBanner.Heading);
        result.Body.Should().BeSameAs(contentResult);
    }
}

public class DummyController : ServiceController
{
    public static NavigationLinkModel? Map(NavigationLink? navigationLink)
    {
        return MapToNavigationLinkModel(navigationLink);
    }

    public static async Task<FeedbackBannerModel?> Map(FeedbackBanner? feedbackBanner, IGovUkContentParser contentParser)
    {
        return await MapToFeedbackBannerModel(feedbackBanner, contentParser);
    }
}