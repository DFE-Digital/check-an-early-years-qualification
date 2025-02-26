using Dfe.EarlyYearsQualification.Web.Controllers;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ErrorControllerTests
{
    [TestMethod]
    public void Index_ReturnsProblemWithServiceView()
    {
        var controller = new ErrorController();

        var result = controller.Index();

        result.Should().BeAssignableTo<ViewResult>()
              .Which.ViewName.Should().Be("ProblemWithTheService");
    }

    [TestMethod]
    public void HttpStatusCodeHandler_404_ReturnsNotFoundView()
    {
        var controller = new ErrorController();

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = controller.HttpStatusCodeHandler(404);

        result.Should().BeAssignableTo<ViewResult>()
              .Which.ViewName.Should().Be("NotFound");

        controller.ControllerContext.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [TestMethod]
    public void HttpStatusCodeHandler_Not404_ReturnsProblemWithServiceView()
    {
        var controller = new ErrorController();

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };

        var result = controller.HttpStatusCodeHandler(500);

        result.Should().BeAssignableTo<ViewResult>()
              .Which.ViewName.Should().Be("ProblemWithTheService");

        controller.ControllerContext.HttpContext.Response.StatusCode.Should().Be(500);
    }
}