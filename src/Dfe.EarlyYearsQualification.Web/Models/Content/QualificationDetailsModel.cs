using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel : BasicQualificationModel
{
    public string? FromWhichYear { get; init; }
    
    public string? QualificationNumber { get; init; }

    public NavigationLinkModel? BackButton { get; init; }

    public DetailsPageModel? Content { get; init; }

    public List<AdditionalRequirementQuestionModel>? AdditionalRequirementQuestions { get; init; }

    public RatioRequirementModel RatioRequirements { get; set; } = new ();
}