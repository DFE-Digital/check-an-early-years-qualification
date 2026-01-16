using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Dfe.EarlyYearsQualification.UnitTests.Attributes;

[TestClass]
public class LogAntiForgeryFailureAttributeTests
{
    [TestMethod]
    public void OnResultExecuting_ResultIsNotIAntiforgeryValidationFailedResult_LoggerNotCalled()
    {
        var context = CreateContext(new AcceptedResult());
        var mockLogger = new Mock<ILogger<LogAntiForgeryFailureAttribute>>();
        
        var attribute = new LogAntiForgeryFailureAttribute(mockLogger.Object);
        attribute.OnResultExecuting(context);
        
        mockLogger.Invocations.Count.Should().Be(0);
        context.Result.Should().BeOfType<AcceptedResult>();
    }
    
    [TestMethod]
    public void OnResultExecuting_ResultIsIAntiforgeryValidationFailedResult_LoggerCalled()
    {
        var context = CreateContext(new AntiforgeryValidationFailedResult());
        var mockLogger = new Mock<ILogger<LogAntiForgeryFailureAttribute>>();
        
        var attribute = new LogAntiForgeryFailureAttribute(mockLogger.Object);
        attribute.OnResultExecuting(context);

        mockLogger.Invocations.Count.Should().Be(1);
        mockLogger.VerifyError("The antiforgery token was not validated successfully.");
        context.Result.Should().BeOfType<ViewResult>();
        var actionResult = context.Result as ViewResult;
        actionResult.Should().NotBeNull();
        actionResult.ViewName.Should().Be("ProblemWithTheService");
        actionResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    private static ResultExecutingContext CreateContext(IActionResult actionResult)
    {
        var mockContentService = new Mock<IContentService>();
        var mockControllerLogger = new Mock<ILogger<AccessibilityStatementController>>();
        var mockAccessibilityStatementMapper = new Mock<IAccessibilityStatementMapper>();

        var controller =
            new AccessibilityStatementController(mockControllerLogger.Object, mockContentService.Object,
                                                 mockAccessibilityStatementMapper.Object);
        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       };
        
        return new ResultExecutingContext(new ActionContext(controller.HttpContext, new RouteData(), new ActionDescriptor()), [], actionResult, controller);
    }
}