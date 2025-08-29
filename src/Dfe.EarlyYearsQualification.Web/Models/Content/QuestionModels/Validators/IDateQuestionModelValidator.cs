using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    DateValidationResult IsValid(DateQuestionModel model, DateQuestion question);
    DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage);
    DatesValidationResult StartDateIsOptionalIsValid(HelpQualificationDetailsPage content, QualificationDetailsPageViewModel viewModel);
    bool DisplayAwardedDateBeforeStartDateError(DateQuestionModel startedQuestion, DateQuestionModel awardedQuestion);
}