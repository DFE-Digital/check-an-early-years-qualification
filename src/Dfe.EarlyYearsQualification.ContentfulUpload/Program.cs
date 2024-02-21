using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.ContentfulUpload;

public class Program
{
  public const string locale = "en-US";
  public const string SpaceId = "";
  public const string ManagementApiKey = "";

  public static async Task Main(string[] args)
  {
    var client = new ContentfulManagementClient(new HttpClient(), new ContentfulOptions
    {
      ManagementApiKey = ManagementApiKey,
      SpaceId = SpaceId,
      MaxNumberOfRateLimitRetries = 10,
    });

    await SetUpContentModels(client);

    await PopulateContentfulWithQualifications(client);
  }

  private static async Task PopulateContentfulWithQualifications(ContentfulManagementClient client)
  {
    // Check existing entries and identify those to update, and those to create.
    var currentEntries = await client.GetEntriesForLocale(new QueryBuilder<Entry<dynamic>>().ContentTypeIs("Qualification").Limit(1000), locale);

    var qualificationsToAddOrUpdate = GetQualificationsToAddOrUpdate();

    foreach (var qualification in qualificationsToAddOrUpdate)
    {
      var entryToAddOrUpdate = BuildEntryFromQualification(qualification);
      var existingEntry = currentEntries.Where(x => x.SystemProperties.Id == qualification.QualificationId.ToString()).FirstOrDefault();

      if (existingEntry != null)
      {
        // Update existing entry
        entryToAddOrUpdate.SystemProperties.Version = existingEntry!.SystemProperties.Version!.Value;
      }

      // Create new entry
      var entryToPublish = await client.CreateOrUpdateEntry(entryToAddOrUpdate, contentTypeId: "Qualification", version: entryToAddOrUpdate.SystemProperties.Version);

      await client.PublishEntry(entryToPublish.SystemProperties.Id, entryToPublish.SystemProperties.Version!.Value);
    }
  }

  private static async Task SetUpContentModels(ContentfulManagementClient client)
  {
    // Check current version of model
    var currentModels = await client.GetContentTypes();

    var currentModel = currentModels.Where(x => x.SystemProperties.Id == "Qualification").FirstOrDefault();

    var version = currentModel != null && currentModel.SystemProperties.Version != null ? currentModel.SystemProperties.Version!.Value : 1;

    var contentType = new ContentType
    {
      SystemProperties = new SystemProperties
      {
        Id = "Qualification",
      },
      Name = "Qualification",
      Description = "Model for storing all the early years qualifications",
      DisplayField = "qualificationName",
      Fields =
      [
          new Field()
          {
              Name = "Qualification ID",
              Id = "qualificationId",
              Type = "Integer",
          },
          new Field()
          {
              Name = "Qualification Name",
              Id = "qualificationName",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "Qualification Level",
              Id = "qualificationLevel",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "Awarding Organisation Title",
              Id = "awardingOrganisationTitle",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "From Which Year",
              Id = "fromWhichYear",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "Qualification Number",
              Id = "qualificationNumber",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "Notes/Additional Requirements",
              Id = "notesAdditionalRequirements",
              Type = "Text",
          },
      ]
    };

    var typeToActivate = await client.CreateOrUpdateContentType(contentType, version: version);
    await client.ActivateContentType("Qualification", version: typeToActivate.SystemProperties.Version!.Value);
    
  }

  private static Entry<dynamic> BuildEntryFromQualification(Qualification qualification)
  {
    var entry = new Entry<dynamic>
    {
      SystemProperties = new SystemProperties
      {
        Id = qualification.QualificationId.ToString(),
        Version = 1
      },

      Fields = new
      {
        qualificationId = new Dictionary<string, int>()
        {
            { locale, qualification.QualificationId}
        },

        qualificationName = new Dictionary<string, string>()
        {
            { locale, qualification.QualificationName }
        },

        awardingOrganisationTitle = new Dictionary<string, string>()
        {
            { locale, qualification.AwardingOrganisationTitle }
        },

        qualificationLevel = new Dictionary<string, string>()
        {
            { locale, qualification.QualificationLevel }
        },

        fromWhichYear = new Dictionary<string, string>()
        {
            { locale, qualification.FromWhichYear ?? ""  }
        },

        qualificationNumber = new Dictionary<string, string>()
        {
            { locale, qualification.QualificationNumber ?? ""  }
        },

        notesAdditionalRequirements = new Dictionary<string, string>()
        {
            { locale, qualification.NotesAdditionalRequirements ?? ""  }
        },
      }
    };

    return entry;
  }

  private static List<Qualification> GetQualificationsToAddOrUpdate()
  {
    var csv = new List<string[]>();
    var lines = System.IO.File.ReadAllLines(@"./csv/ey-quals-full.csv");

    foreach (string line in lines)
      csv.Add(line.Split(','));

    var listObjResult = new List<Qualification>();

    for (int i = 1; i < lines.Length; i++)
    {
      listObjResult.Add(new Qualification(int.Parse(csv[i][0]), csv[i][3], csv[i][4], csv[i][1], csv[i][2], csv[i][5], csv[i][6]));
    }

    return listObjResult;
  }
}


