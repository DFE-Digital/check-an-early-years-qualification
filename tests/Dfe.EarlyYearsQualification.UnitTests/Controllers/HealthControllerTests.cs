using Dfe.EarlyYearsQualification.Web.Controllers;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HealthControllerTests
{
    [TestMethod]
    public void HealthController_Get_ReturnsOK()
    {
        var controller = new HealthController();
        var result = controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<OkResult>();
    }
}