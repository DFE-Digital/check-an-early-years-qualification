using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ContentfulFile = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulQualificationDownloadService(
    // To avoid the redis cache use custom implementation without the delegation handler
    [FromKeyedServices(Clients.ContentfulDeliveryClientNoCache)]IContentfulClient client,
    IContentfulManagementClient contentfulManagementClient,
    IDownloadGenerator downloadGenerator,
    ILogger<ContentfulQualificationDownloadService> logger,
    IHttpClientFactory httpClientFactory) : IQualificationDownloadService
{
    private const int Version = 1;
    private const string Locale = "en-GB";
    private const string ProductionFileName = "Early-Years-Qualifications-List.csv";
    private const string StagingFileName = "Early-Years-Qualifications-List-Staging.csv";
    private const string DevelopmentFileName = "Early-Years-Qualifications-List-Development.csv";
    private const string ProductionEnvironment = "production";
    private const string StagingEnvironment = "staging";
    private const string DevelopmentEnvironment = "development";
    
    public async Task GenerateEyqlDownloadByEnvironment(string environment)
    {
        switch (environment.ToLower())
        {
            case ProductionEnvironment:
            await GenerateEyqlDownload(Assets.EarlyYearsQualificationList, "EYQL Download", ProductionFileName);
            break;
            case StagingEnvironment:
            await GenerateEyqlDownload(Assets.EarlyYearsQualificationListStaging, "EYQL Download Staging", StagingFileName);
            break;
            case DevelopmentEnvironment:
            await GenerateEyqlDownload(Assets.EarlyYearsQualificationListDevelopment, "EYQL Download Development", DevelopmentFileName);
            break;
            default:
            logger.LogWarning("Unknown environment: {Environment}. No EYQL download generated.", environment);
            break;
        }
    }

    public async Task<(byte[] fileContents, string fileName)> GetEyqlDownload(string environment)
    {
        switch (environment.ToLower())
        {
            case ProductionEnvironment:
            return (await GetEyqlDownloadAsByteArray(Assets.EarlyYearsQualificationList), ProductionFileName);
            case StagingEnvironment:
            return (await GetEyqlDownloadAsByteArray(Assets.EarlyYearsQualificationListStaging), StagingFileName);
            case DevelopmentEnvironment:
            return (await GetEyqlDownloadAsByteArray(Assets.EarlyYearsQualificationListDevelopment), DevelopmentFileName);
            default:
            logger.LogWarning("Unknown environment: {Environment}. No EYQL asset found.", environment);
            return (Array.Empty<byte>(), string.Empty);
        }
    }

    public async Task<byte[]?> GetEyqlDataForInternalDownload()
    {
        var allQualifications = await GetAllQualifications();

        // generate csv
        var content = downloadGenerator.GenerateInternalQualificationListContent(allQualifications.ToList());
        if (string.IsNullOrEmpty(content))
        {
            logger.LogWarning("No content found for internal download.");
            return null;
        }

        return Encoding.UTF8.GetBytes(content);
    }

    private async Task GenerateEyqlDownload(string assetId, string title, string fileName)
    {
        try
        {
            var allQualifications = await GetAllQualifications();

            // generate csv
            var content = downloadGenerator.GenerateQualificationListContent(allQualifications.ToList());
            if (string.IsNullOrEmpty(content))
            {
                logger.LogWarning("EYQL not generated. No content found.");
                return;
            }

            // delete the old file and upload new file to Contentful
            await DeletePreviousFile(assetId);
            var managementAsset = CreateManagementAsset(assetId, title, fileName);
            var uploadedAsset = await contentfulManagementClient.UploadFileAndCreateAsset(managementAsset, Encoding.UTF8.GetBytes(content));
            var currentVersion = uploadedAsset?.SystemProperties.Version ?? Version;
            // Allows for the Contentful background process to complete
            Thread.Sleep(4000);
            await contentfulManagementClient.PublishAsset(assetId, currentVersion + 1);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating EYQL download.");
        }
    }

    private async Task<ContentfulCollection<Qualification>> GetAllQualifications()
    {
        var queryBuilder = QueryBuilder<Qualification>.New.ContentTypeIs(ContentTypes.Qualification).Include(2).Limit(1000);
        var allQualifications = await client.GetEntries(queryBuilder);
        return allQualifications;
    }

    private async Task<byte[]> GetEyqlDownloadAsByteArray(string assetId)
    {
        var existingAsset = await GetManagementAsset(assetId);
        if (existingAsset == null)
        {
            logger.LogWarning("EYQL not found.");
            return [];
        }
        
        var url = existingAsset.Files[Locale].Url;
        url = "https:" + url;
        using var httpClient = httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(url);
        var fileContents = await response.Content.ReadAsByteArrayAsync();
        return fileContents;
    }
    
    private async Task DeletePreviousFile(string assetId)
    {
        var existingAsset = await GetManagementAsset(assetId);
        if (existingAsset != null)
        {
            if (existingAsset.SystemProperties.FieldStatus.Status.ContainsValue(FieldStatusType.Published))
            {
                // Assets must be unpublished before they can be deleted
                await contentfulManagementClient.UnpublishAsset(assetId,
                                                                existingAsset.SystemProperties.PublishedVersion ?? Version);
            }
            await contentfulManagementClient.DeleteAsset(assetId, existingAsset.SystemProperties.Version ?? Version);
        }
    }

    private async Task<ManagementAsset?> GetManagementAsset(string assetId)
    {
        var assetQueryBuilder = QueryBuilder<ManagementAsset>.New.Limit(100);
        var existingAssets = await contentfulManagementClient.GetAssetsCollection(assetQueryBuilder);
        if (existingAssets is { Items: not null } && existingAssets.Items.Any(x => x.SystemProperties.Id == assetId))
        {
            return existingAssets.Items.Single(x => x.SystemProperties.Id == assetId);
        }

        return null;
    }

    private static ManagementAsset CreateManagementAsset(string assetId, string title, string fileName)
    {
        return 
            new ManagementAsset
            {
                SystemProperties = new SystemProperties
                                   {
                                       Id = assetId
                                   },
                Description = new Dictionary<string, string>
                              {
                                  { Locale, "The Early Years Qualifications List download." },
                              },
                Title = new Dictionary<string, string>
                        {
                            { Locale, title }
                        },
                Files = new Dictionary<string, ContentfulFile>
                        {
                            {
                                Locale, new ContentfulFile
                                        {
                                            ContentType = "text/csv",
                                            FileName = fileName
                                        }
                            }
                        }
            };
    }
}