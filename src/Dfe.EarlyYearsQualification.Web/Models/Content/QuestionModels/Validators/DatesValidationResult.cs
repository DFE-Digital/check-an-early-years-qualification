namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DatesValidationResult
{
    public bool Valid => StartedValidationResult is { YearValid: true, MonthValid: true } &&
                         AwardedValidationResult is { YearValid: true, MonthValid: true };

    public DateValidationResult? StartedValidationResult { get; set; }
    public DateValidationResult? AwardedValidationResult { get; set; }
}