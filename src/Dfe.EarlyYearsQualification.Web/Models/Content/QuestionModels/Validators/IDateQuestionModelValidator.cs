using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    DateValidationResult IsValid(DateQuestionModel model, DateQuestion question);

    DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage);

    DatesValidationResult IsValid(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage questionPage);

    DateValidationResult StartDateIsValid(DateQuestionModel model, DateQuestion question);

    bool DisplayAwardedDateBeforeStartDateError(DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion);
}