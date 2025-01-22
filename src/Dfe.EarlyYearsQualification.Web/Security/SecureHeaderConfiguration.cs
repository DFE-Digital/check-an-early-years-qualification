using OwaspHeaders.Core.Enums;
using OwaspHeaders.Core.Extensions;
using OwaspHeaders.Core.Models;

namespace Dfe.EarlyYearsQualification.Web.Security;

public static class SecureHeaderConfiguration
{
    public static SecureHeadersMiddlewareConfiguration CustomConfiguration()
    {
        // These are the default ones, see here: https://github.com/GaProgMan/OwaspHeaders.Core
        var configuration = SecureHeadersMiddlewareBuilder
                            .CreateBuilder()
                            .UseHsts()
                            .UseXFrameOptions()
                            .UseContentTypeOptions()
                            .UseDefaultContentSecurityPolicy()
                            .UsePermittedCrossDomainPolicies()
                            .UseReferrerPolicy()
                            .UseCacheControl()
                            .UseXssProtection()
                            .UseCrossOriginResourcePolicy()
                            .SetUrlsToIgnore(["/assets/images/og-image.png", "/favicon.ico"])
                            .Build();

        var govukFrontendSupportedElement = new ContentSecurityPolicyElement
                                            {
                                                CommandType = CspCommandType.Directive,
                                                DirectiveOrUri = "sha256-GUQ5ad8JK5KmEWmROf3LZd9ge94daqNvd8xy9YS1iDw="
                                            };
        
        var govukAllMinifiedElement = new ContentSecurityPolicyElement
                                      {
                                          CommandType = CspCommandType.Directive,
                                          DirectiveOrUri = "sha256-l5MP+9OapFXGxjKMNj/89ExAW2TvAFFoADrbsmtSJXo="
                                      };

        var unsafeHashesElement = new ContentSecurityPolicyElement
                                  { CommandType = CspCommandType.Directive, DirectiveOrUri = "unsafe-hashes" };
        
        var contentfulCspElement = new ContentSecurityPolicyElement
                                   { CommandType = CspCommandType.Uri, DirectiveOrUri = "https://app.contentful.com" };
        
        
        var gtmCspElement = new ContentSecurityPolicyElement
                                   { CommandType = CspCommandType.Uri, DirectiveOrUri = "https://www.googletagmanager.com/gtm.js" };
        
        var gtmInjectedScriptCspElement = new ContentSecurityPolicyElement
                            { CommandType = CspCommandType.Uri, DirectiveOrUri = "https://www.googletagmanager.com/gtag/js" };
        
        var ga4CspElement = new ContentSecurityPolicyElement
                            { CommandType = CspCommandType.Uri, DirectiveOrUri = "*.google-analytics.com" };

        var clarityCspElement = new ContentSecurityPolicyElement
                                {
                                    CommandType = CspCommandType.Uri,
                                    DirectiveOrUri = "https://www.clarity.ms/ https://c.bing.com"
                                };

        var clarityConnectSourceCspElement = new ContentSecurityPolicyElement
                                             {
                                                 CommandType = CspCommandType.Uri,
                                                 DirectiveOrUri = "https://*.clarity.ms/collect"
                                             };
        
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(unsafeHashesElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(govukFrontendSupportedElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(govukAllMinifiedElement);
        configuration.ContentSecurityPolicyConfiguration.FrameAncestors.Add(contentfulCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(gtmCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(gtmInjectedScriptCspElement);
        configuration.ContentSecurityPolicyConfiguration.ConnectSrc.Add(ga4CspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(clarityCspElement);
        configuration.ContentSecurityPolicyConfiguration.ConnectSrc.Add(clarityConnectSourceCspElement);
        
        return configuration;
    }
}