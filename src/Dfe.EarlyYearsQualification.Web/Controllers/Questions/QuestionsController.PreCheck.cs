using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("pre-check")]
    public async Task<IActionResult> PreCheck()
    {
        return await questionService.GetPreCheckView();
    }
    
    [HttpPost("pre-check")]
    public async Task<IActionResult> PreCheck([FromForm] PreCheckPageModel model)
    {
        if (!ModelState.IsValid)
        {
            var preCheckPage = await questionService.GetPreCheckPage();

            if (preCheckPage is not null)
            {
                model = await questionService.MapPreCheckModel(preCheckPage);
                model.HasErrors = true;
            }

            return View("PreCheck", model);
        }

        return model.Option == Options.Yes ? RedirectToAction(nameof(StartNew)) : RedirectToAction("Index", "Home");
    }
}