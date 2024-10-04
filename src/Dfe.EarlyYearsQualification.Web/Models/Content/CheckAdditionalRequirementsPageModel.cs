using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class CheckAdditionalRequirementsPageModel
{
    [Required]
    public string QualificationId { get; set; } = string.Empty;

    [Required]
    public int? QuestionId { get; set; }

    public string Heading { get; set; } = string.Empty;

    public string QuestionSectionHeading { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;

    public NavigationLinkModel? BackButton { get; set; }

    public AdditionalRequirementQuestionModel AdditionalRequirementQuestion { get; set; } = new();

    [Required]
    public string Question { get; set; } = string.Empty;

    [Required]
    public string Answer { get; set; } = string.Empty;

    public bool HasErrors { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public string ErrorSummaryHeading { get; set; } = string.Empty;
}