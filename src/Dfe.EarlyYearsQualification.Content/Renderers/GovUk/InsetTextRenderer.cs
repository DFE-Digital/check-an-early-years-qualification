using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class InsetTextRenderer : IContentRenderer
{
    private readonly IContentfulClient _contentfulClient;

    public InsetTextRenderer(IContentfulClient contentfulClient)
    {
        _contentfulClient = contentfulClient;
    }

    public int Order { get; set; }

    public bool SupportsContent(IContent content)
    {
        var model = (content as EntryStructure)!.Data.Target as CustomNode;

        try
        {
            var insetTextModel = model!.JObject.ToObject<GovUkInsetTextModel>();
            return insetTextModel!.Sys.ContentType.SystemProperties.Id == "govUkInsetText";
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> RenderAsync(IContent content)
    {
        var model = (content as EntryStructure)!.Data.Target as CustomNode;

        var insetTextModel = model!.JObject.ToObject<GovUkInsetTextModel>();
        var documentObject = insetTextModel!.Content!.ToString();
        var doc = JsonConvert.DeserializeObject<Document>(documentObject, _contentfulClient.SerializerSettings);

        var sb = new StringBuilder();

        sb.Append("<div class=\"govuk-inset-text\">");

        sb.Append(await NestedContentHelper.Render(doc!.Content));

        sb.Append("</div>");

        return sb.ToString();
    }
}