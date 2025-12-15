using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Models;

[TestClass]
public class FeedbackComponentContainerModelTests
{
    [TestMethod]
    public void FeedbackComponentContainerModel_SetsInitialValues()
    {
        var model = new FeedbackComponentContainerModel
                    {
                        FeedbackComponent = new FeedbackComponentModel()
                    };

        model.Should().NotBeNull();
        model.IsMobile.Should().BeFalse();
    }
}