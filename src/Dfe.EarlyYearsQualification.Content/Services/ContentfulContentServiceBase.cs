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
    private readonly ILogger _logger;
    protected readonly IContentfulClient ContentfulClient;

    protected readonly Dictionary<object, string> ContentTypeLookup
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

    protected ContentfulContentServiceBase(IContentfulClient contentfulClient, ILogger logger)
    {
        ContentfulClient = contentfulClient;
        ContentfulClient.ContentTypeResolver = new EntityResolver();

        _logger = logger;
    }

    protected async Task<T?> GetEntryById<T>(string entryId)
    {
        try
        {
            // NOTE: GetEntry doesn't bind linked references which is why we are using GetEntriesByType
            var queryBuilder = new QueryBuilder<T>().ContentTypeIs(ContentTypeLookup[typeof(T)])
                                                    .Include(2)
                                                    .FieldEquals("sys.id", entryId);
            var entry = await ContentfulClient.GetEntriesByType(ContentTypeLookup[typeof(T)], queryBuilder);
            return entry.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
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
            _logger.LogError(ex, "Exception trying to retrieve {TypeName} from Contentful.",
                             typeName);
            return default;
        }
    }
}