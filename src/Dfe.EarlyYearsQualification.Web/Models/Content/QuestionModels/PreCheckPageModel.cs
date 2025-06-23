using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class PreCheckPageModel
{
    public string Header { get; init; } = string.Empty;

    public NavigationLinkModel? BackButton { get; init; }

    public string PostHeaderContent { get; init; } = string.Empty;

    public string Question { get; init; } = string.Empty;
    
    public List<IOptionItemModel> OptionsItems { get; init; } = [];

    [Required]
    [IncludeInTelemetry]
    public string Option { get; init; } = string.Empty;

    public string InformationMessage { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;
    
    public bool HasErrors { get; set; }
    
    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;
}