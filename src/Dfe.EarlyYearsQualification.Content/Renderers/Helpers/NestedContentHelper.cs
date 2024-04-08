using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers;

public static class NestedContentHelper
{

  public static async Task<string> Render(List<IContent> content)
  {
    var sb = new StringBuilder();

    foreach (var item in content)
    {

      switch (item)
      {
        case Paragraph p:
          var paragraphRenderer = new GovUkParagraphRenderer();
          var paragraphText = await paragraphRenderer.RenderAsync(p);
          sb.Append(paragraphText);
          continue;

        case Hyperlink hl:
          var hyperlinkRenderer = new HyperlinkRenderer();
          var hyperlinkText = await hyperlinkRenderer.RenderAsync(hl);
          sb.Append(hyperlinkText);
          continue;

        case Text t:
          sb.Append(t.Value);
          continue;
      }
    }

    return sb.ToString();
  }
}

