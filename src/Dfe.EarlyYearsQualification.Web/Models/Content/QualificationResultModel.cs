namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationResultModel
{
    public string Heading { get; init; } = string.Empty;

    public string MessageHeading { get; init; } = string.Empty;

    public string MessageBody { get; init; } = string.Empty;

    public bool IsFullAndRelevant { get; init; }
}