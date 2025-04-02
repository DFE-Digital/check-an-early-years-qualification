using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class CacheController(
    ILogger<CacheController> logger,
    ICacheInvalidator cacheInvalidator,
    IConfiguration configuration)
    : Controller // Do not inherit from Dfe.EarlyYearsQualification.Web.Controllers.Base.ServiceController
{
    [HttpGet("api/clear-distributed-cache")]
    public async Task<IActionResult> Index()
    {
        logger.LogWarning("Call to endpoint to clear distributed cache");

        var submittedSecret = GetSubmittedCacheAuthSecret();

        if (string.IsNullOrWhiteSpace(submittedSecret))
        {
            logger.LogError("No cache auth secret submitted.");
            return new UnauthorizedResult();
        }

        var expectedSecret = GetExpectedCacheAuthSecret();

        if (!submittedSecret.Equals(expectedSecret, StringComparison.Ordinal))
        {
            logger.LogError("Submitted cache auth secret does not match expected secret.");
            return new UnauthorizedResult();
        }

        await cacheInvalidator.ClearCacheAsync();

        return new NoContentResult();
    }

    private string? GetSubmittedCacheAuthSecret()
    {
        var cacheSecrets = Request.Headers["Cache-Secret"];

        string? cacheSecret = null;

        if (cacheSecrets.Count > 0)
        {
            cacheSecret = cacheSecrets[0];
        }

        return cacheSecret;
    }

    private string GetExpectedCacheAuthSecret()
    {
        var expectedSecret = configuration.GetSection("Cache")["AuthSecret"];

        if (string.IsNullOrWhiteSpace(expectedSecret))
        {
            throw new
                InvalidOperationException("The expected cache authorisation secret is missing from configuration.");
        }

        return expectedSecret!;
    }
}