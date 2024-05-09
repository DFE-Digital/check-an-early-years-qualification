using System.Text;
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class PhaseBannerRenderer : IContentRenderer
{
  public int Order { get; set; }

  public async Task<string> RenderAsync(IContent content)
  {
    var paragraph = content as Paragraph;

    var sb = new StringBuilder();

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
    }

    return sb.ToString();
  }

  public bool SupportsContent(IContent content)
  {
    return content is Paragraph;
  }
}