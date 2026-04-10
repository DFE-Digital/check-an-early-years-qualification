using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;
using Dfe.EarlyYearsQualification.Mock.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.Mock.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockContentfulServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateValidator, DateValidator>();
        services.AddSingleton<IQualificationListFilter, QualificationListFilter>();
        services.AddSingleton<IContentService, MockContentfulService>();
        services.AddSingleton<IQualificationsRepository, MockQualificationsRepository>();
        return services;
    }
}