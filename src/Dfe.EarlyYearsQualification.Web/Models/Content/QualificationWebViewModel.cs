using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Helpers;
namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationWebViewModel : BasicQualificationModel
{
    private const string MissingValue = "-";

    public QualificationWebViewModel(Qualification qualification) : base(qualification)
    {
        FromWhichYear = FormatYearContent(qualification.FromWhichYear);
        ToWhichYear = FormatYearContent(qualification.ToWhichYear);
        AdditionalRequirements = string.IsNullOrEmpty(qualification.AdditionalRequirements) ? "None" : qualification.AdditionalRequirements;
        StaffChildRatio = qualification.StaffChildRatio;
        QualificationNumber = string.IsNullOrEmpty(QualificationNumber) ? MissingValue : QualificationNumber;
        EyqlTabs = qualification.EyqlTabs;
    }

    public int StaffChildRatio { get; init; }

    public string? FromWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string? AdditionalRequirements { get; init; }

    public List<Tab> EyqlTabs { get; init; }

    private static string FormatYearContent(string? year)
    {
        if (string.IsNullOrEmpty(year) || year == "null")
        {
            return MissingValue;
        }

        var converted = StringDateHelper.ConvertDate(year);
        if (converted.HasValue)
        {
            return StringDateHelper.ConvertToDateString(converted.Value.startMonth, converted.Value.startYear);
        }

        return year;
    }
}