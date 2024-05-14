using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.Content.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModelRenderers(this IServiceCollection services)
    {
        services.AddSingleton<IHtmlRenderer, HtmlModelRenderer>();

        return services;
    }
}