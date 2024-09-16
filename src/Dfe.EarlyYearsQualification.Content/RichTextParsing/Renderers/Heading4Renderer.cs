using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class Heading4Renderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as Heading4;
        var headingText = heading!.Content[0] as Text;
        return Task.FromResult($"<h4 class='govuk-heading-s'>{headingText!.Value}</h4>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Heading4;
    }
}