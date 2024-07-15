using System.Diagnostics;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Error;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ErrorControllerTests
{
    [TestMethod]
    public void GetError_ReturnsViewWithTraceId()
    {
        var controller = new ErrorController();

        const string traceIdentifier = "Trace";

        controller.ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext
                                                         {
                                                             TraceIdentifier = traceIdentifier
                                                         }
                                       };

        var result = controller.Index();

        result.Should().BeAssignableTo<ViewResult>()
              .Which.Model.Should().BeAssignableTo<ErrorViewModel>()
              .Which.RequestId.Should().Be(traceIdentifier);
    }

    [TestMethod]
    public void GetError_ReturnsViewWithActivityId()
    {
        var controller = new ErrorController();

        const string operationName = "UnitTest";

        var activity = new Activity(operationName).Start();

        try
        {
            var result = controller.Index();

            result.Should().BeAssignableTo<ViewResult>()
                  .Which.Model.Should().BeAssignableTo<ErrorViewModel>()
                  .Which.RequestId.Should().Be(activity.Id);
        }
        finally
        {
            activity.Stop();
        }
    }
}