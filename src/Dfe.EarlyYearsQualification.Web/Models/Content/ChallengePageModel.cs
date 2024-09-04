namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class ChallengePageModel
{
    public string MainHeading { get; set; } = string.Empty;

    public string MainContent { get; set; } = string.Empty;
    
    public string InputHeading { get; set; } = string.Empty;
    
    public string ShowPasswordButtonText { get; set; } = string.Empty;
    
    public string SubmitButtonText { get; set; } = string.Empty;

    public string FooterContent { get; set; } = string.Empty;

    public string RedirectAddress { get; set; } = string.Empty;

    public string PasswordValue { get; set; } = string.Empty;

    public ErrorSummaryModel? ErrorSummaryModel { get; set; }
}