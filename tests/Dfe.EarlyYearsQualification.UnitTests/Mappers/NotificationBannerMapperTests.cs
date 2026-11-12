using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class NotificationBannerMapperTests
{
    [TestMethod]
    public void Map_PassInNull_ReturnsNull()
    {
        var result = NotificationBannerMapper.Map(null, "<p>Body</p>", "banner-id");

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_PassInNotificationBanner_ReturnsModel()
    {
        var notificationBanner = new NotificationBanner
                                 {
                                     Heading = "Important",
                                     Body = ContentfulContentHelper.Paragraph("Banner body")
                                 };

        var result = NotificationBannerMapper.Map(notificationBanner, "<p>Banner body</p>", "banner-id", fullWidth: true);

        result.Should().NotBeNull();
        result!.Id.Should().Be("banner-id");
        result.Heading.Should().Be("Important");
        result.Body.Should().Be("<p>Banner body</p>");
        result.FullWidth.Should().BeTrue();
    }

    [TestMethod]
    public void Map_FullWidthNotSpecified_DefaultsToFalse()
    {
        var notificationBanner = new NotificationBanner { Heading = "Important" };

        var result = NotificationBannerMapper.Map(notificationBanner, string.Empty, "banner-id");

        result.Should().NotBeNull();
        result!.FullWidth.Should().BeFalse();
    }
}
