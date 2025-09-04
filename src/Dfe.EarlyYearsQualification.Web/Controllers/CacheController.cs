using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Contentful;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[IgnoreAntiforgeryToken]
[Route("api/clear-distributed-cache")]
public class CacheController(
    ILogger<CacheController> logger,
    ICacheInvalidator cacheInvalidator,
    IConfiguration configuration)
    : Controller // Do not inherit from Dfe.EarlyYearsQualification.Web.Controllers.Base.ServiceController
{
    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        logger.LogWarning("Call to endpoint to clear distributed cache");

        string? submittedSecret = GetSubmittedCacheAuthSecret();

        if (string.IsNullOrWhiteSpace(submittedSecret))
        {
            logger.LogError("No cache auth secret submitted.");
            return new UnauthorizedResult();
        }

        string expectedSecret = GetExpectedCacheAuthSecret();

        if (!submittedSecret.Equals(expectedSecret, StringComparison.Ordinal))
        {
            logger.LogError("Submitted cache auth secret does not match expected secret.");
            return new UnauthorizedResult();
        }

        await cacheInvalidator.ClearCacheAsync(ContentfulUrlToPathAndQueryCacheKeyConverter.KeyPrefix);

        return new NoContentResult();
    }

    private string? GetSubmittedCacheAuthSecret()
    {
        string? cacheSecret =
            GetCacheSecretFromHeader()
            ?? GetCacheSecretFromQuery();

        return cacheSecret;
    }

    private string? GetCacheSecretFromQuery()
    {
        string? cacheSecret = null;

        bool ok = Request.Query.TryGetValue("cache-secret", out var submitted);
        if (ok && submitted.Count > 0)
        {
            cacheSecret = submitted[0];
        }

        return cacheSecret;
    }

    private string? GetCacheSecretFromHeader()
    {
        string? cacheSecret = null;

        var cacheSecrets = Request.Headers["Cache-Secret"];

        if (cacheSecrets.Count > 0)
        {
            cacheSecret = cacheSecrets[0];
        }

        return cacheSecret;
    }

    private string GetExpectedCacheAuthSecret()
    {
        string? expectedSecret = null;

        try
        {
            expectedSecret = configuration.GetSection("Cache")["AuthSecret"];
        }
        catch
        {
            logger.LogError("Error reading configuration.");
        }

        if (string.IsNullOrWhiteSpace(expectedSecret))
        {
            throw new
                InvalidOperationException("The expected cache authorisation secret is missing from configuration.");
        }

        return expectedSecret;
    }
}