using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Extensions;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService(
    ILogger<ContentfulContentService> logger,
    IContentfulClient contentfulClient,
    IDistributedCache distributedCache)
    : ContentfulContentServiceBase(logger, contentfulClient), IContentService
{
    public async Task<StartPage?> GetStartPage()
    {
        const string cacheKey = $"content:{nameof(StartPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetStartPage);

        return val;
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        const string cacheKey = $"content:{nameof(DetailsPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetDetailsPage);

        return val;
    }

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        const string cacheKey = $"content:{nameof(AccessibilityStatementPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetAccessibilityStatementPage);

        return val;
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        const string cacheKey = $"content:{nameof(CookiesPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetCookiesPage);

        return val;
    }

    public async Task<List<NavigationLink>> GetNavigationLinks()
    {
        const string cacheKey = $"content:{nameof(NavigationLink)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetNavigationLinks);

        return val!;
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        var cacheKey = $"content:{nameof(AdvicePage)}:{entryId}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, () => InternalGetAdvicePage(entryId));

        return val;
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        var cacheKey = $"content:{nameof(RadioQuestionPage)}:{entryId}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, () => InternalGetRadioQuestionPage(entryId));

        return val;
    }

    public async Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId)
    {
        var cacheKey = $"content:{nameof(DatesQuestionPage)}:{entryId}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, () => GetEntryById<DatesQuestionPage>(entryId));

        return val;
    }

    public async Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId)
    {
        var cacheKey = $"content:{nameof(DropdownQuestionPage)}:{entryId}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, () => GetEntryById<DropdownQuestionPage>(entryId));

        return val;
    }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        const string cacheKey = $"content:{nameof(PhaseBanner)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetPhaseBannerContent);

        return val;
    }

    public async Task<CookiesBanner?> GetCookiesBannerContent()
    {
        const string cacheKey = $"content:{nameof(CookiesBanner)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetCookiesBannerContent);

        return val;
    }

    public async Task<ConfirmQualificationPage?> GetConfirmQualificationPage()
    {
        const string cacheKey = $"content:{nameof(ConfirmQualificationPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetConfirmQualificationPage);

        return val;
    }

    public async Task<CheckAdditionalRequirementsPage?> GetCheckAdditionalRequirementsPage()
    {
        const string cacheKey = $"content:{nameof(CheckAdditionalRequirementsPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetCheckAdditionalRequirementsPage);

        return val;
    }

    public async Task<QualificationListPage?> GetQualificationListPage()
    {
        const string cacheKey = $"content:{nameof(QualificationListPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetQualificationListPage);

        return val;
    }

    public async Task<ChallengePage?> GetChallengePage()
    {
        const string cacheKey = $"content:{nameof(ChallengePage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetChallengePage);

        return val;
    }

    public async Task<CheckAdditionalRequirementsAnswerPage?> GetCheckAdditionalRequirementsAnswerPage()
    {
        const string cacheKey = $"content:{nameof(CheckAdditionalRequirementsAnswerPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetCheckAdditionalRequirementsAnswerPage);

        return val;
    }

    public async Task<OpenGraphData?> GetOpenGraphData()
    {
        const string cacheKey = $"content:{nameof(OpenGraphData)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetOpenGraphData);

        return val;
    }

    public async Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(
        int level, int startMonth, int startYear)
    {
        var cacheKey = $"content:{nameof(GetCannotFindQualificationPage)}|{level}";

        var list = await distributedCache.GetOrSetAsync(cacheKey,
                                                        () => InternalGetCannotFindQualificationPages(level));

        var filteredCannotFindQualificationPages =
            FilterCannotFindQualificationPagesByDate(startMonth, startYear, list!);

        if (filteredCannotFindQualificationPages.Count != 0) return filteredCannotFindQualificationPages[0];
        Logger.LogWarning("No filtered 'cannot find qualification' page entries returned");
        return null;
    }

    public async Task<CheckYourAnswersPage?> GetCheckYourAnswersPage()
    {
        const string cacheKey = $"content:{nameof(GetCheckYourAnswersPage)}";
        var val = await distributedCache.GetOrSetAsync(cacheKey, InternalGetCheckYourAnswersPage);

        return val;
    }

    private async Task<CheckYourAnswersPage?> InternalGetCheckYourAnswersPage()
    {
        var checkYourAnswersPages = await GetEntriesByType<CheckYourAnswersPage>();

        // ReSharper disable once InvertIf
        if (checkYourAnswersPages is null || !checkYourAnswersPages.Any())
        {
            Logger.LogWarning("No open graph data entry returned");
            return null;
        }

        return checkYourAnswersPages.First();
    }

    private async Task<List<CannotFindQualificationPage>> InternalGetCannotFindQualificationPages(int level)
    {
        var cannotFindQualificationPageType = ContentTypeLookup[typeof(CannotFindQualificationPage)];
        var queryBuilder = new QueryBuilder<CannotFindQualificationPage>()
                           .ContentTypeIs(cannotFindQualificationPageType)
                           .Include(2)
                           .FieldEquals("fields.level", level.ToString());

        var cannotFindQualificationPages = await GetEntriesByType(queryBuilder);
        if (cannotFindQualificationPages is null || !cannotFindQualificationPages.Any())
        {
            Logger.LogWarning("No 'cannot find qualification' page entries returned");
            return [];
        }

        return cannotFindQualificationPages.ToList();
    }

    private async Task<OpenGraphData?> InternalGetOpenGraphData()
    {
        var openGraphEntities = await GetEntriesByType<OpenGraphData>();
        if (openGraphEntities is null || !openGraphEntities.Any())
        {
            Logger.LogWarning("No open graph data entry returned");
            return null;
        }

        var openGraphData = openGraphEntities.First();
        return openGraphData;
    }

    private async Task<CheckAdditionalRequirementsAnswerPage?> InternalGetCheckAdditionalRequirementsAnswerPage()
    {
        var checkAdditionalRequirementsAnswerPageEntities =
            await GetEntriesByType<CheckAdditionalRequirementsAnswerPage>();
        if (checkAdditionalRequirementsAnswerPageEntities is null ||
            !checkAdditionalRequirementsAnswerPageEntities.Any())
        {
            Logger.LogWarning("No check additional requirements answer entry returned");
            return null;
        }

        var checkAdditionalRequirementsAnswerPage = checkAdditionalRequirementsAnswerPageEntities.First();
        return checkAdditionalRequirementsAnswerPage;
    }

    private async Task<ChallengePage?> InternalGetChallengePage()
    {
        var challengePageEntities = await GetEntriesByType<ChallengePage>();
        if (challengePageEntities is null || !challengePageEntities.Any())
        {
            Logger.LogWarning("No challenge page entry returned");
            return null;
        }

        var challengePage = challengePageEntities.First();
        return challengePage;
    }

    private async Task<QualificationListPage?> InternalGetQualificationListPage()
    {
        var qualificationListPageEntities = await GetEntriesByType<QualificationListPage>();
        if (qualificationListPageEntities is null || !qualificationListPageEntities.Any())
        {
            Logger.LogWarning("No qualification list page entry returned");
            return null;
        }

        var qualificationListPage = qualificationListPageEntities.First();
        return qualificationListPage;
    }

    private async Task<CheckAdditionalRequirementsPage?> InternalGetCheckAdditionalRequirementsPage()
    {
        var checkAdditionalRequirementsPageEntities = await GetEntriesByType<CheckAdditionalRequirementsPage>();

        // ReSharper disable once InvertIf
        // ...more legible as it is
        if (checkAdditionalRequirementsPageEntities is null || !checkAdditionalRequirementsPageEntities.Any())
        {
            Logger.LogWarning("No CheckAdditionalRequirementsPage entry returned");
            return null;
        }

        return checkAdditionalRequirementsPageEntities.First();
    }

    private async Task<ConfirmQualificationPage?> InternalGetConfirmQualificationPage()
    {
        var confirmQualificationEntities = await GetEntriesByType<ConfirmQualificationPage>();

        // ReSharper disable once InvertIf
        if (confirmQualificationEntities is null || !confirmQualificationEntities.Any())
        {
            Logger.LogWarning("No confirm qualification page entry returned");
            return null;
        }

        return confirmQualificationEntities.First();
    }

    private async Task<CookiesBanner?> InternalGetCookiesBannerContent()
    {
        var cookiesBannerEntry = await GetEntriesByType<CookiesBanner>();

        // ReSharper disable once InvertIf
        if (cookiesBannerEntry is null || !cookiesBannerEntry.Any())
        {
            Logger.LogWarning("No cookies banner entry returned");
            return null;
        }

        return cookiesBannerEntry.First();
    }

    private async Task<PhaseBanner?> InternalGetPhaseBannerContent()
    {
        var phaseBannerEntities = await GetEntriesByType<PhaseBanner>();

        // ReSharper disable once InvertIf
        if (phaseBannerEntities is null || !phaseBannerEntities.Any())
        {
            Logger.LogWarning("No phase banner entry returned");
            return null;
        }

        return phaseBannerEntities.First();
    }

    private async Task<RadioQuestionPage?> InternalGetRadioQuestionPage(string entryId)
    {
        ContentfulClient.SerializerSettings.Converters.Add(new OptionItemConverter());
        return await GetEntryById<RadioQuestionPage>(entryId);
    }

    private async Task<AdvicePage?> InternalGetAdvicePage(string entryId)
    {
        var advicePage = await GetEntryById<AdvicePage>(entryId);

        // ReSharper disable once InvertIf
        if (advicePage is null)
        {
            Logger.LogWarning("Advice page with {EntryID} could not be found", entryId);
            return null;
        }

        return advicePage;
    }

    private async Task<List<NavigationLink>> InternalGetNavigationLinks()
    {
        var navigationLinkEntries = await GetEntriesByType<NavigationLinks>();
        if (navigationLinkEntries is not null && navigationLinkEntries.Any())
        {
            return navigationLinkEntries.First().Links;
        }

        Logger.LogWarning("No navigation links returned");
        return [];
    }

    private async Task<CookiesPage?> InternalGetCookiesPage()
    {
        var cookiesEntities = await GetEntriesByType<CookiesPage>();
        if (cookiesEntities is null || !cookiesEntities.Any())
        {
            Logger.LogWarning("No cookies page entry returned");
            return null;
        }

        var cookiesContent = cookiesEntities.First();
        return cookiesContent;
    }

    private async Task<AccessibilityStatementPage?> InternalGetAccessibilityStatementPage()
    {
        var accessibilityStatementEntities = await GetEntriesByType<AccessibilityStatementPage>();

        // ReSharper disable once InvertIf
        if (accessibilityStatementEntities is null || !accessibilityStatementEntities.Any())
        {
            Logger.LogWarning("No accessibility statement page entry returned");
            return null;
        }

        return accessibilityStatementEntities.First();
    }

    private async Task<DetailsPage?> InternalGetDetailsPage()
    {
        var detailsPageType = ContentTypeLookup[typeof(DetailsPage)];

        var queryBuilder = new QueryBuilder<DetailsPage>().ContentTypeIs(detailsPageType)
                                                          .Include(2);

        var detailsPageEntries = await GetEntriesByType(queryBuilder);
        if (detailsPageEntries is null || !detailsPageEntries.Any())
        {
            Logger.LogWarning("No details page entry returned");
            return null;
        }

        var detailsPageContent = detailsPageEntries.First();
        return detailsPageContent;
    }

    private async Task<StartPage?> InternalGetStartPage()
    {
        var startPageEntries = await GetEntriesByType<StartPage>();

        // ReSharper disable once InvertIf
        if (startPageEntries is null || !startPageEntries.Any())
        {
            Logger.LogWarning("No start page entry returned");
            return null;
        }

        return startPageEntries.First();
    }

    private List<CannotFindQualificationPage> FilterCannotFindQualificationPagesByDate(
        int startDateMonth, int startDateYear,
        List<CannotFindQualificationPage> cannotFindQualificationPages)
    {
        var results = new List<CannotFindQualificationPage>();
        var enteredStartDate = new DateOnly(startDateYear, startDateMonth, Day);
        foreach (var page in cannotFindQualificationPages)
        {
            var pageStartDate = GetDate(page.FromWhichYear);
            var pageEndDate = GetDate(page.ToWhichYear);

            var result = ValidateDateEntry(pageStartDate, pageEndDate, enteredStartDate, page);
            if (result is not null)
            {
                results.Add(result);
            }
        }

        return results;
    }
}