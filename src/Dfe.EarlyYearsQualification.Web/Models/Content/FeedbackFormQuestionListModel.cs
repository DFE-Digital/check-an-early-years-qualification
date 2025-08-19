namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormQuestionListModel
{
    public string? Question { get; set; }

    public string? Answer { get; set; }

    public string? AdditionalInfo { get; set; }

    public bool HasError { get; set; }

    public string? ErrorMessage { get; set; }
}