using Dfe.EarlyYearsQualification.Web.Security;
using FluentAssertions;
using OwaspHeaders.Core.Enums;
using OwaspHeaders.Core.Extensions;

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
        config.RemoveXPoweredByHeader.Should().BeTrue();
        config.UseCrossOriginResourcePolicy.Should().BeTrue();
        config.HstsConfiguration.IncludeSubDomains.Should().BeTrue();
        config.HstsConfiguration.MaxAge.Should().Be(63072000);
        config.XFrameOptionsConfiguration.OptionValue.Should().Be(XFrameOptions.Deny);
        config.XFrameOptionsConfiguration.AllowFromDomain.Should().BeNull();
        config.ContentSecurityPolicyConfiguration.BaseUri.Should().BeEmpty();
        config.ContentSecurityPolicyConfiguration.DefaultSrc.Should().BeEmpty();
    }
}