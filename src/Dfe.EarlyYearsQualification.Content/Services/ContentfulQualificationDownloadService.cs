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
            await contentfulManagementClient.DeleteAsset(Assets.EarlyYearsQualificationList, Version);
            var managementAsset = CreateManagementAsset();
            await contentfulManagementClient.UploadFileAndCreateAsset(managementAsset, Encoding.UTF8.GetBytes(content));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error generating EYQL download.");
        }
    }

    public async Task<string> GetEyqlDownloadLink()
    {
        throw new NotImplementedException();
    }
    
    private static ManagementAsset CreateManagementAsset()
    {
        return new ManagementAsset
               {
                   SystemProperties = new SystemProperties
                                      {
                                          Id = Assets.EarlyYearsQualificationList,
                                          Version = Version
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