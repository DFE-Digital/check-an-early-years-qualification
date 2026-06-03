using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Api;

[IgnoreAntiforgeryToken]
[Route("api/generate-download")]
public class GenerateDownloadController(
    ILogger<GenerateDownloadController> logger,
    IQualificationDownloadService qualificationDownloadService,
    IConfiguration configuration)
    : BaseApiController<GenerateDownloadController>(logger, configuration)
{
    private readonly ILogger<GenerateDownloadController> _logger = logger;

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        _logger.LogWarning("Call to endpoint to generate new download file");
        
        if (!HasValidAuthSecret())
        {
            return new UnauthorizedResult();
        }

        await qualificationDownloadService.GenerateEyqlDownload();

        return new NoContentResult();
    }

    protected override string AuthSecretKey => "Download-Secret";
    protected override string ExpectedAuthSecretSectionName => "Download";
    protected override string ExpectedAuthSecretSectionKey => "AuthSecret";
}