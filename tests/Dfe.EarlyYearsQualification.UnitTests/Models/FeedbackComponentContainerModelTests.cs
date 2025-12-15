using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class FeedbackComponentContainerModelTests
{
    [TestMethod]
    public void FeedbackComponentContainerModel_SetsInitialValues()
    {
        var model = new FeedbackComponentContainerModel
                    {
            FeedbackComponent = new()
        };

        model.Should().NotBeNull();
        model.IsMobile.Should().BeFalse();
    }
}