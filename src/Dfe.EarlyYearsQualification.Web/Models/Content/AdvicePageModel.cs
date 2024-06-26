using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class AdvicePageModel
{
    public string Heading { get; init; } = string.Empty;

    public string BodyContent { get; init; } = string.Empty;
    
    public NavigationLink? BackButton { get; init; }
}