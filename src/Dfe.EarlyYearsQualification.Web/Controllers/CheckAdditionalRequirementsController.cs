using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("qualifications/check-additional-questions")]
[RedirectIfDateMissing]
public class CheckAdditionalRequirementsController(
    ILogger<CheckAdditionalRequirementsController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet("{qualificationId}/{questionId}")]
    public async Task<IActionResult> Index(string qualificationId, int questionId)
    {
        if (ModelState.IsValid)
        {
            var model = new CheckAdditionalRequirementsPageModel();
            return await GetResponse(qualificationId, questionId, model);
        }
        
        logger.LogError("No qualificationId passed in");
        return RedirectToAction("Index", "Error");

    }

    [HttpPost("{qualificationId}/{questionId}")]
    public async Task<IActionResult> Post(string qualificationId, int questionId, [FromForm]CheckAdditionalRequirementsPageModel model)
    {
        if (ModelState.IsValid)
        {
            var previouslyAnsweredQuestions = userJourneyCookieService.GetAdditionalQuestionsAnswers() ?? new Dictionary<string, string>();
            var additionalRequirementQuestions = previouslyAnsweredQuestions.ToDictionary(previouslyAnsweredQuestion => previouslyAnsweredQuestion.Key, previouslyAnsweredQuestion => previouslyAnsweredQuestion.Value);
            additionalRequirementQuestions[model.Question] = model.Answer;
            userJourneyCookieService.SetAdditionalQuestionsAnswers(additionalRequirementQuestions);
            
            var qualification = await contentService.GetQualificationById(qualificationId);
            if (qualification is null)
            {
                var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
                logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                                loggedQualificationId);

                return RedirectToAction("Index", "Error");
            }

            if (qualification.AdditionalRequirementQuestions is not null && questionId < qualification.AdditionalRequirementQuestions.Count)
            {
                return RedirectToAction("Index", "CheckAdditionalRequirements",
                                        new { model.QualificationId, questionId = questionId + 1 });
            }
            
            return RedirectToAction("Index", "QualificationDetails",
                                    new { model.QualificationId });
        }
        
        model.HasErrors = true;
        return await GetResponse(qualificationId, questionId, model);
    }

    private async Task<IActionResult> GetResponse(string qualificationId, int questionId,
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

        var mappedModel = await MapModel(content, qualification, questionId, model);
        var answers = userJourneyCookieService.GetAdditionalQuestionsAnswers();
        if (answers is not null && answers.Count != 0 && questionId <= answers.Count)
        {
            var index = questionId - 1;
            mappedModel.Answer = answers.ElementAt(index).Value;
        }
        return View("Index", mappedModel);
    }

    private async Task<CheckAdditionalRequirementsPageModel> MapModel(CheckAdditionalRequirementsPage content, Qualification qualification,
                                                                      int questionId,
                                                                      CheckAdditionalRequirementsPageModel? model = null)
    {
        var mappedModel = model ?? new CheckAdditionalRequirementsPageModel();
        mappedModel.QualificationId = qualification.QualificationId;
        mappedModel.QuestionId = questionId;
        mappedModel.CtaButtonText = content.CtaButtonText;
        mappedModel.Heading = content.Heading;
        mappedModel.QuestionSectionHeading = content.QuestionSectionHeading;
        mappedModel.BackButton = CalculateBackButton(qualification.QualificationId, questionId, content);
        mappedModel.AdditionalRequirementQuestion =
            await MapAdditionalRequirementQuestion(qualification.AdditionalRequirementQuestions!, questionId);
        mappedModel.ErrorMessage = content.ErrorMessage;
        mappedModel.ErrorSummaryHeading = content.ErrorSummaryHeading;
        return mappedModel;
    }

    private static NavigationLinkModel? CalculateBackButton(string qualificationId, int questionId, CheckAdditionalRequirementsPage content)
    {
        if (questionId == 1)
        {
            return MapToNavigationLinkModel(content.BackButton);
        }
        
        var link = content.PreviousQuestionBackButton;

        if (link == null)
        {
            return MapToNavigationLinkModel(content.BackButton);
        }

        var previousQuestionId = questionId - 1;
        if (!link.Href.EndsWith($"/{qualificationId}/{previousQuestionId}", StringComparison.OrdinalIgnoreCase))
        {
            link.Href = $"{link.Href}/{qualificationId}/{previousQuestionId}";
        }

        return MapToNavigationLinkModel(link);
    }

    private async Task<AdditionalRequirementQuestionModel> MapAdditionalRequirementQuestion(List<AdditionalRequirementQuestion> additionalRequirementQuestions, int questionId)
    {
        var additionalRequirementQuestion = additionalRequirementQuestions[questionId - 1];
        return new AdditionalRequirementQuestionModel
               {
                   Question = additionalRequirementQuestion.Question,
                   HintText = additionalRequirementQuestion.HintText,
                   DetailsHeading = additionalRequirementQuestion.DetailsHeading,
                   DetailsContent = await contentParser.ToHtml(additionalRequirementQuestion.DetailsContent),
                   Options = MapOptions(additionalRequirementQuestion.Answers)
               };
    }

    private static List<OptionModel> MapOptions(List<Option> options)
    {
        return options.Select(option => new OptionModel { Label = option.Label, Value = option.Value }).ToList();
    }
}