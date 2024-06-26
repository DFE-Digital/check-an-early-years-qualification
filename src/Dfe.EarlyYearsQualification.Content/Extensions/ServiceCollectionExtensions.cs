using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.Content.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModelRenderers(this IServiceCollection services)
    {
        services.AddSingleton<IHtmlRenderer, HtmlModelRenderer>()
                .AddSingleton<IPhaseBannerRenderer, PhaseBannerRenderer>()
                .AddSingleton<ISideContentRenderer, SideContentRenderer>()
                .AddSingleton<IHtmlTableRenderer, HtmlTableRenderer>()
                .AddSingleton<ISuccessBannerRenderer, SuccessBannerRenderer>()
                .AddSingleton<IGovUkInsetTextRenderer, GovUkInsetTextRenderer>();

        return services;
    }
}