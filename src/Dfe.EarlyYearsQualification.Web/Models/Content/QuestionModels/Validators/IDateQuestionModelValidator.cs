using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    DateValidationResult IsValid(DateQuestionModel model, DateQuestion question);

    DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage);

    DatesValidationResult IsValid(DatesQuestionModel model, HelpQualificationDetailsPage questionPage);

    bool DisplayAwardedDateBeforeStartDateError(DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion);
}