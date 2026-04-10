using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

[Route("/help")]
public partial class HelpController(
    ILogger<HelpController> logger,
    Services.Help.IHelpService helpService)
    : ServiceController
{
    [HttpGet("I-need-a-copy-of-the-qualification-certificate-or-transcript")]
    public async Task<IActionResult> INeedACopyOfTheQualificationCertificateOrTranscript()
    {
        return await GetStaticPage(StaticPages.HowToGetACopyOfTheCertificateOrTranscript);
    }

    [HttpGet("I-do-not-know-what-level-the-qualification-is")]
    public async Task<IActionResult> IDoNotKnowWhatLevelTheQualificationIs()
    {
        return await GetStaticPage(StaticPages.HowToFindTheLevelOfAQualification);
    }

    [HttpGet("I-want-to-check-whether-a-course-is-approved-before-I-enrol")]
    public async Task<IActionResult> IWantToCheckWhetherACourseIsApprovedBeforeIEnrol()
    {
        return await GetStaticPage(StaticPages.HowToFindASuitableCourse);
    }

    private async Task<IActionResult> GetStaticPage(string staticPageId)
    {
        var staticPage = await helpService.GetStaticPage(staticPageId);
        if (staticPage is null)
        {
            logger.LogError("No content for the static page");
            return RedirectToAction("Index", "Error");
        }

        var model = await helpService.MapStaticPage(staticPage);

        return View("../Static/Static", model);
    }
}
