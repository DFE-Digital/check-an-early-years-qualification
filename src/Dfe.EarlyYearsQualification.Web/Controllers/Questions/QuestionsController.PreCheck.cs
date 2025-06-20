using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("pre-check")]
    public async Task<IActionResult> PreCheck()
    {
        return await GetPreCheckView();
    }
    
    [HttpPost("pre-check")]
    public async Task<IActionResult> PreCheck([FromForm] PreCheckPageModel model)
    {
        if (!ModelState.IsValid)
        {
            var preCheckPage = await contentService.GetPreCheckPage();

            if (preCheckPage is not null)
            {
                model = await MapPreCheckModel(preCheckPage);
                model.HasErrors = true;
            }

            return View("PreCheck", model);
        }

        return model.Option == Options.Yes ? RedirectToAction(nameof(StartNew)) : RedirectToAction("Index", "Home");
    }
    
    private async Task<IActionResult> GetPreCheckView()
    {
        var preCheckPage = await contentService.GetPreCheckPage();
        if (preCheckPage is null)
        {
            logger.LogError("No content for the pre-check page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapPreCheckModel(preCheckPage);

        return View("PreCheck", model);
    }

    private async Task<PreCheckPageModel> MapPreCheckModel(PreCheckPage preCheckPage)
    {
        return PreCheckPageMapper.Map(preCheckPage, await contentParser.ToHtml(preCheckPage.PostHeaderContent));
    }
}