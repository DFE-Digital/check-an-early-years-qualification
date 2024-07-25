using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("qualifications/check-additional-questions")]
public class CheckAdditionalRequirementsController(
    ILogger<CheckAdditionalRequirementsController> logger,
    IContentService contentService,
    IHtmlRenderer htmlRenderer,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet("{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (ModelState.IsValid) return await GetResponse(qualificationId);
        
        logger.LogError("No qualificationId passed in");
        return RedirectToAction("Index", "Error");

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]CheckAdditionalRequirementsPageModel model)
    {
        if (ModelState.IsValid)
        {
            userJourneyCookieService.SetAdditionalQuestionsAnswers(model.Answers);
            return RedirectToAction("Index", "QualificationDetails",
                                    new { model.QualificationId });
        }
        
        model.HasErrors = true;
        return await GetResponse(model.QualificationId, model);
    }

    private async Task<IActionResult> GetResponse(string qualificationId,
                                                  CheckAdditionalRequirementsPageModel? model = null)
    {
        var qualification = await contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        if (qualification.AdditionalRequirementQuestions is null || qualification.AdditionalRequirementQuestions.Count == 0)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogInformation("QualificationId has no additional requirement questions: {QualificationId}",
                                  loggedQualificationId);
            return RedirectToAction("Index", "QualificationDetails",
                                    new { qualificationId });
        }
        
        var content = await contentService.GetCheckAdditionalRequirementsPage();
        if (content is null)
        {
            logger.LogError("No content for the check additional requirements page");
            return RedirectToAction("Index", "Error");
        }

        var mappedModel = await MapModel(content, qualification, model);
        if (mappedModel.HasErrors)
        {
            SetQuestionErrorFlag(mappedModel);
        }

        return View("Index", mappedModel);
    }

    private async Task<CheckAdditionalRequirementsPageModel> MapModel(CheckAdditionalRequirementsPage content, Qualification qualification,
                                                                      CheckAdditionalRequirementsPageModel? model = null)
    {
        var mappedModel = model ?? new CheckAdditionalRequirementsPageModel();
        mappedModel.QualificationId = qualification.QualificationId;
        mappedModel.AwardingOrganisation = qualification.AwardingOrganisationTitle;
        mappedModel.AwardingOrganisationLabel = content.AwardingOrganisationLabel;
        mappedModel.CtaButtonText = content.CtaButtonText;
        mappedModel.QualificationLevel = qualification.QualificationLevel;
        mappedModel.QualificationLabel = content.QualificationLabel;
        mappedModel.QualificationName = qualification.QualificationName;
        mappedModel.Heading = content.Heading;
        mappedModel.InformationMessage = content.InformationMessage;
        mappedModel.QualificationLevelLabel = content.QualificationLevelLabel;
        mappedModel.QuestionSectionHeading = content.QuestionSectionHeading;
        mappedModel.BackButton = content.BackButton;
        mappedModel.AdditionalRequirementQuestions =
            await MapAdditionalRequirementQuestions(qualification.AdditionalRequirementQuestions!);
        mappedModel.Answers = MapQuestionsToDictionary(qualification.AdditionalRequirementQuestions!, model);
        mappedModel.ErrorMessage = content.ErrorMessage;
        mappedModel.ErrorSummaryHeading = content.ErrorSummaryHeading;
        mappedModel.QuestionCount = mappedModel.AdditionalRequirementQuestions.Count;
        return mappedModel;
    }
    
    private async Task<List<AdditionalRequirementQuestionModel>> MapAdditionalRequirementQuestions(List<AdditionalRequirementQuestion> additionalRequirementQuestions)
    {
        var results = new List<AdditionalRequirementQuestionModel>();

        foreach (var additionalRequirementQuestion in additionalRequirementQuestions)
        {
            results.Add(new AdditionalRequirementQuestionModel
                        {
                            Question = additionalRequirementQuestion.Question,
                            HintText = additionalRequirementQuestion.HintText,
                            DetailsHeading = additionalRequirementQuestion.DetailsHeading,
                            DetailsContent = await htmlRenderer.ToHtml(additionalRequirementQuestion.DetailsContent),
                            Options = MapOptions(additionalRequirementQuestion.Answers)
                        });
        }
        
        return results;
    }

    private static List<OptionModel> MapOptions(List<Option> options)
    {
        return options.Select(option => new OptionModel { Label = option.Label, Value = option.Value }).ToList();
    }
    
    private static Dictionary<string, string> MapQuestionsToDictionary(List<AdditionalRequirementQuestion> additionalRequirementQuestions, CheckAdditionalRequirementsPageModel? previousModel)
    {
        var result = additionalRequirementQuestions.ToDictionary(additionalRequirementQuestion => additionalRequirementQuestion.Question, _ => string.Empty);
        if (previousModel is null) return result;
        
        foreach (var answer in previousModel.Answers.Where(answer => !string.IsNullOrEmpty(answer.Value)))
        {
            result[answer.Key] = answer.Value;
        }

        return result;
    }
    
    private static void SetQuestionErrorFlag(CheckAdditionalRequirementsPageModel model)
    {
        foreach (var answer in model.Answers.Where(answer => string.IsNullOrEmpty(answer.Value)))
        {
            model.AdditionalRequirementQuestions.First(x => x.Question == answer.Key).HasError = true;
        }
    }
}