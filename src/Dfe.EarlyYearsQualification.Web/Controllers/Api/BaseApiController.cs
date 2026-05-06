using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Api;

public abstract class BaseApiController<T>(ILogger<T> logger,
                               IConfiguration configuration) : Controller
{
    protected abstract string AuthSecretKey { get; }
    
    protected abstract string ExpectedAuthSecretSectionName { get; }
    
    protected abstract string ExpectedAuthSecretSectionKey { get; }

    protected bool HasValidAuthSecret()
    {
        string? submittedSecret = GetSubmittedAuthSecret();

        if (string.IsNullOrWhiteSpace(submittedSecret))
        {
            logger.LogError("No auth secret submitted.");
            return false;
        }

        string expectedSecret = GetExpectedAuthSecret();

        if (!submittedSecret.Equals(expectedSecret, StringComparison.Ordinal))
        {
            logger.LogError("Submitted auth secret does not match expected secret.");
            return false;
        }

        return true;
    }

    private string? GetSubmittedAuthSecret()
    {
        string? cacheSecret =
            GetSecretFromHeader()
            ?? GetSecretFromQuery();

        return cacheSecret;
    }

    private string GetExpectedAuthSecret()
    {
        string? expectedSecret = null;

        try
        {
            expectedSecret = configuration.GetSection(ExpectedAuthSecretSectionName)[ExpectedAuthSecretSectionKey];
        }
        catch
        {
            logger.LogError("Error reading configuration.");
        }

        if (string.IsNullOrWhiteSpace(expectedSecret))
        {
            throw new
                InvalidOperationException("The expected authorisation secret is missing from configuration.");
        }

        return expectedSecret;
    }
    
    private string? GetSecretFromQuery()
    {
        string? secretFromQuery = null;

        bool ok = Request.Query.TryGetValue(AuthSecretKey.ToLowerInvariant(), out var submitted);
        if (ok && submitted.Count > 0)
        {
            secretFromQuery = submitted[0];
        }

        return secretFromQuery;
    }

    private string? GetSecretFromHeader()
    {
        string? secretFromHeader = null;

        var cacheSecrets = Request.Headers[AuthSecretKey];

        if (cacheSecrets.Count > 0)
        {
            secretFromHeader = cacheSecrets[0];
        }

        return secretFromHeader;
    }
}