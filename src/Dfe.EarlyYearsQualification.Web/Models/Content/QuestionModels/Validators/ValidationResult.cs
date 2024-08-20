namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

public class ValidationResult
{
    public bool IsValid { get; init; }

    public string BannerErrorMessage { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;
}