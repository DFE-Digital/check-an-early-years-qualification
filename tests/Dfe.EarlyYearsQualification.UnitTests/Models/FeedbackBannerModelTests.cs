using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models;

[TestClass]
public class FeedbackBannerModelTests
{
    [TestMethod]
    public void FeedbackBannerModelTests_SetsInitialValues()
    {
        var model = new FeedbackBannerModel();

        model.Should().NotBeNull();
        model.Heading.Should().BeEmpty();
        model.Body.Should().BeEmpty();
        model.BannerTitle.Should().BeEmpty();
    }
}