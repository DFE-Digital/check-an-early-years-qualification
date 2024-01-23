using System.Text;
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class ParagraphRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var paragraph = content as Paragraph;
        var listItemText = paragraph!.Content[0] as Text;
        return Task.FromResult("<p class='govuk-body'>" + listItemText!.Value + "</p>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Paragraph;
    }
}
