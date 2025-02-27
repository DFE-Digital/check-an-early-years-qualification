using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AssetJsonConverter = Dfe.EarlyYearsQualification.Content.Converters.AssetJsonConverter;

namespace Dfe.EarlyYearsQualification.Content.Caching;

public static class ServiceProviderExtensions
{
    public static void SetupCacheSerialization(this IServiceProvider serviceProvider)
    {
        var contentfulClient = serviceProvider.GetRequiredService<IContentfulClient>();

        contentfulClient.SerializerSettings.Converters.Clear();

        contentfulClient.SerializerSettings.Converters
                        .Add(new AssetJsonConverter()); // NB: this isn't the Contentful one 

        contentfulClient.SerializerSettings.Converters
                        .Add(new ContentJsonConverter()); // NB: this _is_ the Contentful one

        contentfulClient.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        contentfulClient.SerializerSettings.Formatting = Formatting.Indented;
        contentfulClient.SerializerSettings.ContractResolver = new DefaultContractResolver
                                                               {
                                                                   NamingStrategy = new CamelCaseNamingStrategy()
                                                               };

        DistributedCacheExtensions.ContentfulSerializer = contentfulClient.Serializer;
        DistributedCacheExtensions.ContentfulSerializerSettings = contentfulClient.SerializerSettings;
    }
}