using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class MailtoLinkRenderer : IContentRenderer
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
            var navigationLinkModel = (model.JObject as JObject)!.ToObject<MailtoLink>();
            return navigationLinkModel!.Sys.ContentType.SystemProperties.Id == "mailtoLink";
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> RenderAsync(IContent content)
    {
        var model = (content as EntryStructure)!.Data.Target as CustomNode;

        var mailtoLinkModel = (model!.JObject as JObject)!.ToObject<MailtoLink>()!;
        
        return await Task.FromResult($"<a class='govuk-link' href='mailto:{mailtoLinkModel.Email}'>{mailtoLinkModel.Text}</a>");
    }
}