namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel : BasicQualificationModel
{
    public string? FromWhichYear { get; init; }
    
    public string? QualificationNumber { get; init; }

    public NavigationLinkModel? BackButton { get; init; }

    public DetailsPageModel? Content { get; init; }

    public List<AdditionalRequirementAnswerModel>? AdditionalRequirementAnswers { get; init; }

    public RatioRequirementModel RatioRequirements { get; set; } = new ();

    public string DateStarted { get; init; } = string.Empty;
    public string DateAwarded { get; init; } = string.Empty;
}