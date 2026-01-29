using Dfe.EarlyYearsQualification.Content.Entities;
namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationWebViewModel : BasicQualificationModel
{
    public int StaffChildRatio { get; init; }

    public string? FromWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string? AdditionalRequirements { get; init; }
}