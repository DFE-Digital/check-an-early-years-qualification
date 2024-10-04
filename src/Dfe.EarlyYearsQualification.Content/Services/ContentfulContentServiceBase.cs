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
              { typeof(DateQuestionPage), ContentTypes.DateQuestionPage },
              { typeof(DropdownQuestionPage), ContentTypes.DropdownQuestionPage },
              { typeof(QualificationListPage), ContentTypes.QualificationListPage },
              { typeof(ConfirmQualificationPage), ContentTypes.ConfirmQualificationPage },
              { typeof(CheckAdditionalRequirementsPage), ContentTypes.CheckAdditionalRequirementsPage },
              { typeof(ChallengePage), ContentTypes.ChallengePage }
          };

    protected readonly ILogger Logger;

    protected ContentfulContentServiceBase(ILogger logger, IContentfulClient contentfulClient)
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
            var contentType = ContentTypeLookup[typeof(T)];

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
            var typeName = type.Name;
            Logger.LogError(ex, "Exception trying to retrieve {TypeName} from Contentful.",
                            typeName);
            return default;
        }
    }
}