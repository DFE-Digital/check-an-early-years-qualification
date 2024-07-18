using System.Diagnostics.CodeAnalysis;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Microsoft.VisualBasic.FileIO;

namespace Dfe.EarlyYearsQualification.ContentUpload;

[ExcludeFromCodeCoverage]
public static class Program
{
    private const string Locale = "en-GB";
    private const string SpaceId = "";
    private const string ManagementApiKey = "";
    private const string CsvFile = "./csv/ey-quals-full-2024-with-new-reference-fields.csv";
    private const string FieldTypeSymbol = "Symbol";

    // ReSharper disable once UnusedParameter.Global
    // ...args standard for Program.Main()
    public static async Task Main(string[] args)
    {
        var client = new ContentfulManagementClient(new HttpClient(),
                                                    new ContentfulOptions
                                                    {
                                                        ManagementApiKey = ManagementApiKey,
                                                        SpaceId = SpaceId,
                                                        MaxNumberOfRateLimitRetries = 10
                                                    });

        await SetUpContentModels(client);

        await PopulateContentfulWithQualifications(client);
    }

    private static async Task PopulateContentfulWithQualifications(ContentfulManagementClient client)
    {
        // Check existing entries and identify those to update, and those to create.
        var queryBuilder = new QueryBuilder<Entry<dynamic>>()
                           .ContentTypeIs(ContentTypes.Qualification)
                           .Limit(1000);

        var currentEntries =
            await client.GetEntriesForLocale(queryBuilder, Locale);

        var qualificationsToAddOrUpdate = GetQualificationsToAddOrUpdate();

        foreach (var qualification in qualificationsToAddOrUpdate)
        {
            var entryToAddOrUpdate = BuildEntryFromQualification(qualification);
            var existingEntry =
                currentEntries.FirstOrDefault(x => x.SystemProperties.Id == qualification.QualificationId);

            if (existingEntry != null)
            {
                // Update existing entry
                entryToAddOrUpdate.SystemProperties.Version = existingEntry.SystemProperties.Version!.Value;
            }

            // Create new entry
            var entryToPublish = await client.CreateOrUpdateEntry(entryToAddOrUpdate,
                                                                  contentTypeId: ContentTypes.Qualification,
                                                                  version: entryToAddOrUpdate.SystemProperties.Version);

            await client.PublishEntry(entryToPublish.SystemProperties.Id,
                                      entryToPublish.SystemProperties.Version!.Value);
        }
    }

