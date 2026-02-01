using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Helpers;
namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationWebViewModel : BasicQualificationModel
{
    public QualificationWebViewModel(Qualification qualification) : base(qualification)
    {
        FromWhichYear = FormatYearContent(qualification.FromWhichYear);
        ToWhichYear = FormatYearContent(qualification.ToWhichYear);
        AdditionalRequirements = string.IsNullOrEmpty(qualification.AdditionalRequirements) ? "None" : qualification.AdditionalRequirements;
        StaffChildRatio = qualification.StaffChildRatio;
    }

    public int StaffChildRatio { get; init; }

    public string? FromWhichYear { get; init; }

    public string? ToWhichYear { get; init; }

    public string? AdditionalRequirements { get; init; }

    private static string FormatYearContent(string? year)
    {
        var content = "-";

        if (!string.IsNullOrEmpty(year) && year is not "null")
        {
            var convertedFromDate = StringDateHelper.ConvertDate(year);
            if (convertedFromDate.HasValue)
            {
                content = StringDateHelper.ConvertToDateString(convertedFromDate.Value.startMonth, convertedFromDate.Value.startYear, "");
            }
        }

        return content;
    }
}