using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateQuestionModelValidator(IDateTimeAdapter dateTimeAdapter) : IDateQuestionModelValidator
{
    public bool IsValid(DateQuestionModel model)
    {
        if (model.SelectedYear < 1900
            || model.SelectedMonth < 1
            || model.SelectedMonth > 12)
        {
            return false;
        }

        var selectedDate = new DateOnly(model.SelectedYear, model.SelectedMonth, 1);

        var now = dateTimeAdapter.Now();

        return selectedDate <= DateOnly.FromDateTime(now);
    }
}