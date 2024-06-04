using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class Heading3Renderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as Heading3;
        var headingText = heading!.Content[0] as Text;
        return Task.FromResult($"<h3 class='govuk-heading-m'>{headingText!.Value}</h3>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Heading3;
    }
}