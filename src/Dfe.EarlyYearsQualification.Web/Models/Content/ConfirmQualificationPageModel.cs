using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class ConfirmQualificationPageModel
{
    public string Heading { get; init; } = string.Empty;
    public string QualificationLabel { get; init; } = string.Empty;
    public string LevelLabel { get; init; } = string.Empty;
    public string AwardingOrganisationLabel { get; init; } = string.Empty;
    public string DateAddedLabel { get; init; } = string.Empty;
    public string RadioHeading { get; init; } = string.Empty;
    public List<OptionModel> Options { get; init; } = [];
    public bool HasErrors { get; set; }
    public string ErrorBannerHeading { get; init; } = string.Empty;
    public string ErrorBannerLink { get; init; } = string.Empty;
    public string ErrorText { get; init; } = string.Empty;
    [Required] public string? ConfirmQualificationAnswer { get; init; } = string.Empty;
    public string ButtonText { get; init; } = string.Empty;
    [Required] public string QualificationId { get; init; } = string.Empty;
    public string QualificationName { get; init; } = string.Empty;
    public string QualificationLevel { get; init; } = string.Empty;
    public string QualificationAwardingOrganisation { get; init; } = string.Empty;
    public string QualificationDateAdded { get; init; } = string.Empty;
    public NavigationLink? BackButton { get; init; }
}