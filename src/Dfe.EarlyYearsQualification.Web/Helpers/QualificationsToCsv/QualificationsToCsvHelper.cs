using System.Globalization;
using Azure.Identity;
using Azure.Storage.Blobs;
using CsvHelper;
using Dfe.EarlyYearsQualification.Content.Services;

namespace Dfe.EarlyYearsQualification.Web.Helpers.QualificationsToCsv;

public class QualificationsToCsvHelper(ILogger<QualificationsToCsvHelper> logger, IContentService contentService, IConfiguration configuration) : IQualificationsToCsvHelper
{
    public async Task GetQualificationsAndGenerate()
    {
        // Get all qualifications and convert them to a csv stream.
        var qualifications = await contentService.GetQualifications();

        using var stream = new MemoryStream();
        
        await using var writer = new StreamWriter(stream);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<QualificationCsvMap>();
        await csv.WriteRecordsAsync(qualifications);

        // For testing locally
        // await using var file = new FileStream("./TestCSV.csv", FileMode.OpenOrCreate);
        // stream.WriteTo(file);

        var blobStorageConnectionString = configuration
                                          .GetSection("Storage")
                                          .GetValue<string>("ConnectionString");
        
        var blobServiceClient = new BlobServiceClient(blobStorageConnectionString);

        var containerClient = blobServiceClient.GetBlobContainerClient("EYQUAL-qualifications");
        
        var blobClient = containerClient.GetBlobClient("TestCSV");
        
        logger.LogInformation("Uploading to Blob storage");
        
        // Upload data from the local file, overwrite the blob if it already exists
        await blobClient.UploadAsync(stream, true);
    }
}