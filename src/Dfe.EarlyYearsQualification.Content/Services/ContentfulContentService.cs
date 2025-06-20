using Contentful.Core;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;
using Microsoft.Extensions.Logging;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class ContentfulContentService(
    ILogger<ContentfulContentService> logger,
    IContentfulClient contentfulClient,
    IDateValidator dateValidator)
    : ContentfulContentServiceBase(logger, contentfulClient), IContentService
{
    public async Task<StartPage?> GetStartPage()
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

    public async Task<DetailsPage?> GetDetailsPage()
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

    public async Task<AccessibilityStatementPage?> GetAccessibilityStatementPage()
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

    public async Task<CookiesPage?> GetCookiesPage()
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
            return null;
        }

        return advicePage;
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPage(string entryId)
    {
        ContentfulClient.SerializerSettings.Converters.Add(new OptionItemConverter());
        return await GetEntryById<RadioQuestionPage>(entryId);
    }

    public async Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId)
    {
        return await GetEntryById<DatesQuestionPage>(entryId);
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
            return null;
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
            return null;
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
            return null;
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
            return null;
        }

        return checkAdditionalRequirementsPageEntities.First();
    }

    public async Task<QualificationListPage?> GetQualificationListPage()
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

    public async Task<ChallengePage?> GetChallengePage()
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

    public async Task<CheckAdditionalRequirementsAnswerPage?> GetCheckAdditionalRequirementsAnswerPage()
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

    public async Task<OpenGraphData?> GetOpenGraphData()
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

    public async Task<CannotFindQualificationPage?> GetCannotFindQualificationPage(
        int level, int startMonth, int startYear)
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
            return null;
        }

        var filteredCannotFindQualificationPages =
            FilterCannotFindQualificationPagesByDate(startMonth, startYear, cannotFindQualificationPages.ToList());

        if (filteredCannotFindQualificationPages.Count != 0) return filteredCannotFindQualificationPages[0];
        Logger.LogWarning("No filtered 'cannot find qualification' page entries returned");
        return null;
    }

    public async Task<CheckYourAnswersPage?> GetCheckYourAnswersPage()
    {
        var checkYourAnswersPages = await GetEntriesByType<CheckYourAnswersPage>();

        // ReSharper disable once InvertIf
        if (checkYourAnswersPages is null || !checkYourAnswersPages.Any())
        {
            Logger.LogWarning("No 'Check your answers pages' returned");
            return null;
        }

        return checkYourAnswersPages.First();
    }

    public async Task<HelpPage?> GetHelpPage()
    {
        var helpPage = await GetEntriesByType<HelpPage>();

        // ReSharper disable once InvertIf
        if (helpPage is null || !helpPage.Any())
        {
            Logger.LogWarning("No 'Help Page' returned");
            return null;
        }

        return helpPage.First();
    }

    public async Task<HelpConfirmationPage?> GetHelpConfirmationPage()
    {
        var helpConfirmationPage = await GetEntriesByType<HelpConfirmationPage>();

        // ReSharper disable once InvertIf
        if (helpConfirmationPage is null || !helpConfirmationPage.Any())
        {
            Logger.LogWarning("No 'Help Confirmation Page' returned");
            return null;
        }

        return helpConfirmationPage.First();
    }

    public async Task<PreCheckPage?> GetPreCheckPage()
    {
        ContentfulClient.SerializerSettings.Converters.Add(new OptionItemConverter());
        var preCheckPage = await GetEntriesByType<PreCheckPage>();

        // ReSharper disable once InvertIf
        if (preCheckPage is null || !preCheckPage.Any())
        {
            Logger.LogWarning("No 'Pre Check Page' returned");
            return null;
        }

        return preCheckPage.First();
    }

    private List<CannotFindQualificationPage> FilterCannotFindQualificationPagesByDate(
        int startDateMonth, int startDateYear,
        List<CannotFindQualificationPage> cannotFindQualificationPages)
    {
        var results = new List<CannotFindQualificationPage>();
        var enteredStartDate = new DateOnly(startDateYear, startDateMonth, dateValidator.GetDay());
        foreach (var page in cannotFindQualificationPages)
        {
            var pageStartDate = dateValidator.GetDate(page.FromWhichYear);
            var pageEndDate = dateValidator.GetDate(page.ToWhichYear);

            var result = dateValidator.ValidateDateEntry(pageStartDate, pageEndDate, enteredStartDate, page);
            if (result is not null)
            {
                results.Add(result);
            }
        }

        return results;
    }
}