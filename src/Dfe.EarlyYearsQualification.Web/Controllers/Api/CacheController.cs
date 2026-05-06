using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Contentful;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Api;

[IgnoreAntiforgeryToken]
[Route("api/clear-distributed-cache")]
public class CacheController(
    ILogger<CacheController> logger,
    ICacheInvalidator cacheInvalidator,
    IConfiguration configuration)
    : BaseApiController<CacheController>(logger, configuration)
{
    private readonly ILogger<CacheController> _logger = logger;

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        _logger.LogWarning("Call to endpoint to clear distributed cache");
        
        if (!HasValidAuthSecret())
        {
            return new UnauthorizedResult();
        }

        await cacheInvalidator.ClearCacheAsync(ContentfulUrlToPathAndQueryCacheKeyConverter.KeyPrefix);

        return new NoContentResult();
    }

    protected override string AuthSecretKey => "Cache-Secret";
    protected override string ExpectedAuthSecretSectionName => "Cache";
    protected override string ExpectedAuthSecretSectionKey => "AuthSecret";
}