    private static async Task SetUpContentModels(ContentfulManagementClient client)
    {
        // Check current version of model
        var contentTypeModel = await client.GetContentType(ContentTypes.Qualification);

        var version = contentTypeModel?.SystemProperties.Version ?? 1;

        var contentType = new ContentType
                          {
                              SystemProperties = new SystemProperties
                                                 {
                                                     Id = ContentTypes.Qualification
                                                 },
                              Name = ContentTypes.Qualification,
                              Description = "Model for storing all the early years qualifications",
                              DisplayField = "qualificationName",
                              Fields =
                              [
                                  new Field
                                  {
                                      Name = "Qualification ID",
                                      Id = "qualificationId",
                                      Type = FieldTypeSymbol,
                                      Required = true,
                                      Validations = [new UniqueValidator()]
                                  },
                                  new Field
                                  {
                                      Name = "Qualification Name",
                                      Id = "qualificationName",
                                      Type = FieldTypeSymbol,
                                      Required = true
                                  },
                                  new Field
                                  {
                                      Name = "Qualification Level",
                                      Id = "qualificationLevel",
                                      Type = "Integer",
                                      Required = true
                                  },
                                  new Field
                                  {
                                      Name = "Awarding Organisation Title",
                                      Id = "awardingOrganisationTitle",
                                      Type = FieldTypeSymbol,
                                      Required = true
                                  },
                                  new Field
                                  {
                                      Name = "From Which Year",
                                      Id = "fromWhichYear",
                                      Type = FieldTypeSymbol
                                  },
                                  new Field
                                  {
                                      Name = "To Which Year",
                                      Id = "toWhichYear",
                                      Type = FieldTypeSymbol
                                  },
                                  new Field
                                  {
                                      Name = "Qualification Number",
                                      Id = "qualificationNumber",
                                      Type = FieldTypeSymbol
                                  },
                                  new Field
                                  {
                                      Name = "Additional Requirements",
                                      Id = "additionalRequirements",
                                      Type = "Text"
                                  },
                                  new Field
                                  {
                                      Name = "Additional Requirement Questions",
                                      Id = "additionalRequirementQuestions",
                                      Type = "Array",
                                      Items = new Schema
                                              {
                                                  Type = "Link",
                                                  LinkType = "Entry",
                                                  Validations =
                                                  [
                                                      new LinkContentTypeValidator
                                                      {
                                                          ContentTypeIds =
                                                              ["additionalRequirementQuestion"]
                                                      }
                                                  ]
                                              }
                                  },
                                  new Field
                                  {
                                      Name = "Ratio Requirements",
                                      Id = "ratioRequirements",
                                      Type = "Array",
                                      Items = new Schema
                                              {
                                                  Type = "Link",
                                                  LinkType = "Entry",
                                                  Validations =
                                                  [
                                                      new LinkContentTypeValidator
                                                      {
                                                          ContentTypeIds = ["ratioRequirement"]
                                                      }
                                                  ]
                                              }
                                  }
                              ]
                          };

        var typeToActivate = await client.CreateOrUpdateContentType(contentType, version: version);
        await client.ActivateContentType(ContentTypes.Qualification, typeToActivate.SystemProperties.Version!.Value);

        Thread.Sleep(2000); // Allows the API time to activate the content type
        await SetHelpText(client);
    }

    private static async Task SetHelpText(ContentfulManagementClient client)
    {
        var editorInterface = await client.GetEditorInterface(ContentTypes.Qualification);
        SetHelpTextForField(editorInterface, "qualificationId",
                            "The unique identifier used to reference the qualification");
        SetHelpTextForField(editorInterface, "qualificationName", "The name of the qualification");
        SetHelpTextForField(editorInterface, "qualificationLevel", "The level of the qualification",
                            SystemWidgetIds.NumberEditor);
        SetHelpTextForField(editorInterface, "awardingOrganisationTitle", "The name of the awarding organisation");
        SetHelpTextForField(editorInterface, "fromWhichYear",
                            "The date from which the qualification is considered full and relevant");
        SetHelpTextForField(editorInterface, "toWhichYear",
                            "The date on which the qualification stops being considered full and relevant");
        SetHelpTextForField(editorInterface, "qualificationNumber", "The number of the qualification");
        SetHelpTextForField(editorInterface, "additionalRequirements",
                            "The additional requirements for the qualification", SystemWidgetIds.MultipleLine);
        SetHelpTextForField(editorInterface, "additionalRequirementQuestions",
                            "The optional additional requirements questions", SystemWidgetIds.EntryMultipleLinksEditor);
        SetHelpTextForField(editorInterface, "ratioRequirements", "The optional ratio requirements",
                            SystemWidgetIds.EntryMultipleLinksEditor);

        await client.UpdateEditorInterface(editorInterface, ContentTypes.Qualification,
                                           editorInterface.SystemProperties.Version!.Value);
    }

    private static void SetHelpTextForField(EditorInterface editorInterface, string fieldId, string helpText,
                                            string widgetId = SystemWidgetIds.SingleLine)
    {
        editorInterface.Controls.First(x => x.FieldId == fieldId).Settings =
            new EditorInterfaceControlSettings { HelpText = helpText };
        editorInterface.Controls.First(x => x.FieldId == fieldId).WidgetId = widgetId;
    }

