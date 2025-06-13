using Dfe.EarlyYearsQualification.Web.Services.HeadHandling;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class HeadHandlingMiddlewareTests
{
    [TestMethod]
    public void Constructor_WhenNextIsNull_ThrowsArgumentNullException()
    {
        RequestDelegate next = null!;

        Action act = () => { _ = new HeadHandlingMiddleware(next); };

        act.Should().Throw<ArgumentNullException>().WithParameterName("next");
    }

    [TestMethod]
    public async Task Invoke_WhenHeadRequest_SwitchesToGetAndBack()
    {
        var mockRequestTelemetry = new RequestTelemetry();
        var mockHttpRequest = new Mock<HttpRequest>();
        mockHttpRequest.SetupAllProperties();
        var mockHttpResponse = new Mock<HttpResponse>();
        mockHttpResponse.SetupAllProperties();
        var mockHttpContext = new Mock<HttpContext>();
        var mockFeatureCollection = new Mock<IFeatureCollection>();
        
        mockFeatureCollection.Setup(x => x.Get<RequestTelemetry>()).Returns(mockRequestTelemetry);
        mockHttpContext.Setup(x => x.Features).Returns(mockFeatureCollection.Object);

        var httpRequest = mockHttpRequest.Object;
        httpRequest.Method = HttpMethods.Head;

        var httpResponse = mockHttpResponse.Object;
        httpResponse.Body = Stream.Synchronized(new MemoryStream());
        
        mockHttpContext.SetupGet(c => c.Request).Returns(httpRequest);
        mockHttpContext.SetupGet(c => c.Response).Returns(httpResponse);

        var nextCalled = false;
        var middleware = new HeadHandlingMiddleware(ctx =>
                                                    {
                                                        nextCalled = true;
                                                        ctx.Request.Method.Should().Be(HttpMethods.Get);
                                                        return Task.CompletedTask;
                                                    });

        await middleware.Invoke(mockHttpContext.Object);

        nextCalled.Should().BeTrue();
        httpRequest.Method.Should().Be(HttpMethods.Head);
        mockHttpContext.Object.Response.Body.Should().BeSameAs(Stream.Null);
        
        mockRequestTelemetry.Properties.Count.Should().Be(1);
        mockRequestTelemetry.Properties.Keys.Should().Contain("WasHEADRequest");
        mockRequestTelemetry.Properties.Values.First().Should().Be("true");
    }

    [TestMethod]
    public async Task Invoke_WhenGetRequest_DoesNotModifyRequest()
    {
        var context = new DefaultHttpContext
                      {
                          Request =
                          {
                              Method = HttpMethods.Get
                          }
                      };
        var originalBody = context.Response.Body;
        
        var nextCalled = false;
        var middleware = new HeadHandlingMiddleware(ctx =>
                                                    {
                                                        nextCalled = true;
                                                        ctx.Request.Method.Should().Be(HttpMethods.Get);
                                                        return Task.CompletedTask;
                                                    });

        await middleware.Invoke(context);

        nextCalled.Should().BeTrue();
        context.Request.Method.Should().Be(HttpMethods.Get);
        context.Response.Body.Should().BeSameAs(originalBody);
    }
}