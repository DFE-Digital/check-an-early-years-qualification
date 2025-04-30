using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;

public class QualificationDetailsService(
    ILogger<QualificationDetailsService> logger,
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService
) : IQualificationDetailsService
{
    public async Task<Qualification?> GetQualification(string qualificationId)
    {
        return await qualificationsRepository.GetById(qualificationId);
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        return await contentService.GetDetailsPage();
    }

    public bool HasStartDate()
    {
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        return startDateMonth is not null && startDateYear is not null;
    }

    public async Task<string?> GetFeedbackBannerBodyToHtml(FeedbackBanner? feedbackBanner)
    {
        return feedbackBanner is not null
                   ? await contentParser.ToHtml(feedbackBanner.Body)
                   : null;
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

        // As we know that they didn't answer the Qts question, we need to show the L6 requirements by default.
        // Adding it here covers scenarios where they are OK for L2/3/Unqualified and just Unqualified.
        details.RatioRequirements.ShowRequirementsForLevel6ByDefault = true;

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

    public RatioRequirementModel MarkAsNotFullAndRelevant(RatioRequirementModel model)
    {
        model.ApprovedForLevel2 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel3 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForLevel6 = QualificationApprovalStatus.NotApproved;
        model.ApprovedForUnqualified = QualificationApprovalStatus.Approved;

        return model;
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
        details.RatioRequirements.ShowRequirementsForLevel6ByDefault = true;
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

    public async Task QualificationLevel3OrAboveMightBeRelevantAtLevel2(QualificationDetailsModel model,
                                                                        Qualification qualification)
    {
        // Check if the qualification is not full and relevant and was started between Sept 2014 and Aug 2019 and is above a level 2 qualification
        if (model.RatioRequirements.IsNotFullAndRelevant &&
            userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() &&
            qualification.QualificationLevel > 2)
        {
            // If the qualification is above a level 2 qualification, is not full and relevant and is started between Sept 2014 and Aug 2019
            // then it will have special requirements for level 2.
            model.RatioRequirements.ApprovedForLevel2 = QualificationApprovalStatus.FurtherActionRequired;
            var requirementsForLevel2 = GetRatioProperty<Document>("RequirementForLevel2BetweenSept14AndAug19",
                                                                   RatioRequirements.Level2RatioRequirementName,
                                                                   qualification);
            model.RatioRequirements.RequirementsForLevel2 = await contentParser.ToHtml(requirementsForLevel2);
            model.RatioRequirements.ShowRequirementsForLevel2ByDefault = true;
        }
    }

    public NavigationLink? CalculateBackButton(DetailsPage content, string qualificationId)
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

    public async Task CheckRatioRequirements(Qualification qualification, QualificationDetailsModel model)
    {
        // Build up property name to check for each level
        var beforeOrAfter = userJourneyCookieService.WasStartedBeforeSeptember2014() ? "Before" : "After";

        var fullAndRelevantPropertyToCheck =
            $"FullAndRelevantForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";

        var additionalRequirementDetailPropertyToCheck =
            $"RequirementForLevel{qualification.QualificationLevel}{beforeOrAfter}2014";

        if (qualification.IsAutomaticallyApprovedAtLevel6 || (QualificationContainsQtsQuestion(qualification) &&
                                                              UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification,
                                                                                                                    model.AdditionalRequirementAnswers)))
        {
            // Check user against QTS criteria and swap to Qts Criteria if matches
            fullAndRelevantPropertyToCheck = $"FullAndRelevantForQtsEtc{beforeOrAfter}2014";
            additionalRequirementDetailPropertyToCheck = $"RequirementForQtsEtc{beforeOrAfter}2014";
        }

        const string additionalRequirementHeading = "RequirementHeading";

        var approvedForLevel2 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck,
                                                       RatioRequirements.Level2RatioRequirementName,
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

        var approvedForLevel3 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck,
                                                       RatioRequirements.Level3RatioRequirementName,
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

        var approvedForLevel6 = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck,
                                                       RatioRequirements.Level6RatioRequirementName,
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

        var approvedForUnqualified = GetRatioProperty<bool>(fullAndRelevantPropertyToCheck,
                                                            RatioRequirements.UnqualifiedRatioRequirementName,
                                                            qualification);

        model.RatioRequirements.ApprovedForUnqualified = approvedForUnqualified
                                                             ? QualificationApprovalStatus.Approved
                                                             : QualificationApprovalStatus.NotApproved;

        var requirementsForUnqualified = GetRatioProperty<Document>(additionalRequirementDetailPropertyToCheck,
                                                                    RatioRequirements.UnqualifiedRatioRequirementName,
                                                                    qualification);
        model.RatioRequirements.RequirementsForUnqualified = await contentParser.ToHtml(requirementsForUnqualified);

        model.RatioRequirements.RequirementsHeadingForUnqualified =
            GetRatioProperty<string>(additionalRequirementHeading, RatioRequirements.UnqualifiedRatioRequirementName,
                                     qualification);
    }

    public async Task<QualificationDetailsModel> MapDetails(Qualification qualification, DetailsPage content)
    {
        var backNavLink = CalculateBackButton(content, qualification.QualificationId);

        var dateStarted = string.Empty;
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

        if (startYear is not null && startMonth is not null)
        {
            var dateOnly = new DateOnly(startYear.Value, startMonth.Value, 1);
            dateStarted = dateOnly.ToString("MMMM yyyy");
        }

        var dateAwarded = string.Empty;
        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();

        if (awardedYear is not null && awardedMonth is not null)
        {
            var dateOnly = new DateOnly(awardedYear.Value, awardedMonth.Value, 1);
            dateAwarded = dateOnly.ToString("MMMM yyyy");
        }
        
        var requirementsText = await contentParser.ToHtml(content.RequirementsText);
        var feedbackBodyHtml = await GetFeedbackBannerBodyToHtml(content.FeedbackBanner);
        var improveServiceBodyHtml = content.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(content.UpDownFeedback.ImproveServiceContent)
                                         : null;
        return QualificationDetailsMapper.Map(qualification, content, backNavLink,
                                              MapAdditionalRequirementAnswers(qualification
                                                                                  .AdditionalRequirementQuestions),
                                              dateStarted, dateAwarded,
                                              requirementsText, feedbackBodyHtml, improveServiceBodyHtml);
    }

    public async Task SetRatioText(QualificationDetailsModel model, DetailsPage content)
    {
        switch (model.RatioRequirements.IsNotFullAndRelevant)
        {
            case true:
                await SetRatioTextWhereIsNotFullAndRelevant(model, content);
                break;
            case false:
                await SetRatioTextWhereIsFullAndRelevant(model, content);
                break;
        }
    }

    private async Task SetRatioTextWhereIsFullAndRelevant(QualificationDetailsModel model, DetailsPage content)
    {
        var wasAwardedBeforeSeptember2014 = userJourneyCookieService.WasAwardedBeforeSeptember2014();
        var wasAwardedOnOrAfterSeptember2014 = userJourneyCookieService.WasAwardedOnOrAfterSeptember2014();
        var wasAwardedBeforeJune2016 = userJourneyCookieService.WasAwardedBeforeJune2016();
        var wasAwardedInJune2016 = userJourneyCookieService.WasAwardedInJune2016();
        var wasAwardedAfterJune2016 = userJourneyCookieService.WasAwardedAfterJune2016();
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
                                     };
        switch (model.QualificationLevel)
        {
            case 2 when wasAwardedBeforeJune2016:
            case 3 or 4 or 5 when wasAwardedBeforeSeptember2014:
            case 6 or 7 when approvedAllLevels:
            case 6 or 7 when approvedAllLevelsButL6 && wasAwardedBeforeSeptember2014:
                model.Content!.RatiosText = string.Empty;
                break;

            case 2 when wasAwardedInJune2016:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosTextMaybePfa);
                break;

            case 2 when wasAwardedAfterJune2016:
            case 3 or 4 or 5 when wasAwardedOnOrAfterSeptember2014:
            case 6 or 7 when approvedAllLevelsButL6 && wasAwardedOnOrAfterSeptember2014:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosTextPfa);
                break;

            default:
                model.Content!.RatiosText = await contentParser.ToHtml(content.RatiosText);
                break;
        }
    }

    private async Task SetRatioTextWhereIsNotFullAndRelevant(QualificationDetailsModel model, DetailsPage content)
    {
        var wasStartedBetweenSeptember2014AndAugust2019 = userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019();
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

    public void SetQualificationResultSuccessDetails(QualificationDetailsModel model, DetailsPage content)
    {
        model.Content!.QualificationResultHeading = content.QualificationResultHeading;
        model.Content.QualificationResultMessageHeading = content.QualificationResultFrMessageHeading;
        model.Content.QualificationResultMessageBody = content.QualificationResultFrMessageBody;
    }

    public void SetQualificationResultFailureDetails(QualificationDetailsModel model, DetailsPage content)
    {
        model.Content!.QualificationResultHeading = content.QualificationResultHeading;

        if (model.RatioRequirements.IsNotFullAndRelevant && model.QualificationLevel > 2 &&
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
}