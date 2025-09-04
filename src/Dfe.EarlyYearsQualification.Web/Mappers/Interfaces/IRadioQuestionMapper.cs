using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IRadioQuestionMapper
{
    Task<RadioQuestionModel> Map(RadioQuestionModel model,
                                 RadioQuestionPage question,
                                 string actionName,
                                 string controllerName,
                                 string? selectedAnswer);
}