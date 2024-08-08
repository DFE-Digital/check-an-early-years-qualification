using Dfe.EarlyYearsQualification.Web.Services.Cookies;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class CookieManagerTests
{
    [TestMethod]
    public void CookieManager_Set_CallsSetterOnContext()
    {
        var mockContext = new Mock<IHttpContextAccessor>();
        mockContext.Setup(c => c.HttpContext!.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(),
                                                                      It.IsAny<CookieOptions>()))
                   .Verifiable();

        var service = new CookieManager(mockContext.Object);

        service.SetOutboundCookie("key", "value", new CookieOptions());

        mockContext.Verify(c => c.HttpContext!.Response.Cookies.Append("key", "value", It.IsAny<CookieOptions>()));
    }

    [TestMethod]
    public void CookieManager_Set_OnNullHttpContext_Throws()
    {
        var mockContext = new Mock<IHttpContextAccessor>();
        mockContext.SetupGet(c => c.HttpContext)
                   .Returns((HttpContext?)null);

        var service = new CookieManager(mockContext.Object);

        var action = () => service.SetOutboundCookie("key", "value", new CookieOptions());

        action.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void CookieManager_Get_CallsGetterOnContext()
    {
        var cookies = new CookieCollection { { "key", "value" } };

        var mockContext = new Mock<IHttpContextAccessor>();
        mockContext.SetupGet(c => c.HttpContext!.Request.Cookies)
                   .Returns(cookies);

        var service = new CookieManager(mockContext.Object);

        var result = service.ReadInboundCookies();

        result.Should().ContainSingle(kvp => kvp.Key == "key" && kvp.Value == "value");
    }

    private class CookieCollection : Dictionary<string, string>, IRequestCookieCollection
    {
        private readonly Dictionary<string, string> _store;

        public CookieCollection()
        {
            _store = this;
        }

        public new ICollection<string> Keys
        {
            get { return _store.Keys; }
        }
    }
}