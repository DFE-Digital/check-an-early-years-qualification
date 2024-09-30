using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class InsetTextRenderer : IContentRenderer
{
    public int Order { get; set; }

    public bool SupportsContent(IContent content)
    {
        if (content is not EntryStructure entryStructure)
        {
            return false;
        }

        return entryStructure.Data?.Target is GovUkInsetTextModel;
    }

    public async Task<string> RenderAsync(IContent content)
    {
        var model = (content as EntryStructure)!.Data.Target as GovUkInsetTextModel;

        var sb = new StringBuilder();

        if (model!.Content == null || model.Content.Content.Count <= 0) return sb.ToString();
        
        sb.Append("<div class=\"govuk-inset-text\">");

        sb.Append(await NestedContentHelper.Render(model.Content!.Content));

        sb.Append("</div>");

        return sb.ToString();
    }
}