    private static Entry<dynamic> BuildEntryFromQualification(QualificationUpload qualification)
    {
        var additionalRequirementQuestions = new List<Reference>();
        if (qualification.AdditionalRequirementQuestions is not null)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var questionId in qualification.AdditionalRequirementQuestions)
            {
                additionalRequirementQuestions.Add(new Reference(SystemLinkTypes.Entry, questionId));
            }
        }

        var ratioRequirements = new List<Reference>();
        if (qualification.RatioRequirements is not null)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var ratioId in qualification.RatioRequirements)
            {
                ratioRequirements.Add(new Reference(SystemLinkTypes.Entry, ratioId));
            }
        }

        var entry = new Entry<dynamic>
                    {
                        SystemProperties = new SystemProperties
                                           {
                                               Id = qualification.QualificationId,
                                               Version = 1
                                           },

                        Fields = new
                                 {
                                     qualificationId = new Dictionary<string, string>
                                                       {
                                                           { Locale, qualification.QualificationId }
                                                       },

                                     qualificationName = new Dictionary<string, string>
                                                         {
                                                             { Locale, qualification.QualificationName }
                                                         },

                                     awardingOrganisationTitle = new Dictionary<string, string>
                                                                 {
                                                                     { Locale, qualification.AwardingOrganisationTitle }
                                                                 },

                                     qualificationLevel = new Dictionary<string, int>
                                                          {
                                                              { Locale, qualification.QualificationLevel }
                                                          },

                                     fromWhichYear = new Dictionary<string, string>
                                                     {
                                                         { Locale, qualification.FromWhichYear ?? "" }
                                                     },

                                     toWhichYear = new Dictionary<string, string>
                                                   {
                                                       { Locale, qualification.ToWhichYear ?? "" }
                                                   },

                                     qualificationNumber = new Dictionary<string, string>
                                                           {
                                                               { Locale, qualification.QualificationNumber ?? "" }
                                                           },

                                     additionalRequirements = new Dictionary<string, string>
                                                              {
                                                                  { Locale, qualification.AdditionalRequirements ?? "" }
                                                              },

                                     additionalRequirementQuestions = new Dictionary<string, List<Reference>>
                                                                      {
                                                                          { Locale, additionalRequirementQuestions }
                                                                      },

                                     ratioRequirements = new Dictionary<string, List<Reference>>
                                                         {
                                                             { Locale, ratioRequirements }
                                                         }
                                 }
                    };

        return entry;
    }

    private static List<QualificationUpload> GetQualificationsToAddOrUpdate()
    {
        var lines = ReadCsvFile(CsvFile);

        var listObjResult = new List<QualificationUpload>();

        foreach (var t in lines)
        {
            var qualificationId = t[0];
            var qualificationName = t[4];
            var awardingOrganisationTitle = t[5];
            var qualificationLevel = int.Parse(t[1]);
            var fromWhichYear = t[2];
            var toWhichYear = t[3];
            var qualificationNumber = t[6];
            var additionalRequirements = t[7];
            var additionalRequirementQuestionString = t[8];
            var ratioRequirementsString = t[9];

            string[] additionalRequirementQuestionsArray = [];
            if (!string.IsNullOrEmpty(additionalRequirementQuestionString))
            {
                additionalRequirementQuestionsArray = additionalRequirementQuestionString.Split(':');
            }

            string[] ratioRequirementsArray = [];
            if (!string.IsNullOrEmpty(ratioRequirementsString))
            {
                ratioRequirementsArray = ratioRequirementsString.Split(':');
            }

            listObjResult.Add(new QualificationUpload(
                                                      qualificationId,
                                                      qualificationName,
                                                      awardingOrganisationTitle,
                                                      qualificationLevel,
                                                      fromWhichYear,
                                                      toWhichYear,
                                                      qualificationNumber,
                                                      additionalRequirements,
                                                      additionalRequirementQuestionsArray,
                                                      ratioRequirementsArray
                                                     ));
        }

        return listObjResult;
    }

    private static List<string[]> ReadCsvFile(string filePath)
    {
        var result = new List<string[]>();
        using var csvParser = new TextFieldParser(filePath);
        csvParser.SetDelimiters(",");
        csvParser.HasFieldsEnclosedInQuotes = true;

        // Skip the row with the column names
        csvParser.ReadLine();

        while (!csvParser.EndOfData)
        {
            // Read current line fields, pointer moves to the next line.
            var fields = csvParser.ReadFields()!;
            result.Add(fields);
        }

        return result;
    }
}