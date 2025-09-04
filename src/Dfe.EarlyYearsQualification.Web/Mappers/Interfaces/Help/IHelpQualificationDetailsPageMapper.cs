using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;

public interface IHelpQualificationDetailsPageMapper
{
    public QualificationDetailsPageViewModel MapQualificationDetailsContentToViewModel(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content, DatesValidationResult? validationResult, ModelStateDictionary modelState);

    public DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question, DateValidationResult? validationResult);
}