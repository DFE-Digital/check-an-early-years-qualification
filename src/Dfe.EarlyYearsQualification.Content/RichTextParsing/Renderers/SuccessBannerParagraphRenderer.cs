using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class SuccessBannerParagraphRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var paragraph = content as Paragraph;
        var text = paragraph!.Content[0] as Text;
        return Task.FromResult(text!.Value);
    }

    public bool SupportsContent(IContent content)
    {
        return content is Paragraph;
    }
}