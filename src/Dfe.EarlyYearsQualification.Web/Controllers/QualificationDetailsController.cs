using System.Globalization;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models;
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

        var qualification = await qualificationsRepository.GetById(qualificationId);
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

        var validateAdditionalRequirementQuestions = await ValidateAdditionalQuestions(model, qualification);

        if (!validateAdditionalRequirementQuestions.isValid)
        {
            await QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
            return validateAdditionalRequirementQuestions.actionResult!;
        }

        // If all the additional requirement checks pass, then we can go to check each level individually
        await CheckRatioRequirements(qualificationStartedBeforeSeptember2014, qualification, model);
        
        await QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);

        return View(model);
    }

    private async Task QualificationLevel3OrAboveMightBeRelevantAtLevel2(QualificationDetailsModel model, Qualification qualification)
    {
        // Check if the qualification is not full and relevant and was started between Sept 2014 and Aug 2019 and is above a level 2 qualification
        if (model.RatioRequirements.IsNotFullAndRelevant && userJourneyCookieService.WasStartedBetweenSept2014AndAug2019() && qualification.QualificationLevel > 2)
        {
            await QualIsNotLevel2NotApprovedAndStartedBetweenSept2014AndAug2019(model, qualification);
        }
    }

    private async Task<(bool isValid, IActionResult? actionResult)> ValidateAdditionalQuestions(QualificationDetailsModel model, Qualification qualification)
    {
        // If the qualification has no additional requirements then skip all checks and return.
        if (model.AdditionalRequirementAnswers == null) return (true, null);
        
        // If qualification contains the QTS question, check the answers
        if (QualificationContainsQtsQuestion(qualification))
        {
            var qtsQuestion =
                qualification.AdditionalRequirementQuestions!.First(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                             .QtsQuestion);
            
            if (UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, model.AdditionalRequirementAnswers))
            {
                // Remove the additional requirements that they didn't answer following the bypass.
                model.AdditionalRequirementAnswers.RemoveAll(x => x.Question != qtsQuestion.Question);
                return (true, null);
            }
            
            // Check remaining questions
            var answersToCheck = new List<AdditionalRequirementAnswerModel>();
            answersToCheck.AddRange(model.AdditionalRequirementAnswers);
            // As L6 / L7 can potentially work at L3/2/unqualified, remove the Qts question and check answers
            answersToCheck.RemoveAll(x => x.Question == qtsQuestion.Question);
            
            // As we know that they didn't answer the Qts question, we need to show the L6 requirements by default.
            // Adding it here covers scenarios where they are OK for L2/3/Unqualified and just Unqualified.
            model.RatioRequirements.ShowRequirementsForLevel6ByDefault = true;
            
            if (!AnswersIndicateNotFullAndRelevant(answersToCheck)) return (true, null);
            
            // Answers indicate not full and relevant
            model.RatioRequirements = MarkAsNotFullAndRelevant(model.RatioRequirements);
            // Set any content for L6
            var qualificationStartedBefore2014 = userJourneyCookieService.WasStartedBeforeSeptember2014();
            var beforeOrAfter =
                qualificationStartedBefore2014
                    ? "Before"
                    : "After";
            var additionalRequirementDetailPropertyToCheck = $"RequirementForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";
            var requirementsForLevel6 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                                   RatioRequirements.Level6RatioRequirementName,
                                                                   qualification);
            model.RatioRequirements.RequirementsForLevel6 = await contentParser.ToHtml(requirementsForLevel6);
            model.RatioRequirements.ShowRequirementsForLevel6ByDefault = true;
            
            return (false, View(model));
        }
        
        // If there is a mismatch between the questions answered, then clear the answers and navigate back to the additional requirements check page
        if (model.AdditionalRequirementAnswers.Count == 0 ||
            model.AdditionalRequirementAnswers.Exists(answer => string.IsNullOrEmpty(answer.Answer)))
        {
            return (false,
                    RedirectToAction("Index", "CheckAdditionalRequirements",
                                     new { model.QualificationId, questionIndex = 1 }));
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
    
    /// If the qualification is above a level 2 qualification, is not full and relevant and is started between Sept 2014 and Aug 2019
    /// then it will have special requirements for level 2.
    private async Task QualIsNotLevel2NotApprovedAndStartedBetweenSept2014AndAug2019(QualificationDetailsModel model, Qualification qualification)
    {
        model.RatioRequirements.ApprovedForLevel2 = QualificationApprovalStatus.FurtherActionRequired;
        var requirementsForLevel2 = GetRatioProperty<Document>("RequirementForLevel2BetweenSept14AndAug19",
                                                               RatioRequirements.Level2RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel2 = await contentParser.ToHtml(requirementsForLevel2);
        model.RatioRequirements.ShowRequirementsForLevel2ByDefault = true;
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
        
        if (qualification.IsAutomaticallyApprovedAtLevel6 || 
            (QualificationContainsQtsQuestion(qualification) 
             && UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, model.AdditionalRequirementAnswers)))
        {
            // Check user against QTS criteria and swap to Qts Criteria if matches
            fullAndRelevantPropertyToCheck = $"FullAndRelevantForQtsEtc{beforeOrAfter}2014";
            additionalRequirementDetailPropertyToCheck = $"RequirementForQtsEtc{beforeOrAfter}2014";
        }

        const string additionalRequirementHeading = "RequirementHeading";

        var approvedForLevel2 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level2RatioRequirementName,
                                                       qualification);

        model.RatioRequirements.ApprovedForLevel2 = approvedForLevel2
                                                        ? QualificationApprovalStatus.Approved
                                                        : QualificationApprovalStatus.NotApproved;
            

        var requirementsForLevel2 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level2RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel2 = await contentParser.ToHtml(requirementsForLevel2);

        model.RatioRequirements.RequirementsHeadingForLevel2 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level2RatioRequirementName,
                                     qualification);

        var approvedForLevel3 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level3RatioRequirementName,
                                                       qualification);
        
        model.RatioRequirements.ApprovedForLevel3 = approvedForLevel3
                                                       ? QualificationApprovalStatus.Approved
                                                       : QualificationApprovalStatus.NotApproved;
            

        var requirementsForLevel3 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level3RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel3 = await contentParser.ToHtml(requirementsForLevel3);

        model.RatioRequirements.RequirementsHeadingForLevel3 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level3RatioRequirementName,
                                     qualification);
        
        var approvedForLevel6 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.Level6RatioRequirementName,
                                                       qualification);

        model.RatioRequirements.ApprovedForLevel6 = approvedForLevel6
                                                        ? QualificationApprovalStatus.Approved
                                                        : QualificationApprovalStatus.NotApproved;

        var requirementsForLevel6 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level6RatioRequirementName,
                                                               qualification);
        model.RatioRequirements.RequirementsForLevel6 = await contentParser.ToHtml(requirementsForLevel6);

        model.RatioRequirements.RequirementsHeadingForLevel6 =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.Level6RatioRequirementName,
                                     qualification);

        var approvedForUnqualified = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck, RatioRequirements.UnqualifiedRatioRequirementName, qualification);
        
        model.RatioRequirements.ApprovedForUnqualified = approvedForUnqualified
                                                             ? QualificationApprovalStatus.Approved
                                                             : QualificationApprovalStatus.NotApproved;
            

        var requirementsForUnqualified = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                                    RatioRequirements.UnqualifiedRatioRequirementName,
                                                                    qualification);
        model.RatioRequirements.RequirementsForUnqualified = await contentParser.ToHtml(requirementsForUnqualified);

        model.RatioRequirements.RequirementsHeadingForUnqualified =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.UnqualifiedRatioRequirementName, qualification);
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
        model.ApprovedForLevel2 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel3 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel6 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForUnqualified = QualificationApprovalStatus.Approved;

        return model;
    }
    
    private static bool QualificationContainsQtsQuestion(Qualification qualification)
    {
        return qualification.AdditionalRequirementQuestions != null
               && qualification.AdditionalRequirementQuestions.Any(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                            .QtsQuestion);
    }

    private static bool UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(Qualification qualification,
                                                                              List<AdditionalRequirementAnswerModel>?
                                                                                  additionalRequirementAnswerModels)
    {
        if (additionalRequirementAnswerModels is null)
        {
            return false;
        }
        
        var qtsQuestion =
            qualification.AdditionalRequirementQuestions!.First(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                         .QtsQuestion);

        var userAnsweredQuestion = additionalRequirementAnswerModels.First(x => x.Question == qtsQuestion.Question);
        var answerAsBool = userAnsweredQuestion.Answer == "yes";
        return qtsQuestion.AnswerToBeFullAndRelevant == answerAsBool;
    }
    
    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await qualificationsRepository.Get(level, startDateMonth, startDateYear,
                                                  awardingOrganisation, searchCriteria);
    }

    private async Task<QualificationListModel> MapList(QualificationListPage content,
                                                       List<Qualification>? qualifications)
    {
        var basicQualificationsModels = GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel(content);

        return new QualificationListModel
               {
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
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
        
        var checkAnotherQualificationText = await contentParser.ToHtml(content.CheckAnotherQualificationText);
        var furtherInfoText = await contentParser.ToHtml(content.FurtherInfoText);
        var requirementsText = await contentParser.ToHtml(content.RequirementsText);
        var ratiosText = await contentParser.ToHtml(content.RatiosText);
        var ratiosTextNotFullAndRelevant =
            await contentParser.ToHtml(content.RatiosTextNotFullAndRelevant);
        var feedbackBodyHtml = await GetFeedbackBannerBodyToHtml(content.FeedbackBanner, contentParser);

        return QualificationDetailsMapper.Map(qualification, content, backNavLink,
                                              MapAdditionalRequirementAnswers(qualification
                                                                                  .AdditionalRequirementQuestions),
                                              dateStarted, checkAnotherQualificationText, furtherInfoText,
                                              requirementsText, ratiosText, ratiosTextNotFullAndRelevant,
                                              feedbackBodyHtml);
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
        const string placeholder = "$[qualification-id]$";
        var link = content.BackToConfirmAnswers;

        if (link == null)
        {
            return content.BackButton;
        }

        link.Href = link.Href.Replace(placeholder, qualificationId);

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