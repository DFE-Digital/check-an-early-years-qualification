using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RatioRequirements;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;

public class QualificationDetailsService(
    ILogger<QualificationDetailsService> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    IPlaceholderUpdater placeholderUpdater,
    IQualificationDetailsMapper qualificationDetailsMapper,
    IQualificationSearchService qualificationSearchService
) : IQualificationDetailsService
{
    public async Task<List<Qualification>> GetFilteredQualifications(string? searchCriteriaOverride = null)
    {
        return await qualificationSearchService.GetFilteredQualifications(searchCriteriaOverride);
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        return await qualificationSearchService.GetQualificationById(qualificationId);
    }

    public async Task<QualificationDetailsPage?> GetQualificationDetailsPage(bool userIsCheckingOwnQualification,
                                                                             bool isFullAndRelevant, int level,
                                                                             int startMonth, int startYear,
                                                                             int awardedMonth,
                                                                             int awardedYear,Qualification qualification)
    {
        var additionalRequirementAnswerModels =
            MapAdditionalRequirementAnswers(qualification.AdditionalRequirementQuestions);
        var isApprovedAtL6SpecificPage = IsQts(qualification, additionalRequirementAnswerModels);
        var getDegreeSpecificPage = qualification.IsTheQualificationADegree;
        return await contentService.GetQualificationDetailsPage(userIsCheckingOwnQualification, isFullAndRelevant,
                                                                level, startMonth, startYear, awardedMonth, awardedYear,
                                                                getDegreeSpecificPage, isApprovedAtL6SpecificPage);
    }

    public bool HasStartDate()
    {
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        return startDateMonth is not null && startDateYear is not null;
    }

    public void SetQualificationResultSuccessDetails(QualificationDetailsModel model, DetailsPageLabels content)
    {
        model.Content!.QualificationResultHeading = content.QualificationResultHeading;
        model.Content.QualificationResultMessageHeading = content.QualificationResultFrMessageHeading;
        model.Content.QualificationResultMessageBody = content.QualificationResultFrMessageBody;
    }

    public void SetQualificationResultFailureDetails(QualificationDetailsModel model, DetailsPageLabels content)
    {
        model.Content!.QualificationResultHeading = content.QualificationResultHeading;

        if (!model.RatioRequirements.IsFullAndRelevant && model.QualificationLevel > 2 &&
            userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019())
        {
            if (model.QualificationLevel < 6)
            {
                model.Content.QualificationResultMessageHeading = content.QualificationResultNotFrL3MessageHeading;
                model.Content.QualificationResultMessageBody = content.QualificationResultNotFrL3MessageBody;
            }
            else
            {
                model.Content.QualificationResultMessageHeading = content.QualificationResultNotFrL3OrL6MessageHeading;
                model.Content.QualificationResultMessageBody = content.QualificationResultNotFrL3OrL6MessageBody;
            }
        }
        else
        {
            model.Content.QualificationResultMessageHeading = content.QualificationResultNotFrMessageHeading;
            model.Content.QualificationResultMessageBody = content.QualificationResultNotFrMessageBody;
        }
    }

    public bool GetUserIsCheckingOwnQualification()
    {
        return userJourneyCookieService.GetIsUserCheckingTheirOwnQualification() == Options.Yes;
    }

    public int? GetLevelOfQualification()
    {
        return userJourneyCookieService.GetLevelOfQualification();
    }

    public (int? startMonth, int? startYear) GetWhenWasQualificationStarted()
    {
        return userJourneyCookieService.GetWhenWasQualificationStarted();
    }
    
    public (int? awardedMonth, int? awardedYear) GetWhenWasQualificationAwarded()
    {
        return userJourneyCookieService.GetWhenWasQualificationAwarded();
    }

    public List<AdditionalRequirementAnswerModel>? MapAdditionalRequirementAnswers(
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

    public (bool isFullAndRelevant, QualificationDetailsModel details) RemainingAnswersIndicateFullAndRelevant(
        QualificationDetailsModel details, AdditionalRequirementQuestion qtsQuestion)
    {
        // Check remaining questions
        var answersToCheck = new List<AdditionalRequirementAnswerModel>();
        answersToCheck.AddRange(details.AdditionalRequirementAnswers!);
        // As L6 / L7 can potentially work at L3/2/unqualified, remove the Qts question and check answers
        answersToCheck.RemoveAll(x => x.Question == qtsQuestion.Question);
        
        return AnswersIndicateNotFullAndRelevant(answersToCheck)
                   ? (false, details)
                   : (true, details);
    }

    public bool QualificationContainsQtsQuestion(Qualification qualification)
    {
        return qualification.AdditionalRequirementQuestions != null
               && qualification.AdditionalRequirementQuestions.Exists(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                               .QtsQuestion);
    }
    
    public bool DoAdditionalAnswersMatchQuestions(QualificationDetailsModel details)
    {
        return details.AdditionalRequirementAnswers!.Count == 0 ||
               details.AdditionalRequirementAnswers.Exists(answer => string.IsNullOrEmpty(answer.Answer));
    }

    public async Task<QualificationDetailsModel> CheckLevel6Requirements(
        Qualification qualification, QualificationDetailsModel details)
    {
        // Answers indicate not full and relevant
        details.RatioRequirements = MarkAsNotFullAndRelevant(details.RatioRequirements);
        // Set any content for L6
        var beforeOrAfter = userJourneyCookieService.WasStartedBeforeSeptember2014() ? "Before" : "After";
        var additionalRequirementDetailPropertyToCheck =
            $"RequirementForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";
        var requirementsForLevel6 = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                               RatioRequirements.Level6RatioRequirementName,
                                                               qualification);
        details.RatioRequirements.RequirementsForLevel6 = await contentParser.ToHtml(requirementsForLevel6);
        //details.RatioRequirements.ShowRequirementsForLevel6ByDefault = true;
        return details;
    }

    /// <summary>
    ///     A function to take in the additional requirement questions and answers, match them up and check to see if the
    ///     user has answered any in a non full and relevant way.
    /// </summary>
    /// <param name="additionalRequirementsAnswers">This should come from the pre mapped questions and answers</param>
    /// <returns>True if we find any question answered in a non full and relevant way, false if none are found</returns>
    public bool AnswersIndicateNotFullAndRelevant(List<AdditionalRequirementAnswerModel> additionalRequirementsAnswers)
    {
        return additionalRequirementsAnswers
            .Exists(answer =>
                        answer is
                            { AnswerToBeFullAndRelevant: true, Answer: "no" }
                            or
                            { AnswerToBeFullAndRelevant: false, Answer: "yes" }
                   );
    }

    public bool UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(Qualification qualification,
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

    public async Task SetRatioRequirements(Qualification qualification, QualificationDetailsModel model,
                                           QualificationDetailsPage pageContent, bool isFullAndRelevant)
    {
        if (isFullAndRelevant)
        {
            // Build up property name to check for each level
            var beforeOrAfter = userJourneyCookieService.WasStartedBeforeSeptember2014() ? "Before" : "After";

            var fullAndRelevantPropertyToCheck =
                $"FullAndRelevantForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";

            if (qualification.IsAutomaticallyApprovedAtLevel6 || (QualificationContainsQtsQuestion(qualification) &&
                                                                  UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification,
                                                                   model.AdditionalRequirementAnswers)))
            {
                // Check user against QTS criteria and swap to Qts Criteria if matches
                fullAndRelevantPropertyToCheck = $"FullAndRelevantForQtsEtc{beforeOrAfter}2014";
            }

            // Set ratio requirement approved or not approved
            var approvedForLevel2 = GetFullAndRelevantRatioProperty(fullAndRelevantPropertyToCheck,
                                                                    new Level2RatioRequirements());

            model.RatioRequirements.ApprovedForLevel2 = approvedForLevel2
                                                            ? QualificationApprovalStatus.Approved
                                                            : QualificationApprovalStatus.NotApproved;

            var approvedForLevel3 = GetFullAndRelevantRatioProperty(fullAndRelevantPropertyToCheck,
                                                                    new Level3RatioRequirements());

            model.RatioRequirements.ApprovedForLevel3 = approvedForLevel3
                                                            ? QualificationApprovalStatus.Approved
                                                            : QualificationApprovalStatus.NotApproved;

            var approvedForLevel6 = GetFullAndRelevantRatioProperty(fullAndRelevantPropertyToCheck,
                                                                    new Level6RatioRequirements());

            model.RatioRequirements.ApprovedForLevel6 = approvedForLevel6
                                                            ? QualificationApprovalStatus.Approved
                                                            : QualificationApprovalStatus.NotApproved;

            var approvedForUnqualified = GetFullAndRelevantRatioProperty(fullAndRelevantPropertyToCheck,
                                                                         new UnqualifiedRatioRequirements());

            model.RatioRequirements.ApprovedForUnqualified = approvedForUnqualified
                                                                 ? QualificationApprovalStatus.Approved
                                                                 : QualificationApprovalStatus.NotApproved;
        }
        else
        {
            MarkAsNotFullAndRelevant(model.RatioRequirements);
        }

        // Set the text for the requirement levels to be read from page content
        model.RatioRequirements.RequirementsForLevel2 =
            placeholderUpdater.Replace(await contentParser.ToHtml(pageContent.Level2RatioRequirements));
        model.RatioRequirements.RequirementsForLevel3 =
            placeholderUpdater.Replace(await contentParser.ToHtml(pageContent.Level3RatioRequirements));
        model.RatioRequirements.RequirementsForLevel6 =
            placeholderUpdater.Replace(await contentParser.ToHtml(pageContent.Level6RatioRequirements));
        model.RatioRequirements.RequirementsForUnqualified =
            placeholderUpdater.Replace(await contentParser.ToHtml(pageContent.UnqualifiedRatioRequirements));

        // Check for possible overrides
        QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
        QualificationMayBeEligibleForEbr(model, qualification);
        QualificationMayBeEligibleForEyitt(model, qualification);
    }

    public async Task<QualificationDetailsModel> MapDetails(Qualification qualification,
                                                            QualificationDetailsPage content,
                                                            bool isFullAndRelevant, List<AdditionalRequirementAnswerModel>? additionalRequirementAnswers)
    {
        // Needed for displaying the qualification number if there is a duplicate cert with the same name.
        var filteredQualifications = await GetFilteredQualifications(qualification.QualificationName);
        var hasMultipleQualificationsWithSameName = false;
        if (filteredQualifications.Count != 0)
        {
            hasMultipleQualificationsWithSameName = filteredQualifications
                                                    .Select(x => x.QualificationName == qualification.QualificationName)
                                                    .Count() > 1;
        }

        var backNavLink = CalculateBackButton(content.Labels, qualification.QualificationId);

        var dateStarted = string.Empty;
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

        if (startYear is not null && startMonth is not null)
        {
            var dateOnly = new DateOnly(startYear.Value, startMonth.Value, 1);
            dateStarted = dateOnly < new DateOnly(2014, 9, 1)
                              ? "Before 1 September 2014"
                              : dateOnly.ToString("MMMM yyyy");
        }

        var dateAwarded = string.Empty;
        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();

        if (awardedYear is not null && awardedMonth is not null)
        {
            var dateOnly = new DateOnly(awardedYear.Value, awardedMonth.Value, 1);
            dateAwarded = dateOnly.ToString("MMMM yyyy");
        }

        return await qualificationDetailsMapper.Map(qualification, content, backNavLink,
                                                    additionalRequirementAnswers,
                                                    dateStarted, dateAwarded, hasMultipleQualificationsWithSameName, isFullAndRelevant);
    }

    public async Task SetRatioText(QualificationDetailsModel model, DetailsPageLabels content)
    {
        switch (model.RatioRequirements.IsFullAndRelevant)
        {
            case true:
                SetRatioTextWhereIsFullAndRelevant(model);
                break;
            case false:
                await SetRatioTextWhereIsNotFullAndRelevant(model, content);
                break;
        }
    }

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Checks if a qualification is eligible for Early Years Initial Teacher Training, upon completion of which will
    /// allow the holder to gain Early Years Teacher Status (EYTS)
    /// </summary>
    /// <param name="model">The mapped qualification details</param>
    /// <param name="qualification">The qualification data from Contentful</param>
    private static void QualificationMayBeEligibleForEyitt(QualificationDetailsModel model, Qualification qualification)
    {
        var isQts = IsQts(qualification, model.AdditionalRequirementAnswers);
        if (model.RatioRequirements.ApprovedForLevel6 != QualificationApprovalStatus.Approved
            && qualification is { QualificationLevel: 6, IsTheQualificationADegree: true }
            && !isQts)
        {
            model.RatioRequirements.ApprovedForLevel6 = QualificationApprovalStatus.PossibleRouteAvailable;
        }
    }

    private static void QualificationMayBeEligibleForEbr(QualificationDetailsModel model, Qualification qualification)
    {
        var ebrEligible = (model.RatioRequirements.IsFullAndRelevant && qualification.QualificationLevel == 2) ||
                          (!model.RatioRequirements.IsFullAndRelevant && qualification.QualificationLevel >= 3);
        if (ebrEligible)
        {
            model.RatioRequirements.ApprovedForLevel3 = QualificationApprovalStatus.PossibleRouteAvailable;
        }
    }

    private void QualificationLevel3OrAboveMightBeRelevantAtLevel2(QualificationDetailsModel model,
                                                                   Qualification qualification)
    {
        // Check if the qualification is not full and relevant and was started between Sept 2014 and Aug 2019 and is above a level 2 qualification
        if (!model.RatioRequirements.IsFullAndRelevant &&
            userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() &&
            qualification.QualificationLevel > 2)
        {
            // If the qualification is above a level 2 qualification, is not full and relevant and is started between Sept 2014 and Aug 2019
            // then policy have confirmed it can be automatically approved at L2
            model.RatioRequirements.ApprovedForLevel2 = QualificationApprovalStatus.Approved;
        }
    }

    private NavigationLink? CalculateBackButton(DetailsPageLabels content, string qualificationId)
    {
        if (userJourneyCookieService.UserHasAnsweredAdditionalQuestions())
        {
            var link = content.BackToConfirmAnswers;
            if (link == null) return content.BackButton;
            link.Href = link.Href.Replace("$[qualification-id]$", qualificationId);
            return link;
        }

        return content.BackButton;
    }

    private static bool IsQts(Qualification qualification,
                              List<AdditionalRequirementAnswerModel>?
                                  additionalRequirementAnswerModels)
    {
        if (qualification.IsAutomaticallyApprovedAtLevel6)
        {
            return true;
        }

        if (additionalRequirementAnswerModels is null || qualification.AdditionalRequirementQuestions is null)
        {
            return false;
        }

        var qtsQuestion =
            qualification.AdditionalRequirementQuestions.FirstOrDefault(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                                 .QtsQuestion);
        if (qtsQuestion is null) return false;
        var userAnsweredQuestion = additionalRequirementAnswerModels.First(x => x.Question == qtsQuestion.Question);
        var answerAsBool = userAnsweredQuestion.Answer == "yes";
        return qtsQuestion.AnswerToBeFullAndRelevant == answerAsBool;
    }

    private void SetRatioTextWhereIsFullAndRelevant(QualificationDetailsModel model)
    {
        var wasAwardedBeforeSeptember2014 = userJourneyCookieService.WasAwardedBeforeSeptember2014();
        var wasAwardedBeforeJune2016 = userJourneyCookieService.WasAwardedBeforeJune2016();
        var approvedAllLevels = model.RatioRequirements is
                                {
                                    ApprovedForUnqualified: QualificationApprovalStatus.Approved,
                                    ApprovedForLevel2: QualificationApprovalStatus.Approved,
                                    ApprovedForLevel3: QualificationApprovalStatus.Approved,
                                    ApprovedForLevel6: QualificationApprovalStatus.Approved
                                };

        var approvedAllLevelsButL6 = model.RatioRequirements is
                                     {
                                         ApprovedForUnqualified: QualificationApprovalStatus.Approved,
                                         ApprovedForLevel2: QualificationApprovalStatus.Approved,
                                         ApprovedForLevel3: QualificationApprovalStatus.Approved,
                                         ApprovedForLevel6: QualificationApprovalStatus.NotApproved
                                                            or QualificationApprovalStatus.PossibleRouteAvailable
                                     };
        switch (model.QualificationLevel)
        {
            case 2 when wasAwardedBeforeJune2016:
            case 3 or 4 or 5 when wasAwardedBeforeSeptember2014:
            case 6 or 7 when approvedAllLevels:
            case 6 or 7 when approvedAllLevelsButL6 && wasAwardedBeforeSeptember2014:
                model.Content!.RatiosText = string.Empty;
                break;
        }
    }

    private async Task SetRatioTextWhereIsNotFullAndRelevant(QualificationDetailsModel model, DetailsPageLabels content)
    {
        var wasStartedBetweenSeptember2014AndAugust2019 =
            userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019();
        var wasStartedBeforeSeptember2014 = userJourneyCookieService.WasStartedBeforeSeptember2014();
        var wasStartedOnOrAfterSeptember2019 = userJourneyCookieService.WasStartedOnOrAfterSeptember2019();

        switch (model.QualificationLevel)
        {
            case >= 3 when wasStartedBetweenSeptember2014AndAugust2019:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosTextL3PlusNotFrBetweenSep14Aug19);
                model.Content!.RatiosAdditionalInfoText = await contentParser.ToHtml(content.RatiosTextL3Ebr);
                break;
            case >= 3 when wasStartedBeforeSeptember2014 || wasStartedOnOrAfterSeptember2019:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosTextNotFullAndRelevant);
                model.Content!.RatiosAdditionalInfoText = await contentParser.ToHtml(content.RatiosTextL3Ebr);
                break;
            default:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosTextNotFullAndRelevant);
                break;
        }
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

    private bool GetFullAndRelevantRatioProperty(string propertyToCheck, BaseRatioRequirement ratioRequirement)
    {
        try
        {
            return (bool)ratioRequirement.GetType().GetProperty(propertyToCheck)!.GetValue(ratioRequirement, null)!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                            "Could not find property: {PropertyToCheck} for ratio requirement with name {name}",
                            propertyToCheck, nameof(ratioRequirement));
            throw;
        }
    }
    
    private RatioRequirementModel MarkAsNotFullAndRelevant(RatioRequirementModel model)
    {
        model.ApprovedForLevel2 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel3 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel6 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForUnqualified = QualificationApprovalStatus.Approved;

        return model;
    }
}