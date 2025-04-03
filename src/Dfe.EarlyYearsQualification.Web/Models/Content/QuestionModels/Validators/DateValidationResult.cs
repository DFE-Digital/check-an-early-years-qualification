namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class DateValidationResult
{
    public bool MonthValid { get; set; } = true;

    public bool YearValid { get; set; } = true;

    public List<BannerError> BannerErrorMessages { get; set;  } = [];

    public List<string> ErrorMessages { get; init; } = [];
}