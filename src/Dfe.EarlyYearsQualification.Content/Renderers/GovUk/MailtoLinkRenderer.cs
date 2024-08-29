using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

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
            var navigationLinkModel = model.JObject.ToObject<MailtoLink>();
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

        var mailtoLinkModel = model!.JObject.ToObject<MailtoLink>()!;
        
        return await Task.FromResult($"<a href='mailto:{mailtoLinkModel.Email}'>{mailtoLinkModel.Text}</a>");
    }
}