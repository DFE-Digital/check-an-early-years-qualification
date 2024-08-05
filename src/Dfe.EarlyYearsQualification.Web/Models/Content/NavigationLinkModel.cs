namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class NavigationLinkModel
{
    public string DisplayText { get; init; } = null!;
    
    public string Href { get; init; } = null!;
    
    public bool OpenInNewTab { get; init; }
}