using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

public interface IHtmlRenderer
{
    Task<string> ToHtml(Document? content);
}