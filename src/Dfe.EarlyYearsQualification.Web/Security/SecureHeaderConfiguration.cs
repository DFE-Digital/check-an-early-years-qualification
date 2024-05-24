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
                            .UseContentDefaultSecurityPolicy()
                            .UsePermittedCrossDomainPolicies()
                            .UseReferrerPolicy()
                            .UseCacheControl()
                            .RemovePoweredByHeader()
                            .UseXssProtection()
                            .UseCrossOriginResourcePolicy()
                            .Build();

        // This is to extend the ScriptSrc to allow the javascript inline code for the back button on the journey pages.
        var backButtonShaCspElement = new ContentSecurityPolicyElement
                                      {
                                          CommandType = CspCommandType.Directive,
                                          DirectiveOrUri = "sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y="
                                      };
        var cookiesPageShaCspElement = new ContentSecurityPolicyElement
                                       {
                                           CommandType = CspCommandType.Directive,
                                           DirectiveOrUri = "sha256-oRMkcVFF9H5r4SaWLpGJnNejhYNTeLE1GpA4sFvNcjE="
                                       };
        
        var WindowLocationShaCspElement = new ContentSecurityPolicyElement
                                        {
                                          CommandType = CspCommandType.Directive,
                                          DirectiveOrUri = "sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms="
                                        };

        var unsafeHashesElement = new ContentSecurityPolicyElement
                                  { CommandType = CspCommandType.Directive, DirectiveOrUri = "unsafe-hashes" };
        var contentfulCspElement = new ContentSecurityPolicyElement
                                   { CommandType = CspCommandType.Uri, DirectiveOrUri = "https://app.contentful.com" };
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(backButtonShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(cookiesPageShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(WindowLocationShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(unsafeHashesElement);
        configuration.ContentSecurityPolicyConfiguration.FrameAncestors.Add(contentfulCspElement);
        return configuration;
    }
}