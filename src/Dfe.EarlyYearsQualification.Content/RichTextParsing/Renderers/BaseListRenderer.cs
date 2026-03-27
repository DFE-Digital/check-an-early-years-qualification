using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;
using System.Text;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public abstract class BaseListRenderer
{
    protected static async Task<string> RenderAsync(IContent content, string rendererType)
    {
        var list = content as List;

        if (list!.Content.Count == 0)
        {
            return await Task.FromResult(string.Empty);
        }

        var listType = rendererType == ListRendererType.OrderedList ? "number" : "bullet";

        var sb = new StringBuilder();
        sb.Append($"<{rendererType} class=\"govuk-list govuk-list--{listType}\">");

        foreach (var contentValue in list.Content)
        {
            if (contentValue is ListItem listItem)
            {
                var listItemParagraph = listItem.Content[0] as Paragraph;
                sb.Append($"<li>{await NestedContentHelper.Render(listItemParagraph!.Content)}");

                if (listItem.Content.Count > 1)
                {
                    // Skip the paragraph content as it has already been rendered and render the rest of the content which will be the nested list
                    sb.Append($"{await NestedContentHelper.Render([.. listItem!.Content.Skip(1)])}");
                }

                sb.Append($"</li>");
            }
        }

        sb.Append($"</{rendererType}>");
        return await Task.FromResult(sb.ToString());
    }
}

public static class ListRendererType
{
    public const string OrderedList = "ol";
    public const string UnorderedList = "ul";
}