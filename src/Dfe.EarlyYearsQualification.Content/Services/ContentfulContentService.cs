using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.EarlyYearsQualification.Content.Converters;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
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

    public async Task<QualificationDetailsPage?> GetQualificationDetailsPage(
        bool userIsCheckingOwnQualification, bool isFullAndRelevant, int level, int startMonth, int startYear,
        bool isDegreeSpecificPage)
    {
        var qualificationDetailsPageType = ContentTypeLookup[typeof(QualificationDetailsPage)];

        var queryBuilder = new QueryBuilder<QualificationDetailsPage>()
                           .ContentTypeIs(qualificationDetailsPageType)
                           .Include(3)
                           .FieldEquals("fields.isPractitionerSpecificPage",
                                        userIsCheckingOwnQualification ? "1" : "0");

        if (userIsCheckingOwnQualification)
        {
            queryBuilder = queryBuilder
                           .FieldEquals("fields.level", level.ToString())
                           .FieldEquals("fields.isFullAndRelevant", isFullAndRelevant ? "1" : "0")
                           .FieldEquals("fields.isDegreeSpecificPage", isDegreeSpecificPage ? "1" : "0");
        }

        var qualificationDetailsPageEntries = await GetEntriesByType(queryBuilder);

        if (qualificationDetailsPageEntries is null || !qualificationDetailsPageEntries.Any())
        {
            Logger.LogWarning("No qualification details page entry returned");
            return null;
        }

        // Filter out content where date is not between FromWhichYear and ToWhichYear
        if (userIsCheckingOwnQualification)
        {
            return GetFilteredPractitionerQualificationDetailsPage(startMonth, startYear, qualificationDetailsPageEntries);
        }

        return qualificationDetailsPageEntries.FirstOrDefault();
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
        int level, int startMonth, int startYear, bool isUserCheckingTheirOwnQualification)
    {
        var cannotFindQualificationPageType = ContentTypeLookup[typeof(CannotFindQualificationPage)];
        var queryBuilder = new QueryBuilder<CannotFindQualificationPage>()
                           .ContentTypeIs(cannotFindQualificationPageType)
                           .Include(2)
                           .FieldEquals("fields.level", level.ToString())
                           .FieldEquals("fields.isPractitionerSpecificPage",
                                        isUserCheckingTheirOwnQualification ? "1" : "0");

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

    public async Task<Footer?> GetFooter()
    {
        var footerType = ContentTypeLookup[typeof(Footer)];
        var queryBuilder = new QueryBuilder<Footer>().ContentTypeIs(footerType)
                                                     .Include(2);
        var footer = await GetEntriesByType(queryBuilder);

        // ReSharper disable once InvertIf
        if (footer is null || !footer.Any())
        {
            Logger.LogWarning("No 'Footer' returned");
            return null;
        }

        return footer.First();
    }

    public async Task<FeedbackFormPage?> GetFeedbackFormPage()
    {
        ContentfulClient.SerializerSettings.Converters.Add(new OptionItemConverter());
        ContentfulClient.SerializerSettings.Converters.Add(new FeedbackFormQuestionConverter());
        var feedbackFormPageType = ContentTypeLookup[typeof(FeedbackFormPage)];
        var queryBuilder = new QueryBuilder<FeedbackFormPage>().ContentTypeIs(feedbackFormPageType)
                                                               .Include(3);
        var feedbackFormPages = await GetEntriesByType(queryBuilder);

        // ReSharper disable once InvertIf
        if (feedbackFormPages is null || !feedbackFormPages.Any())
        {
            Logger.LogWarning("No 'Feedback Form Page' returned");
            return null;
        }

        return feedbackFormPages.First();
    }

    public async Task<FeedbackFormConfirmationPage?> GetFeedbackFormConfirmationPage()
    {
        var feedbackFormConfirmationPage = await GetEntriesByType<FeedbackFormConfirmationPage>();

        // ReSharper disable once InvertIf
        if (feedbackFormConfirmationPage is null || !feedbackFormConfirmationPage.Any())
        {
            Logger.LogWarning("No 'Feedback form confirmation page' returned");
            return null;
        }

        return feedbackFormConfirmationPage.First();
    }

    public async Task<GetHelpPage?> GetGetHelpPage()
    {
        var getHelpPage = await GetEntriesByType<GetHelpPage>();

        // ReSharper disable once InvertIf
        if (getHelpPage is null || !getHelpPage.Any())
        {
            Logger.LogWarning("No 'help page' returned");
            return null;
        }

        return getHelpPage.First();
    }

    public async Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPage()
    {
        var helpQualificationDetailsPage = await GetEntriesByType<HelpQualificationDetailsPage>();

        // ReSharper disable once InvertIf
        if (helpQualificationDetailsPage is null || !helpQualificationDetailsPage.Any())
        {
            Logger.LogWarning("No 'help qualification details page' returned");
            return null;
        }

        return helpQualificationDetailsPage.First();
    }

    public async Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage()
    {
        var helpProvideDetailsPage = await GetEntriesByType<HelpProvideDetailsPage>();

        // ReSharper disable once InvertIf
        if (helpProvideDetailsPage is null || !helpProvideDetailsPage.Any())
        {
            Logger.LogWarning("No 'help provide details page' returned");
            return null;
        }

        return helpProvideDetailsPage.First();
    }

    public async Task<HelpEmailAddressPage?> GetHelpEmailAddressPage()
    {
        var emailAddressContent = await GetEntriesByType<HelpEmailAddressPage>();

        // ReSharper disable once InvertIf
        if (emailAddressContent is null || !emailAddressContent.Any())
        {
            Logger.LogWarning("No 'help email address page' returned");
            return null;
        }

        return emailAddressContent.First();
    }

    public async Task<HelpConfirmationPage?> GetHelpConfirmationPage()
    {
        var helpConfirmationPageType = ContentTypeLookup[typeof(HelpConfirmationPage)];
        var queryBuilder = new QueryBuilder<HelpConfirmationPage>().ContentTypeIs(helpConfirmationPageType)
                                                                   .Include(2);
        var helpConfirmationPage = await GetEntriesByType(queryBuilder);

        // ReSharper disable once InvertIf
        if (helpConfirmationPage is null || !helpConfirmationPage.Any())
        {
            Logger.LogWarning("No 'Help Confirmation Page' returned");
            return null;
        }

        return helpConfirmationPage.First();
    }
    
    private QualificationDetailsPage? GetFilteredPractitionerQualificationDetailsPage(
        int startMonth, int startYear, ContentfulCollection<QualificationDetailsPage> qualificationDetailsPageEntries)
    {
        var enteredStartDate = new DateOnly(startYear, startMonth, dateValidator.GetDay());

        foreach (var page in qualificationDetailsPageEntries)
        {
            var pageStartDate = dateValidator.GetDate(page.FromWhichYear);
            var pageEndDate = dateValidator.GetDate(page.ToWhichYear);

            // Start & End dates are optional. If the results only contains 1 page, return that.
            if (qualificationDetailsPageEntries.Items.Count() == 1 && pageStartDate is null && pageEndDate is null)
            {
                return page;
            }

            var result = dateValidator.ValidateDateEntry(pageStartDate, pageEndDate, enteredStartDate, page);

            if (result is not null)
            {
                return result;
            }
        }

        Logger.LogError("No user is checking own qualification details page entry returned");
        return null;
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