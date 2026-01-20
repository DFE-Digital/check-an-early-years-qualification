using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;

public interface IQualificationDetailsService
{
    Task<List<Qualification>> GetFilteredQualifications(string? searchCriteriaOverride = null);
    
    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<QualificationDetailsModel> MapDetails(Qualification qualification, QualificationDetailsPage content,
                                               List<Qualification> qualifications);

    bool HasStartDate();

    bool QualificationContainsQtsQuestion(Qualification qualification);

    bool UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(Qualification qualification,
                                                               List<AdditionalRequirementAnswerModel>?
                                                                   additionalRequirementAnswerModels);

    bool AnswersIndicateNotFullAndRelevant(List<AdditionalRequirementAnswerModel> additionalRequirementsAnswers);

    RatioRequirementModel MarkAsNotFullAndRelevant(RatioRequirementModel model);

    Task QualificationLevel3OrAboveMightBeRelevantAtLevel2(QualificationDetailsModel model,
                                                           Qualification qualification);

    Task QualificationMayBeEligibleForEbr(QualificationDetailsModel model,
                                          Qualification qualification);

    // ReSharper disable once IdentifierTypo
    Task QualificationMayBeEligibleForEyitt(QualificationDetailsModel model,
                                            Qualification qualification);

    Task CheckRatioRequirements(Qualification qualification, QualificationDetailsModel model);

    (bool isFullAndRelevant, QualificationDetailsModel details) RemainingAnswersIndicateFullAndRelevant(
        QualificationDetailsModel details, AdditionalRequirementQuestion qtsQuestion);

    Task<QualificationDetailsModel> CheckLevel6Requirements(Qualification qualification,
                                                            QualificationDetailsModel details);

    bool DoAdditionalAnswersMatchQuestions(QualificationDetailsModel details);

    NavigationLink? CalculateBackButton(DetailsPageLabels content, string qualificationId);

    List<AdditionalRequirementAnswerModel>? MapAdditionalRequirementAnswers(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions);

    Task SetRatioText(QualificationDetailsModel model, DetailsPageLabels content);

    void SetQualificationResultSuccessDetails(QualificationDetailsModel model, DetailsPageLabels content);

    void SetQualificationResultFailureDetails(QualificationDetailsModel model, DetailsPageLabels content);

    Task SetRequirementOverrides(Qualification qualification, QualificationDetailsModel model);

    Task SetDefaultCardContentForApprovedQualifications(Qualification qualification, QualificationDetailsModel model);

    bool GetUserIsCheckingOwnQualification();

    int? GetLevelOfQualification();

    (int? startMonth, int? startYear) GetWhenWasQualificationStarted();

    Task<QualificationDetailsPage?> GetQualificationDetailsPage(bool userIsCheckingOwnQualification,
                                                                bool isFullAndRelevant, int level, int startMonth,
                                                                int startYear, Qualification qualification,
                                                                List<AdditionalRequirementAnswerModel>?
                                                                    additionalRequirementAnswerModels);
}