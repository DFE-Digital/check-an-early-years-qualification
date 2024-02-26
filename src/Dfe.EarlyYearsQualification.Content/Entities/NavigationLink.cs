namespace Dfe.EarlyYearsQualification.Content.Entities;

public class NavigationLink
{
    /// <summary>
    /// Display text (i.e. <a>{DisplayText}</a>)
    /// </summary>
    public string DisplayText { get; set; } = null!;

    /// <summary>
    /// Href value (i.e. <a href="{Href}"></a>)
    /// </summary>
    public string Href { get; set; } = null!;

    /// <summary>
    /// Should this link open in a new tab?
    /// </summary>
    public bool OpenInNewTab { get; set; } = false;
}