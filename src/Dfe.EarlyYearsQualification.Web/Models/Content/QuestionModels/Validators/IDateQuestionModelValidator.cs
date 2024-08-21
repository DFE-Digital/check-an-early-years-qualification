using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    ValidationResult IsValid(DateQuestionModel model, DateQuestionPage questionPage);
}