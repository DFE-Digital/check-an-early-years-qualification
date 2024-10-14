using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class EmbeddedParagraphRenderer : IContentRenderer
{
    public int Order { get; set; }
    public bool SupportsContent(IContent content)
    {
        if (content is not EntryStructure entryStructure)
        {
            return false;
        }

        return entryStructure.Data?.Target is EmbeddedParagraph;
    }

    public async Task<string> RenderAsync(IContent content)
    {
        var model = (content as EntryStructure)!.Data.Target as EmbeddedParagraph;

        return await NestedContentHelper.Render(model!.Content!.Content);
    }
}