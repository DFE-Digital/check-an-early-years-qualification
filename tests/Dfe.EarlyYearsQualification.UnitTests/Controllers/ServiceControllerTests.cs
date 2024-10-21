using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ServiceControllerTests
{
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
    public static async Task<string?> GetFeedbackBannerBody(FeedbackBanner? feedbackBanner, IGovUkContentParser contentParser)
    {
        return await GetFeedbackBannerBodyToHtml(feedbackBanner, contentParser);
    }
}