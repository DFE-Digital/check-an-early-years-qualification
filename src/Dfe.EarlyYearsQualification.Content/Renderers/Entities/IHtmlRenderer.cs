using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

/// <summary>
///     Interface for rendering a Document to HTML
/// </summary>
public interface IHtmlRenderer
{
    /// <summary>
    ///     Returns a string representation of the <paramref name="content" />.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    Task<string> ToHtml(Document? content);
}