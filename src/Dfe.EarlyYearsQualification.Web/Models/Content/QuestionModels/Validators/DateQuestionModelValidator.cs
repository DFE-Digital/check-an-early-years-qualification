using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public static class DateQuestionModelValidator
{
    public static ValidationResult? IsValid(DateQuestionModel? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("Month and year must be entered");
        }

        var dateTimeAdapter = validationContext.GetService<IDateTimeAdapter>()!;

        if (value.SelectedYear < 1900)
        {
            return new ValidationResult("Year cannot be before 1900",
                                        new[] { nameof(DateQuestionModel.SelectedYear) });
        }

        if (value.SelectedMonth < 1 || value.SelectedMonth > 12)
        {
            return new ValidationResult("Month must be a number between 1 and 12",
                                        new[] { nameof(DateQuestionModel.SelectedMonth) });
        }

        var selectedDate = new DateOnly(value.SelectedYear, value.SelectedMonth, 1);

        var now = dateTimeAdapter.Now();

        if (selectedDate > DateOnly.FromDateTime(now))
        {
            return new ValidationResult("Month cannot be in the future",
                                        new[]
                                        {
                                            nameof(DateQuestionModel.SelectedMonth),
                                            nameof(DateQuestionModel.SelectedYear)
                                        });
        }

        return ValidationResult.Success;
    }
}