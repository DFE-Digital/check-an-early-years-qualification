using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;

public interface IQualificationDetailsService
{
    Task<List<Qualification>> GetFilteredQualifications(string? searchCriteriaOverride = null);
    
    Task<Qualification?> GetQualificationById(string qualificationId);

    Task<QualificationDetailsModel> MapDetails(Qualification qualification, QualificationDetailsPage content, bool isFullAndRelevant);

    bool HasStartDate();

    bool QualificationContainsQtsQuestion(Qualification qualification);

    bool UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(Qualification qualification,
                                                               List<AdditionalRequirementAnswerModel>?
                                                                   additionalRequirementAnswerModels);

    bool AnswersIndicateNotFullAndRelevant(List<AdditionalRequirementAnswerModel> additionalRequirementsAnswers);

    (bool isFullAndRelevant, QualificationDetailsModel details) RemainingAnswersIndicateFullAndRelevant(
        QualificationDetailsModel details, AdditionalRequirementQuestion qtsQuestion);

    Task<QualificationDetailsModel> CheckLevel6Requirements(Qualification qualification,
                                                            QualificationDetailsModel details);

    bool DoAdditionalAnswersMatchQuestions(QualificationDetailsModel details);

    List<AdditionalRequirementAnswerModel>? MapAdditionalRequirementAnswers(
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions);
    
    Task SetRatioRequirements(Qualification qualification, QualificationDetailsModel model, QualificationDetailsPage pageContent, bool isFullAndRelevant);

    Task SetRatioText(QualificationDetailsModel model, DetailsPageLabels content);

    void SetQualificationResultSuccessDetails(QualificationDetailsModel model, DetailsPageLabels content);

    void SetQualificationResultFailureDetails(QualificationDetailsModel model, DetailsPageLabels content);

    bool GetUserIsCheckingOwnQualification();

    int? GetLevelOfQualification();

    (int? startMonth, int? startYear) GetWhenWasQualificationStarted();
    
    (int? awardedMonth, int? awardedYear) GetWhenWasQualificationAwarded();

    Task<QualificationDetailsPage?> GetQualificationDetailsPage(bool userIsCheckingOwnQualification,
                                                                bool isFullAndRelevant, int level, int startMonth,
                                                                int startYear, int awardedMonth,
                                                                int awardedYear, Qualification qualification);
}