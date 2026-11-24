using Dfe.EarlyYearsQualification.Web.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class SecureHeaderConfigurationTests
{
    
    [TestMethod]
    public async Task UseCustomSecureHeaders_WithUpgradeInsecureRequestsTrue_SetsExpectedCspAndSecurityHeaders()
    {
        // Arrange
        using var host = await CreateTestHostAsync(upgradeInsecureRequests: true);
        var client = host.GetTestClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.IsTrue(response.Headers.TryGetValues("Content-Security-Policy", out var cspValues));

        var csp = cspValues!.Single();

        // Verify upgrade-insecure-requests is enabled
        Assert.Contains("upgrade-insecure-requests", csp, $"Expected 'upgrade-insecure-requests' in CSP: {csp}");

        // Verify script-src directives and URIs
        const string expectedScriptSrc =
            "script-src 'self' 'unsafe-hashes' " +
            "'sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw=' " +
            "'sha256-I3BKX8MX1uYLru6MvoWKLYAl5xMDeICsDiZsq0ZSlDA=' " +
            "https://www.googletagmanager.com/gtm.js " +
            "https://www.googletagmanager.com/gtag/js " +
            "https://*.clarity.ms " +
            "https://c.bing.com";

        Assert.Contains(expectedScriptSrc, csp, $"Expected script-src not found in CSP: {csp}");

        // Verify connect-src directives and URIs
        const string expectedConnectSrc = "connect-src 'self' *.google-analytics.com https://*.clarity.ms/collect";
        Assert.Contains(expectedConnectSrc, csp, $"Expected connect-src not found in CSP: {csp}");

        // Verify frame-ancestors
        const string expectedFrameAncestors = "frame-ancestors https://app.contentful.com";
        Assert.Contains(expectedFrameAncestors, csp, $"Expected frame-ancestors not found in CSP: {csp}");

        // Verify standard OWASP headers are present
        Assert.IsTrue(response.Headers.Contains("X-Frame-Options"));
        Assert.IsTrue(response.Headers.Contains("X-Content-Type-Options"));
        Assert.IsTrue(response.Headers.Contains("Referrer-Policy"));
        Assert.IsTrue(response.Headers.Contains("Cross-Origin-Resource-Policy"));
    }

    [TestMethod]
    public async Task UseCustomSecureHeaders_WithUpgradeInsecureRequestsFalse_OmitsUpgradeDirectiveAndSetsObjectSrcSelf()
    {
        // Arrange
        using var host = await CreateTestHostAsync(upgradeInsecureRequests: false);
        var client = host.GetTestClient();

        // Act
        var response = await client.GetAsync("/");
        var csp = response.Headers.GetValues("Content-Security-Policy").Single();

        // Assert
        Assert.DoesNotContain("upgrade-insecure-requests", csp, $"Did not expect 'upgrade-insecure-requests' in CSP: {csp}");
        Assert.Contains("object-src 'self'", csp, $"Expected \"object-src 'self'\" in CSP: {csp}");
        Assert.Contains("script-src 'self'", csp, $"Expected \"script-src 'self'\" in CSP: {csp}");
    }

    [TestMethod]
    public async Task UseCustomSecureHeaders_ForIgnoredUrl_DoesNotAttachSecurityHeaders()
    {
        // Arrange
        using var host = await CreateTestHostAsync(upgradeInsecureRequests: true);
        var client = host.GetTestClient();

        // Act
        var response = await client.GetAsync("/favicon.ico");

        // Assert
        Assert.IsFalse(response.Headers.Contains("Content-Security-Policy"));
        Assert.IsFalse(response.Headers.Contains("X-Frame-Options"));
    }
    
    private static async Task<IHost> CreateTestHostAsync(bool upgradeInsecureRequests)
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
                              {
                                  webHost.UseTestServer();
                                  webHost.Configure(app =>
                                                    {
                                                        app.UseCustomSecureHeaders(upgradeInsecureRequests);

                                                        app.Run(async context =>
                                                                {
                                                                    context.Response.StatusCode = 200;
                                                                    await context.Response.WriteAsync("OK");
                                                                });
                                                    });
                              });

        return await hostBuilder.StartAsync();
    }
}