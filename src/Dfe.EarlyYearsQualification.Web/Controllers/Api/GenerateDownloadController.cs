using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Api;

[IgnoreAntiforgeryToken]
[Route("api/generate-download")]
public class GenerateDownloadController : BaseApiController<GenerateDownloadController>
{
    private readonly ILogger<GenerateDownloadController> _logger;
    private readonly IQualificationDownloadService _qualificationDownloadService;
    private readonly IEnvironmentService _environmentService;

    public GenerateDownloadController(
        ILogger<GenerateDownloadController> logger,
        IQualificationDownloadService qualificationDownloadService,
        IConfiguration configuration,
        IEnvironmentService environmentService)
        : base(logger, configuration)
    {
        _logger = logger;
        _qualificationDownloadService = qualificationDownloadService;
        _environmentService = environmentService;
    }

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        _logger.LogWarning("Call to endpoint to generate new download file");

        if (!HasValidAuthSecret())
        {
            return new UnauthorizedResult();
        }

        var environment = _environmentService.GetEnvironment();

        await _qualificationDownloadService.GenerateEyqlDownloadByEnvironment(environment);

        return new NoContentResult();
    }

    protected override string AuthSecretKey => "Download-Secret";
    protected override string ExpectedAuthSecretSectionName => "Download";
    protected override string ExpectedAuthSecretSectionKey => "AuthSecret";
}