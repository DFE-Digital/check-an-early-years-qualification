using Dfe.EarlyYearsQualification.Web.Security;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class SecureHeaderConfigurationTests
{
    [TestMethod]
    public void SecureHeaderConfiguration_ExplicitConfiguration()
    {
        var config = SecureHeaderConfiguration.CustomConfiguration();

        config.UseHsts.Should().BeTrue();
        config.UseXFrameOptions.Should().BeTrue();
        config.UseXssProtection.Should().BeTrue();
        config.UseXContentTypeOptions.Should().BeTrue();
        config.UseContentSecurityPolicy.Should().BeTrue();
        config.UseContentSecurityPolicyReportOnly.Should().BeFalse();
        config.UseXContentSecurityPolicy.Should().BeFalse();
        config.UsePermittedCrossDomainPolicy.Should().BeTrue();
        config.UseReferrerPolicy.Should().BeTrue();
        config.UseExpectCt.Should().BeFalse();
        config.UseCacheControl.Should().BeTrue();
        config.UseCrossOriginResourcePolicy.Should().BeTrue();
        config.HstsConfiguration.Should().NotBeNull();
        var hstsHeaderValue = config.HstsConfiguration.BuildHeaderValue();
        hstsHeaderValue.Should().NotBeNull();
        hstsHeaderValue.Should().Contain("max-age=31536000");
        hstsHeaderValue.Should().Contain("includeSubDomains");
        config.XFrameOptionsConfiguration.Should().NotBeNull();
        var xFrameOptionHeaderValue = config.XFrameOptionsConfiguration.BuildHeaderValue();
        xFrameOptionHeaderValue.Should().NotBeNull();
        xFrameOptionHeaderValue.Should().Contain("deny");
        config.XFrameOptionsConfiguration.AllowFromDomain.Should().BeNull();
        config.ContentSecurityPolicyConfiguration.BaseUri.Should().BeEmpty();
        config.ContentSecurityPolicyConfiguration.DefaultSrc.Should().BeEmpty();
    }
}