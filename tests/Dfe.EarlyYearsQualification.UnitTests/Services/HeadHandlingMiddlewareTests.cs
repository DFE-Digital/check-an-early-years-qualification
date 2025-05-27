using Dfe.EarlyYearsQualification.Web.Services.HeadHandling;
using Microsoft.AspNetCore.Http;

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
        var context = new DefaultHttpContext
                      {
                          Request =
                          {
                              Method = HttpMethods.Head
                          }
                      };
        
        var nextCalled = false;
        var middleware = new HeadHandlingMiddleware(ctx =>
                                                    {
                                                        nextCalled = true;
                                                        ctx.Request.Method.Should().Be(HttpMethods.Get);
                                                        return Task.CompletedTask;
                                                    });

        await middleware.Invoke(context);

        nextCalled.Should().BeTrue();
        context.Request.Method.Should().Be(HttpMethods.Head);
        context.Response.Body.Should().BeSameAs(Stream.Null);
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