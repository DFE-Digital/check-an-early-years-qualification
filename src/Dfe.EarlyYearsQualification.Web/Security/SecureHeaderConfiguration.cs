using OwaspHeaders.Core.Enums;
using OwaspHeaders.Core.Extensions;
using OwaspHeaders.Core.Helpers;
using OwaspHeaders.Core.Models;

namespace Dfe.EarlyYearsQualification.Web.Security;

public static class SecureHeaderConfiguration
{
    public static IApplicationBuilder UseCustomSecureHeaders(
        this IApplicationBuilder app,
        bool upgradeInsecureRequests = true)
    {
        return app.UseSecureHeadersMiddleware(opt =>
        {
            opt.UseHsts()
               .UseXFrameOptions()
               .UseContentTypeOptions()
               .UsePermittedCrossDomainPolicies()
               .UseReferrerPolicy()
               .UseCacheControl()
               .UseXssProtection()
               .UseCrossOriginResourcePolicy()
               .SetUrlsToIgnore(["/favicon.ico"]);

            if (upgradeInsecureRequests)
            {
                opt.UseDefaultContentSecurityPolicy();
            }
            else
            {
                opt.UseContentSecurityPolicy(upgradeInsecureRequests: false);
                opt.SetCspUris([ContentSecurityPolicyHelpers.CreateSelfDirective()], CspUriType.Object);
            }

            var govukFrontendSupportedElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Directive,
                DirectiveOrUri = "sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw="
            };

            var govukAllMinifiedElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Directive,
                DirectiveOrUri = "sha256-I3BKX8MX1uYLru6MvoWKLYAl5xMDeICsDiZsq0ZSlDA="
            };

            var unsafeHashesElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Directive,
                DirectiveOrUri = "unsafe-hashes"
            };

            var contentfulCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://app.contentful.com"
            };

            var gtmCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://www.googletagmanager.com/gtm.js"
            };

            var gtmInjectedScriptCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://www.googletagmanager.com/gtag/js"
            };

            var ga4CspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "*.google-analytics.com"
            };

            var clarityCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://*.clarity.ms"
            };

            var bingClarityCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://c.bing.com"
            };

            var clarityConnectSourceCspElement = new ContentSecurityPolicyElement
            {
                CommandType = CspCommandType.Uri,
                DirectiveOrUri = "https://*.clarity.ms/collect"
            };
            
            opt.SetCspUris( 
                           [
                               ContentSecurityPolicyHelpers.CreateSelfDirective(),
                               unsafeHashesElement,
                               govukFrontendSupportedElement,
                               govukAllMinifiedElement,
                               gtmCspElement,
                               gtmInjectedScriptCspElement,
                               clarityCspElement,
                               bingClarityCspElement
                           ],
                CspUriType.Script
            );

            opt.SetCspUris(
                [
                    ContentSecurityPolicyHelpers.CreateSelfDirective(),
                    ga4CspElement,
                    clarityConnectSourceCspElement
                ],
                CspUriType.Connect
            );

            opt.SetCspUris(
                [
                    contentfulCspElement
                ],
                CspUriType.FrameAncestors
            );
        });
    }
}