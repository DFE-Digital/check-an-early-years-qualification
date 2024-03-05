using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.VisualBasic.FileIO;

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
              Type = "Symbol",
              Required = true,
              Validations = new List<IFieldValidator>() {
                  new UniqueValidator()
              }
          },
          new Field()
          {
              Name = "Qualification Name",
              Id = "qualificationName",
              Type = "Symbol",
              Required = true,
          },
          new Field()
          {
              Name = "Qualification Level",
              Id = "qualificationLevel",
              Type = "Symbol",
              Required = true,
          },
          new Field()
          {
              Name = "Awarding Organisation Title",
              Id = "awardingOrganisationTitle",
              Type = "Symbol",
              Required = true,
          },
          new Field()
          {
              Name = "From Which Year",
              Id = "fromWhichYear",
              Type = "Symbol",
          },
          new Field()
          {
              Name = "To Which Year",
              Id = "toWhichYear",
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
              Name = "Notes",
              Id = "notes",
              Type = "Text",
          },
          new Field()
          {
              Name = "Additional Requirements",
              Id = "additionalRequirements",
              Type = "Text",
          },
      ]
        };

        var typeToActivate = await client.CreateOrUpdateContentType(contentType, version: version);
        await client.ActivateContentType("Qualification", version: typeToActivate.SystemProperties.Version!.Value);

        Thread.Sleep(2000); // Allows the API time to activate the content type
        await SetHelpText(client, typeToActivate);
    }

    private static async Task SetHelpText(ContentfulManagementClient client, ContentType typeToActivate)
    {
        var editorInterface = await client.GetEditorInterface("Qualification");
        SetHelpTextForField(editorInterface, "qualificationId", "The unique identifier used to reference the qualification");
        SetHelpTextForField(editorInterface, "qualificationName", "The name of the qualification");
        SetHelpTextForField(editorInterface, "qualificationLevel", "The level of the qualification");
        SetHelpTextForField(editorInterface, "awardingOrganisationTitle", "The name of the awarding organisation");
        SetHelpTextForField(editorInterface, "fromWhichYear", "The date which the qualification was marked approved");
        SetHelpTextForField(editorInterface, "toWhichYear", "The date which the qualification was marked as disapproved");
        SetHelpTextForField(editorInterface, "qualificationNumber", "The number of the qualification");
        SetHelpTextForField(editorInterface, "notes", "The corresponding notes for the qualification");
        SetHelpTextForField(editorInterface, "additionalRequirements", "The additional requirements for the qualification");

        await client.UpdateEditorInterface(editorInterface, "Qualification", typeToActivate.SystemProperties.Version!.Value);
    }

    private static void SetHelpTextForField(EditorInterface editorInterface, string fieldId, string helpText)
    {
        editorInterface.Controls.First(x => x.FieldId == fieldId).Settings = new EditorInterfaceControlSettings() { HelpText = helpText };
        editorInterface.Controls.First(x => x.FieldId == fieldId).WidgetId = SystemWidgetIds.SingleLine;
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
        qualificationId = new Dictionary<string, string>()
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

        toWhichYear = new Dictionary<string, string>()
        {
            { locale, qualification.ToWhichYear ?? ""  }
        },

        qualificationNumber = new Dictionary<string, string>()
        {
            { locale, qualification.QualificationNumber ?? ""  }
        },

        notes = new Dictionary<string, string>()
        {
            { locale, qualification.Notes ?? ""  }
        },

        additionalRequirements = new Dictionary<string, string>()
        {
            { locale, qualification.AdditionalRequirements ?? ""  }
        },
      }
    };

    return entry;
  }

  private static List<Qualification> GetQualificationsToAddOrUpdate()
  {
    // var csv = new List<string[]>();
    // var lines = System.IO.File.ReadAllLines(@"./csv/ey-quals-full.csv");

    // foreach (string line in lines)
    //   csv.Add(line.Split(','));
    var lines = ReadCsvFile(@"./csv/ey-quals-full-updated.csv");

    var listObjResult = new List<Qualification>();

    for (int i = 0; i < lines.Count; i++)
    {
      var qualificationId = lines[i][0];
      var qualificationName = lines[i][4];
      var awardingOrganisationTitle = lines[i][5];
      var qualificationLevel = lines[i][1];
      var fromWhichYear = lines[i][2];
      var toWhichYear = lines[i][3];
      var qualificationNumber = lines[i][6];
      var notes = lines[i][7];
      var additionalRequirements = lines[i][8];

      listObjResult.Add(new Qualification(
        qualificationId,
        qualificationName,
        awardingOrganisationTitle,
        qualificationLevel,
        fromWhichYear,
        toWhichYear,
        qualificationNumber,
        notes,
        additionalRequirements
      ));
    }

    return listObjResult;
  }

  private static List<string[]> ReadCsvFile(string filePath)
  {
    var result = new List<string[]>();
    using (TextFieldParser csvParser = new TextFieldParser(filePath))
    {
      csvParser.SetDelimiters(new string[] { "," });
      csvParser.HasFieldsEnclosedInQuotes = true;

      // Skip the row with the column names
      csvParser.ReadLine();

      while (!csvParser.EndOfData)
      {
        // Read current line fields, pointer moves to the next line.
        string[] fields = csvParser.ReadFields();
        result.Add(fields);
      }
    }
    return result;
  }
}


