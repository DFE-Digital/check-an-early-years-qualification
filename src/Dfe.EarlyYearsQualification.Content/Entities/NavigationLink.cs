using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class NavigationLink
{
    /// <summary>
    /// Contentful system properties
    /// </summary>
    public SystemProperties Sys { get; init; } = new();
    
    /// <summary>
    ///     Display text (i.e. <a>{DisplayText}</a>)
    /// </summary>
    public string DisplayText { get; init; } = null!;

    /// <summary>
    ///     Href value (i.e. <a href="{Href}"></a>)
    /// </summary>
    public string Href { get; set; } = null!;

    /// <summary>
    ///     Should this link open in a new tab?
    /// </summary>
    public bool OpenInNewTab { get; init; }
}