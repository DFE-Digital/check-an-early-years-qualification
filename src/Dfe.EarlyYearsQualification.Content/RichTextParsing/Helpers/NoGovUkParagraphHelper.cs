using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

public static class NoGovUkParagraphHelper
{
    public static async Task<string> Render(IContent content)
    {
        var paragraph = content as Paragraph;

        var sb = new StringBuilder();

        var externalLinkRenderer = new NavigationLinkRenderer();

        foreach (var item in paragraph!.Content)
        {
            switch (item)
            {
                case Hyperlink hl:
                    var hyperlinkRenderer = new HyperlinkRenderer();
                    var hyperlinkText = await hyperlinkRenderer.RenderAsync(hl);
                    sb.Append(hyperlinkText);
                    continue;

                case Text t:
                    sb.Append(t.Value);
                    continue;
            }

            if (externalLinkRenderer.SupportsContent(item))
            {
                sb.Append(await externalLinkRenderer.RenderAsync(item));
            }
        }

        return sb.ToString();
    }
}