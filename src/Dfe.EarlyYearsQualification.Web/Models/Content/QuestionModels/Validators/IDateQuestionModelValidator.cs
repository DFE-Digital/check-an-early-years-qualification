using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    DateValidationResult IsValid(DateQuestionModel model, DateQuestion question);
    DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage);
    bool IsAwardedDateAfterStartDate(DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion);
}