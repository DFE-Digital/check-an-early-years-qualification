namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public abstract class BaseQuestionModel
{
    public string Question { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;

    public string ActionName { get; set; } = string.Empty;

    public string ControllerName { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public NavigationLinkModel? BackButton { get; set; }
    
    public string ErrorBannerHeading { get; set; } = string.Empty;

    public string ErrorBannerLinkText { get; set; } = string.Empty;
    
    public string AdditionalInformationHeader { get; set; } = string.Empty;

    public string AdditionalInformationBody { get; set; } = string.Empty;
}