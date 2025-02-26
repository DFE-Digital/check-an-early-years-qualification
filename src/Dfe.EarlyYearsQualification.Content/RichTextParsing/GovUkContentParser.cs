using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing;

/// <summary>
///     A class to turn a contentful rich text field (Document object) into a GovUk styled HTML string
/// </summary>
public class GovUkContentParser : IGovUkContentParser
{
    private readonly HtmlRenderer _renderer = new();

    public GovUkContentParser(IEnumerable<IContentRenderer> renderers)
    {
        foreach (var renderer in renderers)
        {
            _renderer.AddRenderer(renderer);
        }
    }

    /// <summary>
    ///     Returns a string representation (HTML) of the <paramref name="content" />
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<string> ToHtml(Document? content)
    {
        if (content is null)
        {
            return string.Empty;
        }

        return await _renderer.ToHtml(content);
    }
}