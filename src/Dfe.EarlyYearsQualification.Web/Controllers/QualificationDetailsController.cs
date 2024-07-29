using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IContentService contentService,
    IContentFilterService contentFilterService,
    IGovUkInsetTextRenderer govUkInsetTextRenderer,
    IHtmlRenderer htmlRenderer,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var listPageContent = await contentService.GetQualificationListPage();
        if (listPageContent is null)
        {
            logger.LogError("No content for the qualification list page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapList(listPageContent, await GetFilteredQualifications());

        return View(model);
    }

    [HttpPost]
    public IActionResult Refine(string? refineSearch)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning($"Invalid model state in {nameof(QualificationDetailsController)} POST");
        }

        userJourneyCookieService.SetQualificationNameSearchCriteria(refineSearch ?? string.Empty);

        return RedirectToAction("Get");
    }

    [HttpGet("qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(qualificationId))
        {
            return BadRequest();
        }

        var detailsPageContent = await contentService.GetDetailsPage();
        if (detailsPageContent is null)
        {
            logger.LogError("No content for the qualification details page");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        // Grab the level and start date of the qualification
        var levelSelected = userJourneyCookieService.GetLevelOfQualification();
        var (_, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();

        // Check that the user has chosen a level and a start date, if not then redirect them back to the start of the journey
        if (levelSelected == null || startDateYear == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var model = await MapDetails(qualification, detailsPageContent);

        // Get additional requirements questions if any and check answers from previous page

        // If the qualification has no additional requirements then skip this check
        if (qualification.AdditionalRequirementQuestions != null &&
            qualification.AdditionalRequirementQuestions.Count != 0)
        {
            var additionalRequirementsAnswers = userJourneyCookieService.GetAdditionalQuestionsAnswers();

            // If there is a mismatch between the questions answered, then clear the answers and navigate back to the additional requirements check page
            if (additionalRequirementsAnswers == null ||
                (qualification.AdditionalRequirementQuestions.Count != additionalRequirementsAnswers.Count))
            {
                return RedirectToAction("Index", "CheckAdditionalRequirements", new { qualificationId });
            }

            if ((from question in qualification.AdditionalRequirementQuestions
                 from answer in additionalRequirementsAnswers
                 where (question.AnswerToBeFullAndRelevant && answer.Value == "no") ||
                       (!question.AnswerToBeFullAndRelevant && answer.Value == "yes")
                 select question).Any())
            {
                model.RatioRequirements = MarkAsNotFullAndRelevant(model.RatioRequirements);
                return View(model);
            }
        }

        // If all the additional requirement checks pass, then we can go to check each level individually
        CheckRatioRequirements(levelSelected.Value, startDateYear.Value, qualification, model);

        return View(model);
    }

    private void CheckRatioRequirements(int levelSelected, int startDateYear, Qualification qualification,
                                        QualificationDetailsModel model)
    {
        // Build up property name to check for each level
        var propertyToCheck =
            $"FullAndRelevantForLevel{levelSelected}{(startDateYear > 2014 ? "After" : "Before")}2014";

        model.RatioRequirements.ApprovedForLevel2 =
            CheckRatio(propertyToCheck, "Level 2 Ratio Requirements", qualification);
        
        model.RatioRequirements.ApprovedForLevel3 =
            CheckRatio(propertyToCheck, "Level 3 Ratio Requirements", qualification);
        
        model.RatioRequirements.ApprovedForLevel6 =
            CheckRatio(propertyToCheck, "Level 6 Ratio Requirements", qualification);
        
        model.RatioRequirements.ApprovedForUnqualified = true;
    }

    private bool CheckRatio(string propertyToCheck, string ratioName, Qualification qualification)
    {
        try
        {
            var requirement =
                qualification.RatioRequirements!.FirstOrDefault(x => x.RatioRequirementName == ratioName);

            return (bool)requirement!.GetType().GetProperty(propertyToCheck)!.GetValue(requirement, null)!;
        }
        catch
        {
            logger.LogError($"Could not find property: {propertyToCheck} within {ratioName} for qualification: {qualification.QualificationId}");
            throw;
        }
    }


    private RatioRequirementModel MarkAsNotFullAndRelevant(RatioRequirementModel model)
    {
        model.ApprovedForLevel2 = false;
        model.ApprovedForLevel3 = false;
        model.ApprovedForLevel6 = false;
        model.ApprovedForUnqualified = true;

        return model;
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await contentFilterService.GetFilteredQualifications(level, startDateMonth, startDateYear,
                                                                    awardingOrganisation, searchCriteria);
    }

    private async Task<QualificationListModel> MapList(QualificationListPage content,
                                                       List<Qualification>? qualifications)
    {
        var basicQualificationsModels = GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel(content);

        return new QualificationListModel
               {
                   BackButton = content.BackButton,
                   Filters = filterModel,
                   Header = content.Header,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await htmlRenderer.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   LevelHeading = content.LevelHeading,
                   AwardingOrganisationHeading = content.AwardingOrganisationHeading,
                   PostSearchCriteriaContent = await htmlRenderer.ToHtml(content.PostSearchCriteriaContent),
                   PostQualificationListContent = await htmlRenderer.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels.OrderBy(x => x.QualificationName).ToList()
               };
    }

    private FilterModel GetFilterModel(QualificationListPage content)
    {
        var filterModel = new FilterModel
                          {
                              Country = userJourneyCookieService.GetWhereWasQualificationAwarded()!,
                              Level = content.AnyLevelHeading,
                              AwardingOrganisation = content.AnyAwardingOrganisationHeading
                          };

        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
            filterModel.StartDate = $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        var level = userJourneyCookieService.GetLevelOfQualification();
        if (level > 0)
        {
            filterModel.Level = $"Level {level}";
        }

        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            filterModel.AwardingOrganisation = awardingOrganisation;
        }

        return filterModel;
    }

    private static List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification>? qualifications)
    {
        var basicQualificationsModels = new List<BasicQualificationModel>();

        // ReSharper disable once InvertIf
        if (qualifications is not null && qualifications.Count > 0)
        {
            foreach (var qualification in qualifications)
            {
                basicQualificationsModels.Add(new BasicQualificationModel
                                              {
                                                  QualificationId = qualification.QualificationId,
                                                  QualificationLevel = qualification.QualificationLevel,
                                                  QualificationName = qualification.QualificationName,
                                                  AwardingOrganisationTitle = qualification.AwardingOrganisationTitle
                                              });
            }
        }

        return basicQualificationsModels;
    }

    private async Task<QualificationDetailsModel> MapDetails(Qualification qualification, DetailsPage content)
    {
        return new QualificationDetailsModel
               {
                   QualificationId = qualification.QualificationId,
                   QualificationLevel = qualification.QualificationLevel,
                   QualificationName = qualification.QualificationName,
                   QualificationNumber = qualification.QualificationNumber,
                   AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
                   FromWhichYear = qualification.FromWhichYear,
                   BackButton = content.BackButton,
                   AdditionalRequirementQuestions =
                       await MapAdditionalRequirementQuestions(qualification.AdditionalRequirementQuestions),
                   Content = new DetailsPageModel
                             {
                                 AwardingOrgLabel = content.AwardingOrgLabel,
                                 BookmarkHeading = content.BookmarkHeading,
                                 BookmarkText = content.BookmarkText,
                                 CheckAnotherQualificationHeading = content.CheckAnotherQualificationHeading,
                                 CheckAnotherQualificationText =
                                     await govUkInsetTextRenderer.ToHtml(content.CheckAnotherQualificationText),
                                 DateAddedLabel = content.DateAddedLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 FurtherInfoHeading = content.FurtherInfoHeading,
                                 FurtherInfoText = await govUkInsetTextRenderer.ToHtml(content.FurtherInfoText),
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 QualificationNumberLabel = content.QualificationNumberLabel,
                                 RequirementsHeading = content.RequirementsHeading,
                                 RequirementsText = await htmlRenderer.ToHtml(content.RequirementsText),
                                 RatiosHeading = content.RatiosHeading,
                                 RatiosText = await htmlRenderer.ToHtml(content.RatiosText),
                                 CheckAnotherQualificationLink = content.CheckAnotherQualificationLink
                             }
               };
    }

    private async Task<List<AdditionalRequirementQuestionModel>?> MapAdditionalRequirementQuestions(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions)
    {
        if (additionalRequirementQuestions is null) return null;

        var results = new List<AdditionalRequirementQuestionModel>();

        foreach (var additionalRequirementQuestion in additionalRequirementQuestions)
        {
            results.Add(new AdditionalRequirementQuestionModel
                        {
                            Question = additionalRequirementQuestion.Question,
                            HintText = additionalRequirementQuestion.HintText,
                            DetailsHeading = additionalRequirementQuestion.DetailsHeading,
                            DetailsContent = await htmlRenderer.ToHtml(additionalRequirementQuestion.DetailsContent),
                        });
        }

        return results;
    }
}