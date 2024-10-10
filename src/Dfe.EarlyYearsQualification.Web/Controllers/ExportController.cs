using System.Globalization;
using Azure.Identity;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Helpers.QualificationsToCsv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Dfe.EarlyYearsQualification.Web.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class ExportController(
    ILogger<HomeController> logger,
    IQualificationsToCsvHelper qualificationsToCsvHelper) : ControllerBase
{
    [IgnoreAntiforgeryToken]
    [HttpPost("generate-csv")]
    public async Task<IActionResult> GenerateQualificationCsv()
    {
        logger.LogInformation("Qualification CSV generation requested.");
        
        await qualificationsToCsvHelper.GetQualificationsAndGenerate();

        return Ok();
    }
}