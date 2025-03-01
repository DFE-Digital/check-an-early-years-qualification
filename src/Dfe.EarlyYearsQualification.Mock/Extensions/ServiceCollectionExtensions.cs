﻿using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.Mock.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockContentfulServices(this IServiceCollection services)
    {
        services.AddSingleton<IContentService, MockContentfulService>();
        services.AddSingleton<IQualificationsRepository, MockQualificationsRepository>();
        return services;
    }
}