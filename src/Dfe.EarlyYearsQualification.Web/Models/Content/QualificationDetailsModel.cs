using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel : BasicQualificationModel
{
    public string? FromWhichYear { get; init; }
    
    public string? QualificationNumber { get; init; }

    public NavigationLink? BackButton { get; init; }

    public DetailsPageModel? Content { get; init; }

    public List<AdditionalRequirementQuestionModel>? AdditionalRequirementQuestions { get; set; }

    public RatioRequirementModel RatioRequirements { get; set; } = new ();
}