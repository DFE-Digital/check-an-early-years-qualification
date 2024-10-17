using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService(
    ILogger<ContentfulContentService> logger,
    IContentfulClient contentfulClient)
    : ContentfulContentServiceBase(logger, contentfulClient), IContentService
{
    public async Task<StartPage?> GetStartPage()
    {
        var startPageEntries = await GetEntriesByType<StartPage>();

        // ReSharper disable once InvertIf
        if (startPageEntries is null || !startPageEntries.Any())
        {
            Logger.LogWarning("No start page entry returned");
            return default;
        }

        return startPageEntries.First();
    }

    public async Task<DetailsPage?> GetDetailsPage()
    {
        var detailsPageType = ContentTypeLookup[typeof(DetailsPage)];

        var queryBuilder = new QueryBuilder<DetailsPage>().ContentTypeIs(detailsPageType)
                                                          .Include(2);
        
        var detailsPageEntries = await GetEntriesByType(queryBuilder);
        if (detailsPageEntries is null || !detailsPageEntries.Any())
        {
            Logger.LogWarning("No details page entry returned");
            return default;
        }

        var detailsPageContent = detailsPageEntries.First();
        return detailsPageContent;
    }

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
    {
        var accessibilityStatementEntities = await GetEntriesByType<AccessibilityStatementPage>();

        // ReSharper disable once InvertIf
        if (accessibilityStatementEntities is null || !accessibilityStatementEntities.Any())
        {
            Logger.LogWarning("No accessibility statement page entry returned");
            return default;
        }

        return accessibilityStatementEntities.First();
    }

    public async Task<CookiesPage?> GetCookiesPage()
    {
        var cookiesEntities = await GetEntriesByType<CookiesPage>();
        if (cookiesEntities is null || !cookiesEntities.Any())
        {
            Logger.LogWarning("No cookies page entry returned");
            return default;
        }

        var cookiesContent = cookiesEntities.First();
        return cookiesContent;
    }

    public async Task<List<NavigationLink>> GetNavigationLinks()
    {
        var navigationLinkEntries = await GetEntriesByType<NavigationLinks>();
        if (navigationLinkEntries is not null && navigationLinkEntries.Any())
        {
            return navigationLinkEntries.First().Links;
        }

        Logger.LogWarning("No navigation links returned");
        return [];
    }

    public async Task<AdvicePage?> GetAdvicePage(string entryId)
    {
        var advicePage = await GetEntryById<AdvicePage>(entryId);

        // ReSharper disable once InvertIf
        if (advicePage is null)
        {
            Logger.LogWarning("Advice page with {EntryID} could not be found", entryId);
            return default;
        }

        return advicePage;
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        ContentfulClient.SerializerSettings.Converters.Add(new OptionItemConverter());
        return await GetEntryById<RadioQuestionPage>(entryId);
    }

    public async Task<DateQuestionPage?> GetDateQuestionPage(string entryId)
    {
        return await GetEntryById<DateQuestionPage>(entryId);
    }

    public async Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId)
    {
        return await GetEntryById<DropdownQuestionPage>(entryId);
    }

    public async Task<PhaseBanner?> GetPhaseBannerContent()
    {
        var phaseBannerEntities = await GetEntriesByType<PhaseBanner>();

        // ReSharper disable once InvertIf
        if (phaseBannerEntities is null || !phaseBannerEntities.Any())
        {
            Logger.LogWarning("No phase banner entry returned");
            return default;
        }

        return phaseBannerEntities.First();
    }

    public async Task<CookiesBanner?> GetCookiesBannerContent()
    {
        var cookiesBannerEntry = await GetEntriesByType<CookiesBanner>();

        // ReSharper disable once InvertIf
        if (cookiesBannerEntry is null || !cookiesBannerEntry.Any())
        {
            Logger.LogWarning("No cookies banner entry returned");
            return default;
        }

        return cookiesBannerEntry.First();
    }

    public async Task<ConfirmQualificationPage?> GetConfirmQualificationPage()
    {
        var confirmQualificationEntities = await GetEntriesByType<ConfirmQualificationPage>();

        // ReSharper disable once InvertIf
        if (confirmQualificationEntities is null || !confirmQualificationEntities.Any())
        {
            Logger.LogWarning("No confirm qualification page entry returned");
            return default;
        }

        return confirmQualificationEntities.First();
    }

    public async Task<CheckAdditionalRequirementsPage?> GetCheckAdditionalRequirementsPage()
    {
        var checkAdditionalRequirementsPageEntities = await GetEntriesByType<CheckAdditionalRequirementsPage>();

        // ReSharper disable once InvertIf
        // ...more legible as it is
        if (checkAdditionalRequirementsPageEntities is null || !checkAdditionalRequirementsPageEntities.Any())
        {
            Logger.LogWarning("No CheckAdditionalRequirementsPage entry returned");
            return default;
        }

        return checkAdditionalRequirementsPageEntities.First();
    }

    public async Task<QualificationListPage?> GetQualificationListPage()
    {
        var qualificationListPageEntities = await GetEntriesByType<QualificationListPage>();
        if (qualificationListPageEntities is null || !qualificationListPageEntities.Any())
        {
            Logger.LogWarning("No qualification list page entry returned");
            return default;
        }

        var qualificationListPage = qualificationListPageEntities.First();
        return qualificationListPage;
    }

    public async Task<ChallengePage?> GetChallengePage()
    {
        var challengePageEntities = await GetEntriesByType<ChallengePage>();
        if (challengePageEntities is null || !challengePageEntities.Any())
        {
            Logger.LogWarning("No challenge page entry returned");
            return default;
        }

        var challengePage = challengePageEntities.First();
        return challengePage;
    }

    public async Task<CheckAdditionalRequirementsAnswerPage?> GetCheckAdditionalRequirementsAnswerPage()
    {
        var checkAdditionalRequirementsAnswerPageEntities = await GetEntriesByType<CheckAdditionalRequirementsAnswerPage>();
        if (checkAdditionalRequirementsAnswerPageEntities is null || !checkAdditionalRequirementsAnswerPageEntities.Any())
        {
            Logger.LogWarning("No check additional requirements answer entry returned");
            return default;
        }

        var checkAdditionalRequirementsAnswerPage = checkAdditionalRequirementsAnswerPageEntities.First();
        return checkAdditionalRequirementsAnswerPage;
    }
    
    public async Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(int level, int startMonth, int startYear)
    {
        var cannotFindQualificationPageType = ContentTypeLookup[typeof(CannotFindQualificationPage)];
        var queryBuilder = new QueryBuilder<CannotFindQualificationPage>().ContentTypeIs(cannotFindQualificationPageType)
                                                                          .Include(2)
                                                                          .FieldEquals("fields.level", level.ToString());
        
        var cannotFindQualificationPages = await GetEntriesByType(queryBuilder);
        if (cannotFindQualificationPages is null || !cannotFindQualificationPages.Any())
        {
            Logger.LogWarning("No 'cannot find qualification' page entries returned");
            return default;
        }

        var filteredCannotFindQualificationPages =
            FilterCannotFindQualificationPagesByDate(startMonth, startYear, cannotFindQualificationPages.ToList());

        if (filteredCannotFindQualificationPages.Count != 0) return filteredCannotFindQualificationPages.First();
        Logger.LogWarning("No filtered 'cannot find qualification' page entries returned");
        return default;
    }
    
    private List<CannotFindQualificationPage> FilterCannotFindQualificationPagesByDate(int startDateMonth, int startDateYear,
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