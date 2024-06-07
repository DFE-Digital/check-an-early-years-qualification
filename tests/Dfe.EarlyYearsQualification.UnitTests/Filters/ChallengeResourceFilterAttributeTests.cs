using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Filters;

[TestClass]
public class ChallengeResourceFilterAttributeTests
{
    [TestMethod]
    public void ExecuteFilter_NoSecretValue_RedirectsToChallenge()
    {
        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance);

        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost"),
                                  Path = "/start"
                              }
                          };

        var actionContext = new ActionContext(httpContext,
                                              new RouteData(),
                                              new ActionDescriptor(),
                                              new ModelStateDictionary());

        var resourceExecutingContext = new ResourceExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    new List<IValueProviderFactory>());

        filter.OnResourceExecuting(resourceExecutingContext);

        var result = resourceExecutingContext.Result;

        result.Should().BeAssignableTo<RedirectToActionResult>();

        var redirect = (RedirectToActionResult)result!;

        redirect.ActionName.Should().Be("Index");
        redirect.ControllerName.Should().Be("Challenge");
        redirect.RouteValues.Should().ContainKey("redirectAddress");
        redirect.Permanent.Should().BeFalse();
        redirect.PreserveMethod.Should().BeFalse();
    }

    [TestMethod]
    public void ExecuteFilter_NoSecretValue_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(mockLogger.Object);

        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost"),
                                  Path = "/start"
                              }
                          };

        var actionContext = new ActionContext(httpContext,
                                              new RouteData(),
                                              new ActionDescriptor(),
                                              new ModelStateDictionary());

        var resourceExecutingContext = new ResourceExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    new List<IValueProviderFactory>());

        filter.OnResourceExecuting(resourceExecutingContext);

        mockLogger.VerifyWarning($"Access denied by {nameof(ChallengeResourceFilterAttribute)}");
    }

    [TestMethod]
    public void ExecuteFilter_CorrectSecretValue_PassesThrough()
    {
        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance);

        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost"),
                                  Path = "/start"
                              }
                          };

        var cookie = new[]
                     {
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}={ChallengeResourceFilterAttribute.Challenge}"
                     };

        httpContext.Request.Headers["Cookie"] = cookie;

        var actionContext = new ActionContext(httpContext,
                                              new RouteData(),
                                              new ActionDescriptor(),
                                              new ModelStateDictionary());

        var resourceExecutingContext = new ResourceExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    new List<IValueProviderFactory>());

        filter.OnResourceExecuting(resourceExecutingContext);

        resourceExecutingContext.Result.Should().BeNull();
    }

    [TestMethod]
    public void ExecuteFilter_IncorrectSecretValue_RedirectsToChallenge()
    {
        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance);

        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost"),
                                  Path = "/start"
                              }
                          };

        var cookie = new[]
                     {
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}=not-{ChallengeResourceFilterAttribute.Challenge}"
                     };

        httpContext.Request.Headers["Cookie"] = cookie;

        var actionContext = new ActionContext(httpContext,
                                              new RouteData(),
                                              new ActionDescriptor(),
                                              new ModelStateDictionary());

        var resourceExecutingContext = new ResourceExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    new List<IValueProviderFactory>());

        filter.OnResourceExecuting(resourceExecutingContext);

        var result = resourceExecutingContext.Result;

        result.Should().BeAssignableTo<RedirectToActionResult>();

        var redirect = (RedirectToActionResult)result!;

        redirect.ActionName.Should().Be("Index");
        redirect.ControllerName.Should().Be("Challenge");
        redirect.RouteValues.Should().ContainKey("redirectAddress");
        redirect.Permanent.Should().BeFalse();
        redirect.PreserveMethod.Should().BeFalse();
    }

    [TestMethod]
    public void ExecuteFilter_IncorrectSecretValue_LogsWarning()
    {
        var logger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(logger.Object);

        var httpContext = new DefaultHttpContext
                          {
                              Request =
                              {
                                  Scheme = "https",
                                  Host = new HostString("localhost"),
                                  Path = "/start"
                              }
                          };

        var cookie = new[]
                     {
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}=not-{ChallengeResourceFilterAttribute.Challenge}"
                     };

        httpContext.Request.Headers["Cookie"] = cookie;

        var actionContext = new ActionContext(httpContext,
                                              new RouteData(),
                                              new ActionDescriptor(),
                                              new ModelStateDictionary());

        var resourceExecutingContext = new ResourceExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    new List<IValueProviderFactory>());

        filter.OnResourceExecuting(resourceExecutingContext);

        logger.VerifyWarning($"Access denied by {nameof(ChallengeResourceFilterAttribute)} (incorrect value submitted)");
    }
}