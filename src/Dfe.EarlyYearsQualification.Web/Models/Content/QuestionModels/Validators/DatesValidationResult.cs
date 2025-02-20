namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DatesValidationResult
{
    public bool Valid => DatesValid && StartedValidationResult is { YearValid: true, MonthValid: true } &&
                         AwardedValidationResult is { YearValid: true, MonthValid: true };

    public bool DatesValid { get; set; } = true;
    public DateValidationResult? StartedValidationResult { get; set; }
    public DateValidationResult? AwardedValidationResult { get; set; }
}