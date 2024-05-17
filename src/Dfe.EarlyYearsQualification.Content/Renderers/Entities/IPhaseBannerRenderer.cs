using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

public interface IPhaseBannerRenderer
{
    Task<string> ToHtml(Document? content);
}