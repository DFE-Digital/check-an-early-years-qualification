namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class StaticPageModel
{
    public string Heading { get; init; } = string.Empty;

    public string BodyContent { get; init; } = string.Empty;

    public NavigationLinkModel? BackButton { get; init; }

    public OpenGraphDataModel? OpenGraphData { get; init; }
}