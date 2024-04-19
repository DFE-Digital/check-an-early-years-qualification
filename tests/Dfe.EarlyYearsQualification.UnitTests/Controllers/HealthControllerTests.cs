using Dfe.EarlyYearsQualification.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HealthControllerTests
{
    [TestMethod]
    public void Health_Get_ReturnsOK()
    {
        var controller = new HealthController();
        var result = controller.Get();
        
        Assert.IsInstanceOfType<OkResult>(result);
        
        Assert.Fail("Just testing what happens on the pipeline and PR when a test fails.");
    }
}