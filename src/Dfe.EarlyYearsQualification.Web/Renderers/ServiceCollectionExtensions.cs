using Dfe.EarlyYearsQualification.Content.Renderers.Entities;

namespace Dfe.EarlyYearsQualification.Web.Renderers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContentfulRenderers(this IServiceCollection services)
    {
        services.AddSingleton<IHtmlRenderer, HtmlRenderer>();

        return services;
    }
}