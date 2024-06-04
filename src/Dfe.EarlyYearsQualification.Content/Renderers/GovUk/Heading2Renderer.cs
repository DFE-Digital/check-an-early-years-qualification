using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class Heading2Renderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as Heading2;
        var headingText = heading!.Content[0] as Text;
        return Task.FromResult($"<h2 class='govuk-heading-m'>{headingText!.Value}</h2>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Heading2;
    }
}