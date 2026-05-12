using System.Text;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ContentfulFile = Contentful.Core.Models.File;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulQualificationDownloadService(
    IContentfulManagementClient contentfulManagementClient,
    IDownloadGenerator downloadGenerator,
    ILogger<ContentfulQualificationDownloadService> logger) : IQualificationDownloadService
{
    public async Task GenerateEyqlDownload()
    {
        // To avoid the redis cache use the contentfulManagementClient to get the entries to ensure we have the most up-to-date qualifications
        var queryBuilder = QueryBuilder<Qualification>.New.ContentTypeIs(ContentTypes.Qualification).Include(2).Limit(1000);
        var allQualifications = await contentfulManagementClient.GetEntriesCollection(queryBuilder);

        // generate csv
        var content = downloadGenerator.GenerateQualificationListContent(allQualifications.ToList());
        
        if (string.IsNullOrEmpty(content))
        {
            logger.LogWarning("EYQL not generated. No content found.");
            return;
        }

        // upload file to Contentful
        var managementAsset = new ManagementAsset();

        managementAsset.SystemProperties = new SystemProperties();
        managementAsset.SystemProperties.Id = Assets.EarlyYearsQualificationList;
        managementAsset.Title = new Dictionary<string, string>
                                {
                                    { "en-GB", "EYQL Download" }
                                };
        managementAsset.Files = new Dictionary<string, ContentfulFile>
                                {
                                    {
                                        "en-GB", new ContentfulFile
                                                 {
                                                     ContentType = "text/csv",
                                                     FileName = "Early-Years-Qualifications-List.csv"
                                                 }
                                    }
                                };

        await contentfulManagementClient.UploadFileAndCreateAsset(managementAsset, Encoding.UTF8.GetBytes(content));
    }

    public async Task<string> GetEyqlDownloadLink()
    {
        throw new NotImplementedException();
    }
}