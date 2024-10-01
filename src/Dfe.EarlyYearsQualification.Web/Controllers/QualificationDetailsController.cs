using System.Globalization;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
[RedirectIfDateMissing]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IGovUkContentParser contentParser,
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

        var qualification = await qualificationsRepository.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        // Grab the start date of the qualification
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

        // Check that the user has chosen a start date, if not then redirect them back to the start of the journey
        if (startDateYear == null || startDateMonth == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var qualificationStartedBeforeSeptember2014 = userJourneyCookieService.WasStartedBeforeSeptember2014();

        var model = await MapDetails(qualificationStartedBeforeSeptember2014, qualification, detailsPageContent);

        var validateAdditionalRequirementQuestions = ValidateAdditionalQuestions(model);

        if (!validateAdditionalRequirementQuestions.isValid)
        {
            return validateAdditionalRequirementQuestions.actionResult!;
        }

        // If all the additional requirement checks pass, then we can go to check each level individually
        await CheckRatioRequirements(qualificationStartedBeforeSeptember2014, qualification, model);

        return View(model);
    }

    private (bool isValid, IActionResult? actionResult) ValidateAdditionalQuestions(QualificationDetailsModel model)
    {
        // If the qualification has no additional requirements then skip all checks and return.
        if (model.AdditionalRequirementAnswers == null) return (true, null);

        // If there is a mismatch between the questions answered, then clear the answers and navigate back to the additional requirements check page
        if (model.AdditionalRequirementAnswers.Count == 0 ||
            model.AdditionalRequirementAnswers.Exists(answer => string.IsNullOrEmpty(answer.Answer)))
        {
            return (false,
                    RedirectToAction("Index", "CheckAdditionalRequirements",
                                     new { model.QualificationId }));
        }

        // If there are not any answers to the questions that are not full and relevant we can continue back to check the ratios.
        if (!AnswersIndicateNotFullAndRelevant(model.AdditionalRequirementAnswers)) return (true, null);

        // At this point, there will be at least one question answered in a non full and relevant way.
        // we mark the ratios as not full and relevant and return.
        model.RatioRequirements = MarkAsNotFullAndRelevant(model.RatioRequirements);
        return (false, View(model));
    }

    /// <summary>
    ///     A function to take in the additional requirement questions and answers, match them up and check to see if the
    ///     user has answered any in a non full and relevant way.
    /// </summary>
    /// <param name="additionalRequirementsAnswers">This should come from the pre mapped questions and answers</param>
    /// <returns>True if we find any question answered in a non full and relevant way, false if none are found</returns>
    private static bool AnswersIndicateNotFullAndRelevant(
        List<AdditionalRequirementAnswerModel> additionalRequirementsAnswers)
    {
        return additionalRequirementsAnswers
            .Exists(answer =>
                        answer is
                            { AnswerToBeFullAndRelevant: true, Answer: "no" }
                            or
                            { AnswerToBeFullAndRelevant: false, Answer: "yes" });
    }

    private async Task CheckRatioRequirements(bool qualificationStartedBeforeSeptember2014,
                                              Qualification qualification,
                                              QualificationDetailsModel model)
    {
        // Build up property name to check for each level
        var beforeOrAfter =
            qualificationStartedBeforeSeptember2014
                ? "Before"
                : "After";

        var fullAndRelevantPropertyToCheck =
            $"FullAndRelevantForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";

        var additionalRequirementDetailPropertyToCheck =
            $"RequirementForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";

        const string additionalRequirementHeading = "RequirementHeading";

        model.RatioRequirements.ApprovedForLevel2 =
            GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level2RatioRequirementName,
                                   qualification);

        var requirementsForLevel2 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level2RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel2 = await contentParser.ToHtml(requirementsForLevel2);

        model.RatioRequirements.RequirementsHeadingForLevel2 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level2RatioRequirementName,
                                     qualification);

        model.RatioRequirements.ApprovedForLevel3 =
            GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level3RatioRequirementName,
                                   qualification);

        var requirementsForLevel3 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level3RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel3 = await contentParser.ToHtml(requirementsForLevel3);

        model.RatioRequirements.RequirementsHeadingForLevel3 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level3RatioRequirementName,
                                     qualification);

        model.RatioRequirements.ApprovedForLevel6 =
            GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level6RatioRequirementName,
                                   qualification);

        var requirementsForLevel6 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level6RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel6 = await contentParser.ToHtml(requirementsForLevel6);

        model.RatioRequirements.RequirementsHeadingForLevel6 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level6RatioRequirementName,
                                     qualification);

        model.RatioRequirements.ApprovedForUnqualified =
            GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.UnqualifiedRatioRequirementName,
                                   qualification);

        var requirementsForUnqualified = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                                    RatioRequirements.UnqualifiedRatioRequirementName,
                                                                    qualification);
        model.RatioRequirements.RequirementsForUnqualified = await contentParser.ToHtml(requirementsForUnqualified);

        model.RatioRequirements.RequirementsHeadingForUnqualified =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.UnqualifiedRatioRequirementName,
                                     qualification);
    }

    private T GetRatioProperty<T>(string propertyToCheck, string ratioName, Qualification qualification)
    {
        try
        {
            var requirement = qualification.RatioRequirements!.Find(x => x.RatioRequirementName == ratioName);

            return (T)requirement!.GetType().GetProperty(propertyToCheck)!.GetValue(requirement, null)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                            "Could not find property: {PropertyToCheck} within {RatioName} for qualification: {QualificationId}",
                            propertyToCheck, ratioName, qualification.QualificationId);
            throw;
        }
    }

    private static RatioRequirementModel MarkAsNotFullAndRelevant(RatioRequirementModel model)
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
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await qualificationsRepository.GetFilteredQualifications(level, startDateMonth, startDateYear,
                                                                        awardingOrganisation, searchCriteria);
    }

    private async Task<QualificationListModel> MapList(QualificationListPage content,
                                                       List<Qualification>? qualifications)
    {
        var basicQualificationsModels = GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel(content);

        return new QualificationListModel
               {
                   BackButton = MapToNavigationLinkModel(content.BackButton),
                   Filters = filterModel,
                   Header = content.Header,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await contentParser.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   LevelHeading = content.LevelHeading,
                   AwardingOrganisationHeading = content.AwardingOrganisationHeading,
                   PostSearchCriteriaContent = await contentParser.ToHtml(content.PostSearchCriteriaContent),
                   PostQualificationListContent = await contentParser.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels.OrderBy(x => x.QualificationName).ToList(),
                   NoResultText = await contentParser.ToHtml(content.NoResultsText),
                   ClearSearchText = content.ClearSearchText,
                   NoQualificationsFoundText = content.NoQualificationsFoundText
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

        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
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

    private async Task<QualificationDetailsModel> MapDetails(
        bool qualificationStartedBeforeSeptember2014,
        Qualification qualification,
        DetailsPage content)
    {
        var backNavLink = CalculateBackButton(qualificationStartedBeforeSeptember2014,
                                              content, qualification.QualificationId);

        var dateStarted = string.Empty;
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

        // ReSharper disable once InvertIf
        if (startYear is not null && startMonth is not null)
        {
            var dateOnly = new DateOnly(startYear.Value, startMonth.Value, 1);
            dateStarted = dateOnly.ToString("MMMM yyyy");
        }

        return new QualificationDetailsModel
               {
                   QualificationId = qualification.QualificationId,
                   QualificationLevel = qualification.QualificationLevel,
                   QualificationName = qualification.QualificationName,
                   QualificationNumber = qualification.QualificationNumber,
                   AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
                   FromWhichYear = qualification.FromWhichYear,
                   BackButton = MapToNavigationLinkModel(backNavLink),
                   AdditionalRequirementAnswers =
                       MapAdditionalRequirementAnswers(qualification.AdditionalRequirementQuestions),
                   DateStarted = dateStarted,
                   Content = new DetailsPageModel
                             {
                                 AwardingOrgLabel = content.AwardingOrgLabel,
                                 BookmarkHeading = content.BookmarkHeading,
                                 BookmarkText = content.BookmarkText,
                                 CheckAnotherQualificationHeading = content.CheckAnotherQualificationHeading,
                                 CheckAnotherQualificationText =
                                     await contentParser.ToHtml(content.CheckAnotherQualificationText),
                                 DateAddedLabel = content.DateAddedLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 FurtherInfoHeading = content.FurtherInfoHeading,
                                 FurtherInfoText = await contentParser.ToHtml(content.FurtherInfoText),
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 QualificationNumberLabel = content.QualificationNumberLabel,
                                 RequirementsHeading = content.RequirementsHeading,
                                 RequirementsText = await contentParser.ToHtml(content.RequirementsText),
                                 RatiosHeading = content.RatiosHeading,
                                 RatiosText = await contentParser.ToHtml(content.RatiosText),
                                 RatiosTextNotFullAndRelevant =
                                     await contentParser.ToHtml(content.RatiosTextNotFullAndRelevant),
                                 CheckAnotherQualificationLink =
                                     MapToNavigationLinkModel(content.CheckAnotherQualificationLink),
                                 PrintButtonText = content.PrintButtonText,
                                 QualificationNameLabel = content.QualificationNameLabel,
                                 QualificationStartDateLabel = content.QualificationStartDateLabel,
                                 QualificationDetailsSummaryHeader = content.QualificationDetailsSummaryHeader,
                                 FeedbackBanner = await MapToFeedbackBannerModel(content.FeedbackBanner, contentParser)
                             }
               };
    }

    private NavigationLink? CalculateBackButton(
        bool qualificationStartedBeforeSeptember2014,
        DetailsPage content,
        string qualificationId)
    {
        if (userJourneyCookieService.UserHasAnsweredAdditionalQuestions())
        {
            return ContentBackButtonLinkForAdditionalQuestions(content, qualificationId);
        }

        var level = userJourneyCookieService.GetLevelOfQualification();

        NavigationLink? backButton = null;

        if (userJourneyCookieService.GetQualificationWasSelectedFromList() != YesOrNo.Yes
            && level == 6)
        {
            // Advice is different for qualifications started before September 2014
            backButton = qualificationStartedBeforeSeptember2014
                             ? content.BackToLevelSixAdviceBefore2014
                             : content.BackToLevelSixAdvice;
        }

        return backButton ?? content.BackButton;
    }

    private static NavigationLink? ContentBackButtonLinkForAdditionalQuestions(
        DetailsPage content, string qualificationId)
    {
        var link = content.BackToAdditionalQuestionsLink;

        if (link == null)
        {
            return content.BackButton;
        }

        if (!link.Href.EndsWith($"/{qualificationId}", StringComparison.OrdinalIgnoreCase))
        {
            link.Href = $"{link.Href}/{qualificationId}";
        }

        return link;
    }

    private List<AdditionalRequirementAnswerModel>? MapAdditionalRequirementAnswers(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions)
    {
        if (additionalRequirementQuestions is null) return null;

        var additionalRequirementsAnswers = userJourneyCookieService.GetAdditionalQuestionsAnswers();

        var results = new List<AdditionalRequirementAnswerModel>();

        if (additionalRequirementsAnswers is null) return results;

        foreach (var additionalRequirementQuestion in additionalRequirementQuestions)
        {
            var answerToAdd = new AdditionalRequirementAnswerModel
                              {
                                  Question = additionalRequirementQuestion.Question,
                                  AnswerToBeFullAndRelevant = additionalRequirementQuestion.AnswerToBeFullAndRelevant,
                                  ConfirmationStatement = additionalRequirementQuestion.ConfirmationStatement
                              };

            if (additionalRequirementsAnswers.TryGetValue(additionalRequirementQuestion.Question, out var answer))
            {
                answerToAdd.Answer = answer;
            }

            results.Add(answerToAdd);
        }

        return results;
    }
}