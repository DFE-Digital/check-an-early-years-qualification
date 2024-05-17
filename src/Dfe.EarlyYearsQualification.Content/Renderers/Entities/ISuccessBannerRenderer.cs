using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

/// <summary>
///     Interface for rendering a GovUK success banner from a contentful Document field
/// </summary>
public interface ISuccessBannerRenderer
{
    /// <summary>
    ///     Returns a string representation of the <paramref name="content" />.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    Task<string> ToHtml(Document? content);
}