using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Filters;

[TestClass]
public class ApplicationInsightsActionFilterAttributeTests
{
    private const string FromFormParameterName = "Parameter Name";
    private const string CookieValueKey = "Cookie-Value";
    private const string RequestBodyKey = "Request-Body";
    
    [TestMethod]
    public async Task OnActionExecutionAsync_NotPostMethod_Returns()
    {
        var mockRequestTelemetry = new RequestTelemetry();
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        
        mockHttpRequest.SetupGet(r => r.Method).Returns(HttpMethods.Get);
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        
        var filter = new ApplicationInsightsActionFilterAttribute(NullLogger<ApplicationInsightsActionFilterAttribute>.Instance);
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              new Mock<ActionDescriptor>().Object,
                                              new ModelStateDictionary()
                                             );

        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                new Dictionary<string, object?>(),
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);
        
        mockRequestTelemetry.Properties.Should().BeEmpty();
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }
    
    [TestMethod]
    public async Task OnActionExecutionAsync_NoFromFormParameter_Returns()
    {
        var mockRequestTelemetry = new RequestTelemetry();
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        
        mockHttpRequest.SetupGet(r => r.Method).Returns(HttpMethods.Post);
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        
        var filter = new ApplicationInsightsActionFilterAttribute(NullLogger<ApplicationInsightsActionFilterAttribute>.Instance);
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              new Mock<ActionDescriptor>().Object,
                                              new ModelStateDictionary()
                                             );

        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                new Dictionary<string, object?>(),
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);
        
        mockRequestTelemetry.Properties.Should().BeEmpty();
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }
    
    [TestMethod]
    public async Task OnActionExecutionAsync_HasFromFormParameter_AddsCookieToRequestTelemetry()
    {
        const string cookieValue = "Fake cookie value";
        
        var mockRequestTelemetry = new RequestTelemetry();
        var mockCookiesCollection = new Mock<IRequestCookieCollection>();
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        
        mockCookiesCollection.Setup(c => c[CookieKeyNames.UserJourneyKey]).Returns(cookieValue);
        mockCookiesCollection.Setup(x => x.ContainsKey(CookieKeyNames.UserJourneyKey)).Returns(true);
        mockHttpRequest.SetupGet(x => x.Cookies).Returns(mockCookiesCollection.Object);
        mockHttpRequest.SetupGet(r => r.Method).Returns(HttpMethods.Post);
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        mockHttpContext.Setup(x => x.Features).Returns(mockFeatureCollection.Object);
        
        var filter = new ApplicationInsightsActionFilterAttribute(NullLogger<ApplicationInsightsActionFilterAttribute>.Instance);
        
        var mockActionDescriptor = new ActionDescriptor
                                   {
                                       Parameters = new List<ParameterDescriptor>
                                                    {
                                                        new ()
                                                        {
                                                            BindingInfo = new BindingInfo
                                                                          {
                                                                              BindingSource = BindingSource.Form
                                                                          },
                                                            Name = FromFormParameterName
                                                        }
                                                    }
                                   };
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              mockActionDescriptor,
                                              new ModelStateDictionary()
                                             );

        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                new Dictionary<string, object?>(),
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);

        mockRequestTelemetry.Properties.Count.Should().Be(1);
        mockRequestTelemetry.Properties.Keys.Should().Contain(CookieValueKey);
        mockRequestTelemetry.Properties.Values.First().Should().Be(cookieValue);
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }
    
    [TestMethod]
    public async Task OnActionExecutionAsync_CookieNotFound_RequestTelemetryCookieValueIsNotFound()
    {
        var mockRequestTelemetry = new RequestTelemetry();
        var mockCookiesCollection = new Mock<IRequestCookieCollection>();
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        
        mockCookiesCollection.Setup(x => x.ContainsKey(CookieKeyNames.UserJourneyKey)).Returns(false);
        mockHttpRequest.SetupGet(x => x.Cookies).Returns(mockCookiesCollection.Object);
        mockHttpRequest.SetupGet(r => r.Method).Returns(HttpMethods.Post);
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        mockHttpContext.Setup(x => x.Features).Returns(mockFeatureCollection.Object);
        
        var filter = new ApplicationInsightsActionFilterAttribute(NullLogger<ApplicationInsightsActionFilterAttribute>.Instance);
        
        var mockActionDescriptor = new ActionDescriptor
                                   {
                                       Parameters = new List<ParameterDescriptor>
                                                    {
                                                        new ()
                                                        {
                                                            BindingInfo = new BindingInfo
                                                                          {
                                                                              BindingSource = BindingSource.Form
                                                                          },
                                                            Name = FromFormParameterName
                                                        }
                                                    }
                                   };
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              mockActionDescriptor,
                                              new ModelStateDictionary()
                                             );

        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                new Dictionary<string, object?>(),
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);

        mockRequestTelemetry.Properties.Count.Should().Be(1);
        mockRequestTelemetry.Properties.Keys.Should().Contain(CookieValueKey);
        mockRequestTelemetry.Properties.Values.First().Should().Be("Not found");
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }

    [TestMethod]
    public async Task OnActionExecutionAsync_SerialisesRequestBody_AddsRequestBodyToRequestTelemetry()
    {
        const string cookieValue = "Fake cookie value";
        
        var mockRequestTelemetry = new RequestTelemetry();
        var mockCookiesCollection = new Mock<IRequestCookieCollection>();
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        
        mockCookiesCollection.Setup(c => c[CookieKeyNames.UserJourneyKey]).Returns(cookieValue);
        mockCookiesCollection.Setup(x => x.ContainsKey(CookieKeyNames.UserJourneyKey)).Returns(true);
        mockHttpRequest.SetupGet(x => x.Cookies).Returns(mockCookiesCollection.Object);
        mockHttpRequest.SetupGet(r => r.Method).Returns(HttpMethods.Post);
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        mockHttpContext.Setup(x => x.Features).Returns(mockFeatureCollection.Object);
        
        var filter = new ApplicationInsightsActionFilterAttribute(NullLogger<ApplicationInsightsActionFilterAttribute>.Instance);
        
        var mockActionDescriptor = new ActionDescriptor
                                   {
                                       Parameters = new List<ParameterDescriptor>
                                                    {
                                                        new ()
                                                        {
                                                            BindingInfo = new BindingInfo
                                                                          {
                                                                              BindingSource = BindingSource.Form
                                                                          },
                                                            Name = FromFormParameterName
                                                        }
                                                    }
                                   };
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              mockActionDescriptor,
                                              new ModelStateDictionary()
                                             );

        var actionArguments = new Dictionary<string, object?>
                              { { FromFormParameterName, new GetHelpPageViewModel
                                                         {
                                                             SelectedOption = "This is the selected option"
                                                         } } };
        
        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                actionArguments,
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);

        mockRequestTelemetry.Properties.Count.Should().Be(2);
        mockRequestTelemetry.Properties.Keys.Should().Contain(CookieValueKey);
        mockRequestTelemetry.Properties.TryGetValue(CookieValueKey, out var requestCookieValue); 
        requestCookieValue.Should().Be(cookieValue);
        mockRequestTelemetry.Properties.Keys.Should().Contain(RequestBodyKey);
        mockRequestTelemetry.Properties.TryGetValue(RequestBodyKey, out var requestBodyValue); 
        const string expectedRequestBodyValue = "{\"SelectedOption\":\"This is the selected option\"}";
        requestBodyValue.Should().Match(expectedRequestBodyValue);
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }

    [TestMethod]
    public async Task OnActionExecutionAsync_ExceptionThrown_CallsLogger()
    {
        var mockHttpRequest = new Mock<HttpRequest>();
        var mockHttpContext = new Mock<HttpContext>();
        var mockActionExecutionDelegate = new Mock<ActionExecutionDelegate>();
        var mockLogger = new Mock<ILogger<ApplicationInsightsActionFilterAttribute>>();

        mockHttpRequest.SetupGet(r => r.Method).Throws(new Exception("Test exception"));
        mockHttpContext.SetupGet(c => c.Request).Returns(mockHttpRequest.Object);
        
        var filter = new ApplicationInsightsActionFilterAttribute(mockLogger.Object);
        
        var mockActionDescriptor = new ActionDescriptor();
        var actionContext = new ActionContext(
                                              mockHttpContext.Object,
                                              new Mock<RouteData>().Object,
                                              mockActionDescriptor,
                                              new ModelStateDictionary()
                                             );
        
        var actionExecutingContext = new ActionExecutingContext(
                                                                actionContext,
                                                                new List<IFilterMetadata>(),
                                                                new Dictionary<string, object?>(),
                                                                new Mock<HomeController>()
                                                               );
        
        await filter.OnActionExecutionAsync(actionExecutingContext, mockActionExecutionDelegate.Object);
        
        mockLogger.VerifyCritical("Error executing the application insights telemetry task");
        mockActionExecutionDelegate.Verify(x => x.Invoke(), Times.Once);
    }
}