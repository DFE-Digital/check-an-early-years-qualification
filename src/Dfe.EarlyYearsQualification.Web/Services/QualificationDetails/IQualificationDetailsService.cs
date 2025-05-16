using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;

public interface IQualificationDetailsService
{
    Task<Qualification?> GetQualification(string qualificationId);
    Task<DetailsPage?> GetDetailsPage();
    bool HasStartDate();
    Task<string?> GetFeedbackBannerBodyToHtml(FeedbackBanner? feedbackBanner);
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
    Task CheckRatioRequirements(Qualification qualification, QualificationDetailsModel model);

    (bool isFullAndRelevant, QualificationDetailsModel details) RemainingAnswersIndicateFullAndRelevant(
        QualificationDetailsModel details, AdditionalRequirementQuestion qtsQuestion);

    Task<QualificationDetailsModel> CheckLevel6Requirements(Qualification qualification,
                                                            QualificationDetailsModel details);

    bool DoAdditionalAnswersMatchQuestions(QualificationDetailsModel details);
    NavigationLink? CalculateBackButton(DetailsPage content, string qualificationId);

    List<AdditionalRequirementAnswerModel>? MapAdditionalRequirementAnswers(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions);

    Task<QualificationDetailsModel> MapDetails(Qualification qualification, DetailsPage content);
    Task SetRatioText(QualificationDetailsModel model, DetailsPage content);

    void SetQualificationResultSuccessDetails(QualificationDetailsModel model, DetailsPage content);

    void SetQualificationResultFailureDetails(QualificationDetailsModel model, DetailsPage content);
    
    Task ProcessNewRequirements(Qualification qualification, QualificationDetailsModel model);
}