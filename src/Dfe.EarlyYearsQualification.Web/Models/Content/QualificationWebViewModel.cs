using Dfe.EarlyYearsQualification.Content.Entities;
namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationWebViewModel : BasicQualificationModel
{
    public QualificationWebViewModel(Qualification qualification) : base(qualification)
    {
        EyqlTabs = qualification.EyqlTabs;
    }

    public int StaffChildRatio { get; init; }

    public string? FromWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string? AdditionalRequirements { get; init; }

    public string? Notes { get; init; }

    public List<Tab> EyqlTabs { get; init; }
}