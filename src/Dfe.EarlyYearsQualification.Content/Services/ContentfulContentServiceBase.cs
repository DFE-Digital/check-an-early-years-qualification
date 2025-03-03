using System.Collections.ObjectModel;
using System.Globalization;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Resolvers;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentServiceBase
{
    protected const int Day = 28;

    private static readonly ReadOnlyDictionary<string, int>
        Months = new(
                     new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
                     {
                         { "Jan", 1 },
                         { "Feb", 2 },
                         { "Mar", 3 },
                         { "Apr", 4 },
                         { "May", 5 },
                         { "Jun", 6 },
                         { "Jul", 7 },
                         { "Aug", 8 },
                         { "Sep", 9 },
                         { "Oct", 10 },
                         { "Nov", 11 },
                         { "Dec", 12 }
                     });

    protected readonly IContentfulClient ContentfulClient;

    protected readonly Dictionary<Type, string> ContentTypeLookup
        = new()
          {
              { typeof(StartPage), ContentTypes.StartPage },
              { typeof(Qualification), ContentTypes.Qualification },
              { typeof(DetailsPage), ContentTypes.DetailsPage },
              { typeof(AdvicePage), ContentTypes.AdvicePage },
              { typeof(RadioQuestionPage), ContentTypes.RadioQuestionPage },
              { typeof(AccessibilityStatementPage), ContentTypes.AccessibilityStatementPage },
              { typeof(NavigationLinks), ContentTypes.NavigationLinks },
              { typeof(CookiesPage), ContentTypes.CookiesPage },
              { typeof(PhaseBanner), ContentTypes.PhaseBanner },
              { typeof(CookiesBanner), ContentTypes.CookiesBanner },
              { typeof(DatesQuestionPage), ContentTypes.DatesQuestionPage },
              { typeof(DropdownQuestionPage), ContentTypes.DropdownQuestionPage },
              { typeof(QualificationListPage), ContentTypes.QualificationListPage },
              { typeof(ConfirmQualificationPage), ContentTypes.ConfirmQualificationPage },
              { typeof(CheckAdditionalRequirementsPage), ContentTypes.CheckAdditionalRequirementsPage },
              { typeof(ChallengePage), ContentTypes.ChallengePage },
              { typeof(CannotFindQualificationPage), ContentTypes.CannotFindQualificationPage },
              { typeof(CheckAdditionalRequirementsAnswerPage), ContentTypes.CheckAdditionalRequirementsAnswerPage },
              { typeof(OpenGraphData), ContentTypes.OpenGraphData },
              { typeof(CheckYourAnswersPage), ContentTypes.CheckYourAnswersPage }
          };

    protected readonly ILogger Logger;

    protected ContentfulContentServiceBase(ILogger logger,
                                           IContentfulClient contentfulClient)
    {
        ContentfulClient = contentfulClient;
        ContentfulClient.ContentTypeResolver = new EntityResolver();

        Logger = logger;
    }

    protected async Task<T?> GetEntryById<T>(string entryId)
    {
        try
        {
            // NOTE: ContentfulClient.GetEntry doesn't bind linked references which is why we are using GetEntriesByType
            string contentType = ContentTypeLookup[typeof(T)];

            var queryBuilder = new QueryBuilder<T>().ContentTypeIs(contentType)
                                                    .Include(2)
                                                    .FieldEquals("sys.id", entryId);

            var entries = await ContentfulClient.GetEntriesByType(contentType, queryBuilder);

            return entries.FirstOrDefault();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                            "Exception trying to retrieve entryId {EntryId} for type {Type} from Contentful.",
                            entryId, nameof(T));
            return default;
        }
    }

    protected async Task<ContentfulCollection<T>?> GetEntriesByType<T>(QueryBuilder<T>? queryBuilder = null)
    {
        var type = typeof(T);

        try
        {
            var results = await ContentfulClient.GetEntriesByType(ContentTypeLookup[type], queryBuilder);
            return results;
        }
        catch (Exception ex)
        {
            string typeName = type.Name;
            Logger.LogError(ex, "Exception trying to retrieve {TypeName} from Contentful.", typeName);
            return null;
        }
    }

    protected static T? ValidateDateEntry<T>(DateOnly? startDate, DateOnly? endDate, DateOnly enteredStartDate,
                                             T entry)
    {
        if (startDate is not null
            && endDate is not null
            && enteredStartDate >= startDate
            && enteredStartDate <= endDate)
        {
            // check start date falls between those dates & add to results
            return entry;
        }

        if (startDate is null
            && endDate is not null
            // ReSharper disable once MergeSequentialChecks
            // ...reveals the intention more clearly this way
            && enteredStartDate <= endDate)
        {
            // if qualification start date is null, check entered start date is <= ToWhichYear & add to results
            return entry;
        }

        // if qualification end date is null, check entered start date is >= FromWhichYear & add to results
        if (startDate is not null
            && endDate is null
            && enteredStartDate >= startDate)
        {
            return entry;
        }

        return default;
    }

    protected DateOnly? GetDate(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString) || dateString == "null")
        {
            return null;
        }

        return ConvertToDateTime(dateString);
    }

    private DateOnly? ConvertToDateTime(string dateString)
    {
        (bool isValid, int month, int yearMod2000) = ValidateDate(dateString);

        if (!isValid)
        {
            return null;
        }

        int year = yearMod2000 + 2000;

        return new DateOnly(year, month, Day);
    }

    private (bool isValid, int month, int yearMod2000) ValidateDate(string dateString)
    {
        string[] splitDateString = dateString.Split('-');
        if (splitDateString.Length != 2)
        {
            Logger.LogError("dateString {DateString} has unexpected format", dateString);
            return (false, 0, 0);
        }

        string abbreviatedMonth = splitDateString[0];
        string yearFilter = splitDateString[1];

        bool yearIsValid = int.TryParse(yearFilter,
                                        NumberStyles.Integer,
                                        NumberFormatInfo.InvariantInfo,
                                        out int yearPart);

        if (!yearIsValid)
        {
            Logger.LogError("dateString {DateString} contains unexpected year value",
                            dateString);
            return (false, 0, 0);
        }

        if (Months.TryGetValue(abbreviatedMonth, out int month))
        {
            return (true, month, yearPart);
        }

        Logger.LogError("dateString {DateString} contains unexpected month value",
                        dateString);

        return (false, 0, 0);
    }
}