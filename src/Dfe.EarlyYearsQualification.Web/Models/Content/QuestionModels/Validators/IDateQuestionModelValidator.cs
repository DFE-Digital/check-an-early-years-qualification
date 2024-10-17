using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public interface IDateQuestionModelValidator
{
    DateValidationResult IsValid(DateQuestionModel model, DateQuestionPage questionPage);
}