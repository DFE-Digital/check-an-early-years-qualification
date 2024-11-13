using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
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

            if (qualification.AdditionalRequirementQuestions is null)
                return RedirectToAction("ConfirmAnswers", "CheckAdditionalRequirements",
                                        new { model.QualificationId });
            
            // If the user answer matches the answer to be full and relevant to the Qts question, then go straight to the qualification details page
            var modelAnswerAsBool = model.Answer == "yes";
            var question = qualification.AdditionalRequirementQuestions[questionIndex - 1];
            if (question.Sys.Id == AdditionalRequirementQuestions.QtsQuestion && question.AnswerToBeFullAndRelevant == modelAnswerAsBool)
            {
                var overrideAdditionalRequirementQuestion = new Dictionary<string, string>
                                                            {
                                                                [question.Question] = model.Answer
                                                            };
                userJourneyCookieService.SetAdditionalQuestionsAnswers(overrideAdditionalRequirementQuestion);
                return RedirectToAction("ConfirmAnswers", "CheckAdditionalRequirements",
                                        new { model.QualificationId });
            }

            if (questionIndex < qualification.AdditionalRequirementQuestions.Count)
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
            return RedirectToAction("Index", "QualificationDetails",
                                    new { qualificationId });
        }
        
        var answers = userJourneyCookieService.GetAdditionalQuestionsAnswers();

        if (answers == null || !UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, answers))
        {
            if (answers!.Count != qualification.AdditionalRequirementQuestions!.Count)
            {
                return RedirectToAction("Index", "CheckAdditionalRequirements", new { qualificationId, questionIndex = 1 });
            }
        }

        var model = MapCheckAnswers(content, answers, qualificationId);

        return View("ConfirmAnswers", model);
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
        var backButton = CalculateBackButton(qualification.QualificationId, questionIndex, content);
        var additionalRequirementQuestion =
            await MapAdditionalRequirementQuestion(qualification.AdditionalRequirementQuestions!, questionIndex);
        return CheckAdditionalRequirementsPageMapper.Map(content, qualification.QualificationId, questionIndex, backButton,
                                                         additionalRequirementQuestion, model);
    }

    private static NavigationLinkModel? CalculateBackButton(string qualificationId, int questionIndex, CheckAdditionalRequirementsPage content)
    {
        if (questionIndex == 1)
        {
            return NavigationLinkMapper.Map(content.BackButton);
        }
        
        var link = content.PreviousQuestionBackButton;

        if (link == null)
        {
            return NavigationLinkMapper.Map(content.BackButton);
        }

        var previousQuestionIndex = questionIndex - 1;
        if (!link.Href.EndsWith($"/{qualificationId}/{previousQuestionIndex}", StringComparison.OrdinalIgnoreCase))
        {
            link.Href = $"{link.Href}/{qualificationId}/{previousQuestionIndex}";
        }

        return NavigationLinkMapper.Map(link);
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

    private static CheckAdditionalRequirementsAnswerPageModel MapCheckAnswers(CheckAdditionalRequirementsAnswerPage pageModel, Dictionary<string, string> answers, string qualificationId)
    {
        var backButtonIndex = answers.Count;

        pageModel.BackButton!.Href = pageModel.BackButton.Href + $"/{qualificationId}/{backButtonIndex}";

        var answersToDisplay = answers.ToDictionary(answer => answer.Key, answer => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(answer.Value));

        return new CheckAdditionalRequirementsAnswerPageModel
               {
                   Answers = answersToDisplay,
                   BackButton = NavigationLinkMapper.Map(pageModel.BackButton),
                   ButtonText = pageModel.ButtonText,
                   PageHeading = pageModel.PageHeading,
                   AnswerDisclaimerText = pageModel.AnswerDisclaimerText,
                   ChangeAnswerText = pageModel.ChangeAnswerText,
                   GetResultsHref = $"/qualifications/qualification-details/{qualificationId}",
                   ChangeQuestionHref = $"/qualifications/check-additional-questions/{qualificationId}/"
               };
    }
    
    private static bool UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(Qualification qualification,
                                                                              Dictionary<string, string> additionalRequirementAnswers)
    {
        if (qualification.AdditionalRequirementQuestions is null || qualification.AdditionalRequirementQuestions.All(x => x.Sys.Id != AdditionalRequirementQuestions.QtsQuestion))
        {
            return false;
        }
        
        var qtsQuestion =
            qualification.AdditionalRequirementQuestions.First(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                         .QtsQuestion);

        var userAnsweredQuestion = additionalRequirementAnswers.First(x => x.Key == qtsQuestion.Question);
        var answerAsBool = userAnsweredQuestion.Value == "yes";
        return qtsQuestion.AnswerToBeFullAndRelevant == answerAsBool;
    }
}