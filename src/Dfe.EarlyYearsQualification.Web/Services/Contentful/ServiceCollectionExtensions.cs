using Dfe.EarlyYearsQualification.Content.Caching;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public static class ServiceCollectionExtensions
{
    public static void SetupContentfulServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IContentService, ContentfulContentService>();
        serviceCollection.AddTransient<IQualificationsRepository, QualificationsRepository>();

        // "ContentfulClient" is the value of Contentful.AspNetCore.IServiceCollectionExtensions.HttpClientName
        serviceCollection.AddHttpClient("ContentfulClient")
                         .ConfigurePrimaryHttpMessageHandler<CachingHandler>();
    }
}