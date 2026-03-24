using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.ModelBinding;

public class RadioQuestionModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var model = new RadioQuestionModel();

        var optionResult = bindingContext.ValueProvider.GetValue(nameof(RadioQuestionModel.Option));
        if (optionResult != ValueProviderResult.None)
        {
            model.Option = optionResult.FirstValue ?? string.Empty;
            bindingContext.ModelState.SetModelValue(nameof(RadioQuestionModel.Option), optionResult);
        }

        var monthResult = bindingContext.ValueProvider.GetValue("Month");
        var yearResult = bindingContext.ValueProvider.GetValue("Year");

        int? selectedMonth = int.TryParse(monthResult.FirstValue, out var m) ? m : null;
        int? selectedYear = int.TryParse(yearResult.FirstValue, out var y) ? y : null;

        model.OptionsItems.Add(new RadioButtonAndDateInputModel
                               {
                                   Question = new DateQuestionModel
                                              {
                                                  SelectedMonth = selectedMonth,
                                                  SelectedYear = selectedYear
                                              }
                               });

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}