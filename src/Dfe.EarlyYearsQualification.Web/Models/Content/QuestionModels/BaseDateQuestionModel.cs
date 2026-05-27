namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class BaseDateQuestionModel
{
    public string QuestionHeader { get; set; } = string.Empty;

    public string QuestionHint { get; set; } = string.Empty;

    public string QuestionId { get; set; } = string.Empty;

    public string MonthId { get; set; } = string.Empty;

    public string YearId { get; set; } = string.Empty;

    public string MonthLabel { get; set; } = string.Empty;

    public string YearLabel { get; set; } = string.Empty;

    public bool MonthError { get; set; }

    public bool YearError { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public string Prefix { get; set; } = string.Empty;

    public List<ErrorSummaryLink> ErrorSummaryLinks { get; set; } = [];

    public bool DisplayHeader { get; set; } = true;

    public bool DisplayHint => !string.IsNullOrEmpty(QuestionHint);

    public string AriaDescribedBy => string.Join(" ", new[]
    {
        DisplayHeader ? $"{Prefix}-header" : null,
        DisplayHint ? $"{Prefix}-hint" : null,
        (MonthError || YearError) ? $"{Prefix}-error" : null
    }.Where(s => s != null));

    public string MonthClass => "govuk-input govuk-date-input__input govuk-input--width-2" + (MonthError ? " govuk-input--error" : "");

    public string YearClass => "govuk-input govuk-date-input__input govuk-input--width-4" + (YearError ? " govuk-input--error" : "");

    public bool HasError => MonthError || YearError;
}