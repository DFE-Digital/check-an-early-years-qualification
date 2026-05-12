namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class SearchResultContentModel
{
    public required BasicQualificationModel Qualification { get; init; }

    public string? SearchResultContents { get; init; }
}