using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class GovUkParagraphRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        var paragraph = content as Paragraph;

        var sb = new StringBuilder();
        sb.Append("<p class=\"govuk-body\">");

        sb.Append(await NestedContentHelper.Render(paragraph!.Content));

        sb.Append("</p>");
        return sb.ToString();
    }

    public bool SupportsContent(IContent content)
    {
        return content is Paragraph;
    }
}