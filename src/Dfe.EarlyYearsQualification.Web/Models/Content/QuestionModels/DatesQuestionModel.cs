namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class DatesQuestionModel : BaseQuestionModel
{
    public bool HasErrors { get; set; }
    public ErrorSummaryModel? Errors { get; set; }
    public DateQuestionModel? StartedQuestion { get; set; }
    public DateQuestionModel? AwardedQuestion { get; set; }
}