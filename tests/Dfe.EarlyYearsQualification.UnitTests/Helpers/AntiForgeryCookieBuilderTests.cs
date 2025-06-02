using Dfe.EarlyYearsQualification.Web.Helpers;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class AntiForgeryCookieBuilderTests
{
    [TestMethod]
    public void Build_PassInContext_SecureFlagIsTrue()
    {
        var builder = new AntiForgeryCookieBuilder
                      {
                          Name = ".AspNetCore.Antiforgery",
                          SameSite = SameSiteMode.Strict,
                          HttpOnly = true,
                          IsEssential = true,
                          SecurePolicy = CookieSecurePolicy.None
                      };

        var result = builder.Build(new DefaultHttpContext(), DateTimeOffset.Now);
        result.Secure.Should().BeTrue();
    }
}