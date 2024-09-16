using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class InsetTextRenderer(IContentfulClient contentfulClient) : IContentRenderer
{
    public int Order { get; set; }

    public bool SupportsContent(IContent content)
    {
        if (content is not EntryStructure entryStructure)
        {
            return false;
        }

        if (entryStructure.Data?.Target is not CustomNode model)
        {
            return false;
        }

        try
        {
            var insetTextModel = model.JObject.ToObject<GovUkInsetTextModel>();
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
        var doc = JsonConvert.DeserializeObject<Document>(documentObject, contentfulClient.SerializerSettings);

        var sb = new StringBuilder();

        sb.Append("<div class=\"govuk-inset-text\">");

        sb.Append(await NestedContentHelper.Render(doc!.Content));

        sb.Append("</div>");

        return sb.ToString();
    }
}