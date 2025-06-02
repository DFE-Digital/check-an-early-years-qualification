using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackBannerMapperTests
{
    [TestMethod]
    public void Map_PassInNullFeedbackBanner_ReturnsNull()
    {
        var result = FeedbackBannerMapper.Map(null, "test");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInNullFeedbackBannerBody_ReturnsNull()
    {
        var result = FeedbackBannerMapper.Map(new FeedbackBanner(), null);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInFeedbackBannerAndBody_ReturnsModel()
    {
        const string body = "This is a test";
        var feedbackBanner = new FeedbackBanner
                             {
                                 Heading = "This is the heading",
                                 Body = ContentfulContentHelper.Paragraph(body),
                                 BannerTitle = "This is the title"
                             };

        var result = FeedbackBannerMapper.Map(feedbackBanner, body);

        result.Should().NotBeNull();
        result!.Heading.Should().BeSameAs(feedbackBanner.Heading);
        result.Body.Should().BeSameAs(body);
        result.BannerTitle.Should().BeSameAs(feedbackBanner.BannerTitle);
    }
}