using System.Net;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public static class ServiceCollectionExtensions
{
    // "ContentfulClient" is set in Contentful.AspNetCore.IServiceCollectionExtensions.HttpClientName
    private const string ContentfulClientHttpClientName = "ContentfulClient";

    public static IServiceCollection AddContentful(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddOptions();
        services.Configure<ContentfulOptions>(configuration.GetSection("ContentfulOptions"));

        services.AddScoped<HttpClientHandler>(_ => new HttpClientHandler());

        services.TryAddSingleton<HttpClient>();

        services.AddHttpClient(ContentfulClientHttpClientName)
                .ConfigurePrimaryHttpMessageHandler(ConfigureHttpMessageHandler);

        services.TryAddScoped(ContentfulClientFactory);

        services.AddTransient<HtmlRenderer>();

        SetupContentOptionsManagement(services, configuration);

        return services;
    }

    private static IContentfulClient ContentfulClientFactory(IServiceProvider sp)
    {
        var contentOptionsManager = sp.GetService<IContentOptionsManager>();

        var contentOption = contentOptionsManager!.GetContentOption().Result;

        var options = sp.GetService<IOptions<ContentfulOptions>>()!.Value;
        if (contentOption == ContentOption.UsePreview)
        {
            options.UsePreviewApi = true;
        }

        var factory = sp.GetService<IHttpClientFactory>();

        return new ContentfulClient(factory!.CreateClient(ContentfulClientHttpClientName), options);
    }

    /// <summary>
    ///     Sets up the Contentful services for <see cref="IContentService" /> and <see cref="IQualificationsRepository" />.
    ///     Also sets up the HttpMessageHandler for the HttpClient used by Contentful to cache use the distributed
    ///     cache for all responses received from Contentful.
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void SetupContentfulServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDateValidator, DateValidator>();
        serviceCollection.AddScoped<IQualificationListFilter, QualificationListFilter>();
        serviceCollection.AddScoped<IContentService, ContentfulContentService>();
        serviceCollection.AddScoped<IQualificationsRepository, QualificationsRepository>();
    }
    
    private static HttpMessageHandler ConfigureHttpMessageHandler(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetService<IOptions<ContentfulOptions>>()!.Value;

        var httpHandler = serviceProvider.GetService<HttpClientHandler>()!;

        if (options.AllowHttpResponseCompression && httpHandler.SupportsAutomaticDecompression)
        {
            httpHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }

        var cache = serviceProvider.GetService<IDistributedCache>()!;
        var urlToKeyConverter = serviceProvider.GetService<IUrlToKeyConverter>()!;
        var cachingOptionsManager = serviceProvider.GetService<ICachingOptionsManager>()!;
        var logger = serviceProvider.GetService<ILogger<CachingHandler>>()!;

        return new CachingHandler(cache,
                                  urlToKeyConverter,
                                  cachingOptionsManager,
                                  logger,
                                  httpHandler);
    }

    private static void SetupContentOptionsManagement(IServiceCollection services, IConfiguration? configuration)
    {
        if (new EnvironmentService(configuration).IsProduction())
        {
            services.AddSingleton<IContentOptionsManager, UsePublishedContentOptionsManager>();
        }
        else
        {
            services.AddScoped<IContentOptionsManager, ContentOptionsManager>();
        }
    }
}