using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class Heading1Renderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as Heading1;
        var headingText = heading!.Content[0] as Text;
        return Task.FromResult($"<h1 class='govuk-heading-l'>{headingText!.Value}</h1>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Heading1;
    }
}