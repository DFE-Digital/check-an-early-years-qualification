using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.Mock.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockContentfulService(this IServiceCollection services)
    {
        services.AddSingleton<IContentService, MockContentfulService>();
        return services;
    }
}
