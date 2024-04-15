using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class GovUkInsetTextRenderer : IContentRenderer
{
  public int Order { get; set; }

  private readonly IContentfulClient _contentfulClient;

  public GovUkInsetTextRenderer(IContentfulClient contentfulClient)
  {
    _contentfulClient = contentfulClient;
  }

  public bool SupportsContent(IContent content)
  {
    var model = (content as EntryStructure)!.Data.Target as CustomNode;

    try
    {
      return model!.JObject.SelectToken("sys.contentType.sys.id")!.ToString().Contains("govUkInsetText");
    }
    catch
    {
      return false;
    }
  }

  public async Task<string> RenderAsync(IContent content)
  {
    var model = (content as EntryStructure)!.Data.Target as CustomNode;

    // To simplify, we just grab the contentful ID from the content and recall to contentful to get a nice object back
    var contentfulId = model!.JObject.Values().First(x => x.Path == "$id").ToString();
    var insetTextModel = await _contentfulClient.GetEntry<GovUkInsetTextModel>(contentfulId);

    var sb = new StringBuilder();

    sb.Append("<div class=\"govuk-inset-text\">");

    sb.Append(await NestedContentHelper.Render(insetTextModel!.Content!.Content));

    sb.Append("</div>");

    return sb.ToString();
  }
}