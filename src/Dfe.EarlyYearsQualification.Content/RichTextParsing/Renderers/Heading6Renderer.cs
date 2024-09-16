using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class Heading6Renderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as Heading6;
        var headingText = heading!.Content[0] as Text;
        return Task.FromResult($"<h6 class='govuk-heading-m'>{headingText!.Value}</h6>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Heading6;
    }
}