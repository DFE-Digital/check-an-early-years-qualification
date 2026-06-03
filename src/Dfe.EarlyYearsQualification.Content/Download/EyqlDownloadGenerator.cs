using System.Text;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Download;

// ReSharper disable once IdentifierTypo
public class EyqlDownloadGenerator : IDownloadGenerator
{
    public string GenerateQualificationListContent(List<Qualification> qualifications)
    {
        if (qualifications.Count == 0) return string.Empty;
        
        var orderedQualifications = new List<Qualification>();
        foreach (var qualification in qualifications)
        {
            orderedQualifications.AddRange(qualification.EyqlTabs.Select(tab => new Qualification(
                                                                                  qualification.QualificationId,
                                                                                  qualification.QualificationName,
                                                                                  qualification
                                                                                      .AwardingOrganisationTitle,
                                                                                  qualification.QualificationLevel)
                                                                             {
                                                                                 EyqlTabs = [tab],
                                                                                 StaffChildRatio =
                                                                                     qualification.StaffChildRatio,
                                                                                 FromWhichYear =
                                                                                     qualification.FromWhichYear,
                                                                                 ToWhichYear =
                                                                                     qualification.ToWhichYear,
                                                                                 QualificationNumber =
                                                                                     qualification
                                                                                         .QualificationNumber,
                                                                                 AdditionalRequirements =
                                                                                     qualification
                                                                                         .AdditionalRequirements
                                                                             }));
        }
        
        orderedQualifications = orderedQualifications.OrderBy(x => x.EyqlTabs[0].Order)
                                                     .ThenBy(x => x.QualificationLevel)
                                                     .ThenBy(x => x.QualificationName)
                                                     .ToList();
        const string headers =
            "Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements";
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(headers);
        foreach (var qualification in orderedQualifications)
        {
            var qualificationData =
                $"{qualification.EyqlTabs[0].Heading},{qualification.QualificationLevel},{qualification.StaffChildRatio},{qualification.FromWhichYear},{qualification.ToWhichYear},{EscapeCsvValue(qualification.QualificationName)},{EscapeCsvValue(qualification.AwardingOrganisationTitle)},{qualification.QualificationNumber},{EscapeCsvValue(qualification.AdditionalRequirements)}";
            stringBuilder.AppendLine(qualificationData);
        }

        // Remove empty last line
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        
        return stringBuilder.ToString();
    }
    
    private static string EscapeCsvValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\""; // Return empty string escaped
        // Check if value contains special characters
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            // Escape the double quotes by replacing them with two double quotes
            value = value.Replace("\"", "\"\"");
            // Enclose the entire value in quotes
            return $"\"{value}\"";
        }
        return value; // Return value without escaping
    }
}