using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Helpers;
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
        const string accessKey = "CX";

        var accessKeys = new List<string>
                  {
                      accessKey
                  };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);
        
        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance,
                                                          accessKeysHelper.Object);
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
        const string accessKey = "CX";

        var accessKeys = new List<string>
                         {
                             accessKey
                         };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var mockLogger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(mockLogger.Object, accessKeysHelper.Object);

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
    public void ExecuteFilter_CorrectSecretValue1_PassesThrough()
    {
        const string accessKey = "CX";

        var accessKeys = new List<string>
                         {
                             accessKey
                         };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance,
                                                          accessKeysHelper.Object);

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
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}={accessKey}"
                     };

        httpContext.Request.Headers.Cookie = cookie;

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
    public void ExecuteFilter_CorrectSecretValue2_PassesThrough()
    { 
        const string accessKey = "CX";

        var accessKeys = new List<string>
                         {
                             "SomeKey", // <== NB, not using the first key in the array 
                             accessKey
                         };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance,
                                                          accessKeysHelper.Object);

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
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}={accessKey}"
                     };

        httpContext.Request.Headers.Cookie = cookie;

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
        const string accessKey = "CX";

        var accessKeys = new List<string>
                         {
                             accessKey
                         };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance,
                                                          accessKeysHelper.Object);

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
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}=not-{accessKey}"
                     };

        httpContext.Request.Headers.Cookie = cookie;

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
        const string accessKey = "CX";

        var accessKeys = new List<string>
                         {
                             accessKey
                         };
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(accessKeys);

        var logger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(logger.Object, accessKeysHelper.Object);

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
                         $"{ChallengeResourceFilterAttribute.AuthSecretCookieName}=not-{accessKey}"
                     };

        httpContext.Request.Headers.Cookie = cookie;

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

    [TestMethod]
    public void Filter_NoSecretsConfigured_RedirectsToError()
    {
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(new List<string>());

        var logger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(logger.Object, accessKeysHelper.Object);


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
        redirect.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public void Filter_NoSecretsConfigured_LogsError()
    {
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.ConfiguredKeys).Returns(new List<string>());

        var mockLogger = new Mock<ILogger<ChallengeResourceFilterAttribute>>();

        var filter = new ChallengeResourceFilterAttribute(mockLogger.Object, accessKeysHelper.Object);

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

        mockLogger.VerifyError("Service access keys not configured");
    }

    [TestMethod]
    public void ExecuteFilter_AllowPublicAccess_PassesThrough()
    {
        
        var accessKeysHelper = new Mock<ICheckServiceAccessKeysHelper>();
        accessKeysHelper.Setup(x => x.AllowPublicAccess).Returns(true);

        var filter = new ChallengeResourceFilterAttribute(NullLogger<ChallengeResourceFilterAttribute>.Instance,
                                                          accessKeysHelper.Object);

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

        result.Should().BeNull();
    }
}