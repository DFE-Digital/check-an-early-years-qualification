using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

public interface ISideContentRenderer
{
    Task<string> ToHtml(Document? content);
}