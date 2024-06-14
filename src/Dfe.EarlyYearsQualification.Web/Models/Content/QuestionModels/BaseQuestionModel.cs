namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public abstract class BaseQuestionModel
{
    public string Question { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;

    public string ActionName { get; set; } = string.Empty;

    public string ControllerName { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public bool HasErrors { get; set; }

    
}