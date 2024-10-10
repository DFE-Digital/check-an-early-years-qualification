using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
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
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet("{qualificationId}/{questionIndex}")]
    public async Task<IActionResult> Index(string qualificationId, int questionIndex)
    {
        if (ModelState.IsValid)
        {
            var model = new CheckAdditionalRequirementsPageModel();
            return await GetResponse(qualificationId, questionIndex, model);
        }

        logger.LogError("No qualificationId passed in");
        return RedirectToAction("Index", "Error");
    }

    [HttpPost("{qualificationId}/{questionIndex}")]
    public async Task<IActionResult> Post(string qualificationId, int questionIndex, [FromForm]CheckAdditionalRequirementsPageModel model)
    {
        if (ModelState.IsValid)
        {
            var previouslyAnsweredQuestions = userJourneyCookieService.GetAdditionalQuestionsAnswers() ?? new Dictionary<string, string>();
            var additionalRequirementQuestions = previouslyAnsweredQuestions.ToDictionary(previouslyAnsweredQuestion => previouslyAnsweredQuestion.Key, previouslyAnsweredQuestion => previouslyAnsweredQuestion.Value);
            additionalRequirementQuestions[model.Question] = model.Answer;
            userJourneyCookieService.SetAdditionalQuestionsAnswers(additionalRequirementQuestions);
            
            var qualification = await qualificationsRepository.GetById(qualificationId);
            if (qualification is null)
            {
                var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
                logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                                loggedQualificationId);

                return RedirectToAction("Index", "Error");
            }

            if (qualification.AdditionalRequirementQuestions is not null && questionIndex < qualification.AdditionalRequirementQuestions.Count)
            {
                return RedirectToAction("Index", "CheckAdditionalRequirements",
                                        new { model.QualificationId, questionIndex = questionIndex + 1 });
            }
            
            return RedirectToAction("ConfirmAnswers", "CheckAdditionalRequirements",
                                    new { model.QualificationId });
        }

        model.HasErrors = true;
        return await GetResponse(qualificationId, questionIndex, model);
    }

    [HttpGet]
    [Route("{qualificationId}/confirm-answers")]
    public async Task<IActionResult> ConfirmAnswers(string qualificationId)
    {
        var content = await contentService.GetCheckAdditionalRequirementsAnswerPage();
        if (content is null)
        {
            logger.LogError("No content for the check additional requirements answer page content");
            return RedirectToAction("Index", "Error");
        }

        var answers = userJourneyCookieService.GetAdditionalQuestionsAnswers();
        
        var qualification = await qualificationsRepository.GetById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        if (qualification.AdditionalRequirementQuestions == null)
        {
            //TODO: go straight to result?
        }
        
        if (answers == null)
        {
            //TODO: redirect to first question?
        }

        if (answers!.Count != qualification.AdditionalRequirementQuestions!.Count)
        {
            //TODO: redirect to first question?
        }

        var model = MapCheckAnswers(content, answers, qualificationId);

        return View(model);
    }

    private async Task<IActionResult> GetResponse(string qualificationId, int questionIndex,
                                                  CheckAdditionalRequirementsPageModel? model = null)
    {
        var qualification = await qualificationsRepository.GetById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        if (qualification.AdditionalRequirementQuestions is null ||
            qualification.AdditionalRequirementQuestions.Count == 0)
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

        var mappedModel = await MapModel(content, qualification, questionIndex, model);
        var answers = userJourneyCookieService.GetAdditionalQuestionsAnswers();
        if (answers is not null && answers.Count != 0 && questionIndex <= answers.Count)
        {
            var index = questionIndex - 1;
            mappedModel.Answer = answers.ElementAt(index).Value;
        }
        return View("Index", mappedModel);
    }

    private async Task<CheckAdditionalRequirementsPageModel> MapModel(CheckAdditionalRequirementsPage content,
                                                                      Qualification qualification,
                                                                      int questionIndex,
                                                                      CheckAdditionalRequirementsPageModel? model = null)
    {
        var mappedModel = model ?? new CheckAdditionalRequirementsPageModel();
        mappedModel.QualificationId = qualification.QualificationId;
        mappedModel.QuestionIndex = questionIndex;
        mappedModel.CtaButtonText = content.CtaButtonText;
        mappedModel.Heading = content.Heading;
        mappedModel.QuestionSectionHeading = content.QuestionSectionHeading;
        mappedModel.BackButton = CalculateBackButton(qualification.QualificationId, questionIndex, content);
        mappedModel.AdditionalRequirementQuestion =
            await MapAdditionalRequirementQuestion(qualification.AdditionalRequirementQuestions!, questionIndex);
        mappedModel.ErrorMessage = content.ErrorMessage;
        mappedModel.ErrorSummaryHeading = content.ErrorSummaryHeading;
        return mappedModel;
    }

    private static NavigationLinkModel? CalculateBackButton(string qualificationId, int questionIndex, CheckAdditionalRequirementsPage content)
    {
        if (questionIndex == 1)
        {
            return MapToNavigationLinkModel(content.BackButton);
        }
        
        var link = content.PreviousQuestionBackButton;

        if (link == null)
        {
            return MapToNavigationLinkModel(content.BackButton);
        }

        var previousQuestionIndex = questionIndex - 1;
        if (!link.Href.EndsWith($"/{qualificationId}/{previousQuestionIndex}", StringComparison.OrdinalIgnoreCase))
        {
            link.Href = $"{link.Href}/{qualificationId}/{previousQuestionIndex}";
        }

        return MapToNavigationLinkModel(link);
    }

    private async Task<AdditionalRequirementQuestionModel> MapAdditionalRequirementQuestion(List<AdditionalRequirementQuestion> additionalRequirementQuestions, int questionIndex)
    {
        var additionalRequirementQuestion = additionalRequirementQuestions[questionIndex - 1];
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

    private CheckAdditionalRequirementsAnswerPageModel MapCheckAnswers(CheckAdditionalRequirementsAnswerPage pageModel, Dictionary<string, string> answers, string qualificationId)
    {
        var backButtonIndex = answers.Count;

        pageModel.BackButton!.Href = pageModel.BackButton.Href + $"/{qualificationId}/{backButtonIndex}";

        var answersToDisplay = answers.ToDictionary(answer => answer.Key, answer => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(answer.Value));

        return new CheckAdditionalRequirementsAnswerPageModel
               {
                   Answers = answersToDisplay,
                   BackButton = MapToNavigationLinkModel(pageModel.BackButton),
                   ButtonText = pageModel.ButtonText,
                   PageHeading = pageModel.PageHeading,
                   AnswerDisclaimerText = pageModel.AnswerDisclaimerText,
                   ChangeAnswerText = pageModel.ChangeAnswerText,
                   GetResultsHref = $"/qualifications/qualification-details/{qualificationId}",
                   ChangeQuestionHref = $"/qualifications/check-additional-questions/{qualificationId}/"
               };
    }
}