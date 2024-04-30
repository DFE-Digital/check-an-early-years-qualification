using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class TableHeadingRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as TableHeader;
        var headingParagraph = heading!.Content[0] as Paragraph;
        var headingText = headingParagraph!.Content[0] as Text;
        return Task.FromResult("<th class='govuk-table__header'>" + headingText!.Value + "</th>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is TableHeader;
    }
}