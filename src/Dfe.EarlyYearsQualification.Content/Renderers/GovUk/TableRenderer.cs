using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class TableRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
      var table = content as Table;
      var sb = new StringBuilder();

      sb.Append("<table class='govuk-table'>");
      sb.Append(await TableHelper.Render(table!.Content));
      sb.Append("</table>");

      return sb.ToString();

    }

    public bool SupportsContent(IContent content)
    {
        return content is Table;
    }
}