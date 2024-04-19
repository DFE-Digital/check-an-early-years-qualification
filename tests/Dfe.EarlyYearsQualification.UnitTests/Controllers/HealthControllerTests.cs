using Dfe.EarlyYearsQualification.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HealthControllerTests
{
    [TestMethod]
    public void HealthController_Get_ReturnsOK()
    {
        var controller = new HealthController();
        var result = controller.Get();
        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<OkResult>(result);
    }
}