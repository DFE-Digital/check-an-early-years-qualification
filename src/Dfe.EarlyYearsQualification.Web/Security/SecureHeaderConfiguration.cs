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
                                           DirectiveOrUri = "sha256-VAoCuOmBv4C4V/WthoGzlhYyYpWir44ETG7WKh+3kG8="
                                       };

        var windowLocationShaCspElement = new ContentSecurityPolicyElement
                                          {
                                              CommandType = CspCommandType.Directive,
                                              DirectiveOrUri = "sha256-Om9RNNoMrdmIZzT4Oo7KaozVNUg6zYxVQuq3CPld2Ms="
                                          };

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

        var dropdownPageCheckbox = new ContentSecurityPolicyElement
                                   {
                                       CommandType = CspCommandType.Directive,
                                       DirectiveOrUri = "sha256-lD2YLKoqlgPJ6bMRB0gZKeUdZqwszfrRSmAnzX0TSls="
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

        var windowPrint = new ContentSecurityPolicyElement
                          {
                              CommandType = CspCommandType.Directive,
                              DirectiveOrUri = "sha256-1f+6vEGZewP7dkvrYIBD4bqMLOhumfg10mwfKd2jU7I="
                          };
        
        var challengePageShowPassword = new ContentSecurityPolicyElement
                          {
                              CommandType = CspCommandType.Directive,
                              DirectiveOrUri = "sha256-LBWtLNxa0f5+6KBUNLCp8JXVP7YuPtJtEt1Ku3cCKdY="
                          };

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
        
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(backButtonShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(cookiesPageShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(windowLocationShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(unsafeHashesElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(govukFrontendSupportedElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(govukAllMinifiedElement);
        configuration.ContentSecurityPolicyConfiguration.FrameAncestors.Add(contentfulCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(dropdownPageCheckbox);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(gtmCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(gtmInjectedScriptCspElement);
        configuration.ContentSecurityPolicyConfiguration.ConnectSrc.Add(ga4CspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(windowPrint);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(challengePageShowPassword);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(clarityCspElement);
        configuration.ContentSecurityPolicyConfiguration.ConnectSrc.Add(clarityConnectSourceCspElement);
        
        return configuration;
    }
}