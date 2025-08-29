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
}