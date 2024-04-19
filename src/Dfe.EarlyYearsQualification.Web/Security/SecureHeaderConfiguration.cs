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
        var backButtonShaCspElement = new ContentSecurityPolicyElement { CommandType = OwaspHeaders.Core.Enums.CspCommandType.Directive, DirectiveOrUri = "sha256-2eCA8tPChvVMeSRvRNqlmBco1wRmAKXWVzJ8Vpb9S6Y=" };
        var unsafeHashesElement = new ContentSecurityPolicyElement { CommandType = OwaspHeaders.Core.Enums.CspCommandType.Directive, DirectiveOrUri = "unsafe-hashes" };
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(backButtonShaCspElement);
        configuration.ContentSecurityPolicyConfiguration.ScriptSrc.Add(unsafeHashesElement);
        return configuration;
    }
}