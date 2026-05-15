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
    ILogger<ContentfulQualificationDownloadService> logger) : IQualificationDownloadService
{
    private const int Version = 1;
    private const string Locale = "en-GB";
    
    public async Task GenerateEyqlDownload()
    {
        try
        {
            var queryBuilder = QueryBuilder<Qualification>.New.ContentTypeIs(ContentTypes.Qualification).Include(2).Limit(1000);
            var allQualifications = await client.GetEntries(queryBuilder);

            // generate csv
            var content = downloadGenerator.GenerateQualificationListContent(allQualifications.ToList());
            if (string.IsNullOrEmpty(content))
            {
                logger.LogWarning("EYQL not generated. No content found.");
                return;
            }

            // delete the old file and upload new file to Contentful
            await DeletePreviousFile();
            var managementAsset = CreateManagementAsset();
            var uploadedAsset = await contentfulManagementClient.UploadFileAndCreateAsset(managementAsset, Encoding.UTF8.GetBytes(content));
            var currentVersion = uploadedAsset?.SystemProperties.Version ?? Version;
            await contentfulManagementClient.PublishAsset(Assets.EarlyYearsQualificationList, currentVersion + 1);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating EYQL download.");
        }
    }

    public async Task<byte[]> GetEyqlDownloadAsByteArray()
    {
        // var existingAsset = await GetManagementAsset();
        // if (existingAsset != null)
        // {
        //     var url = existingAsset.Files[Locale].Url;
        //     url = "https:" + url;
        //     using var client = new HttpClient();
        //     using (HttpResponseMessage response = await client.GetAsync(url))
        //     {
        //         byte[] fileContents = await response.Content.ReadAsByteArrayAsync();
        //         return fileContents;
        //     }
        // }

        throw new NotImplementedException();
    }
    
    private async Task DeletePreviousFile()
    {
        var existingAsset = await GetManagementAsset();
        if (existingAsset != null)
        {
            if (existingAsset.SystemProperties.FieldStatus.Status.ContainsValue(FieldStatusType.Published))
            {
                // Assets must be unpublished before they can be deleted
                await contentfulManagementClient.UnpublishAsset(Assets.EarlyYearsQualificationList,
                                                                existingAsset.SystemProperties.PublishedVersion ?? Version);
            }
            await contentfulManagementClient.DeleteAsset(Assets.EarlyYearsQualificationList, existingAsset.SystemProperties.Version ?? Version);
        }
    }

    private async Task<ManagementAsset?> GetManagementAsset()
    {
        var assetQueryBuilder = QueryBuilder<ManagementAsset>.New.Limit(100);
        var existingAssets = await contentfulManagementClient.GetAssetsCollection(assetQueryBuilder);
        if (existingAssets is { Items: not null } && existingAssets.Items.Any(x => x.SystemProperties.Id == Assets.EarlyYearsQualificationList))
        {
            return existingAssets.Items.Single(x => x.SystemProperties.Id == Assets.EarlyYearsQualificationList);
        }

        return null;
    }
    
    private static ManagementAsset CreateManagementAsset()
    {
        return new ManagementAsset
               {
                   SystemProperties = new SystemProperties
                                      {
                                          Id = Assets.EarlyYearsQualificationList
                                      },
                   Description = new Dictionary<string, string>
                                 {
                                     { Locale, "The Early Years Qualifications List download." },
                                 },
                   Title = new Dictionary<string, string>
                           {
                               { Locale, "EYQL Download" }
                           },
                   Files = new Dictionary<string, ContentfulFile>
                           {
                               {
                                   Locale, new ContentfulFile
                                           {
                                               ContentType = "text/csv",
                                               FileName = "Early-Years-Qualifications-List.csv"
                                           }
                               }
                           }
               };
    }
}