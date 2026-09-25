using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Services.Entities;

namespace Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;

public class QualificationSearchService(
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService
) : IQualificationSearchService
{
    public void Refine(string refineSearch)
    {
        userJourneyCookieService.SetQualificationNameSearchCriteria(refineSearch);
    }

    public async Task<QualificationListModel?> GetQualifications()
    {
        var qualificationListPage = await contentService.GetQualificationListPage();
        if (qualificationListPage is null) return null;

        var filteredQualifications = await GetFilteredQualifications();
        var baselineQualifications = await GetFilteredQualifications(searchCriteriaOverride: string.Empty);
        // TODO: revert before committing — hardcoded to 0 to test the no-results state locally
        var model = await MapList(qualificationListPage, filteredQualifications, 0);
        return model;
    }

    public async Task<List<Qualification>> GetFilteredQualifications(string? searchCriteriaOverride = null)
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = searchCriteriaOverride ?? userJourneyCookieService.GetSearchCriteria();
        var nationAwardedIn = userJourneyCookieService.GetWhereWasQualificationAwarded();

        var qualifications = await qualificationsRepository.Get(new QualificationFilterOptions
                                                                {
                                                                    Level = level,
                                                                    StartDateMonth = startDateMonth,
                                                                    StartDateYear = startDateYear,
                                                                    AwardingOrganisation = awardingOrganisation,
                                                                    QualificationName = searchCriteria,
                                                                    Nation = nationAwardedIn,
                                                                    IncludeAllQualifications = false
                                                                });

        // Not in list has been selected so we need to filter out qualifications with specific awarding organisations
        if (awardingOrganisation is null)
        {
            qualifications = qualifications.Where(q => q.AwardingOrganisationTitle.Equals(AwardingOrganisations.Various, StringComparison.OrdinalIgnoreCase) 
                                                       || q.AwardingOrganisationTitle.Equals(AwardingOrganisations.AllHigherEducation, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return qualifications;
    }

    public async Task<Qualification?> GetQualificationById(string qualificationId)
    {
        return await qualificationsRepository.GetById(qualificationId);
    }

    public async Task<QualificationListModel> MapList(QualificationListPage content,
                                                      List<Qualification>? qualifications,
                                                      int totalNumberOfQualifications)
    {
        var basicQualificationsModels = qualifications == null ? [] : GetBasicQualificationsModels(qualifications);
        var numberOfMatchingQualifications = qualifications?.Count ?? 0;
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();
        var hasSearchCriteria = !string.IsNullOrWhiteSpace(searchCriteria);
        var isSingleQualification = totalNumberOfQualifications == 1;

        var searchWithinHeading = isSingleQualification
            ? content.SearchWithinSingleHeading
            : string.Format(content.SearchWithinMultipleHeadingFormat, totalNumberOfQualifications);

        var enterKeywordsContent = isSingleQualification
            ? content.EnterKeywordsSingleContent
            : string.Format(content.EnterKeywordsMultipleContentFormat, totalNumberOfQualifications);

        string? searchMatchHeading = null;
        string? searchNoMatchGuidanceIntro = null;
        string? searchNoMatchGuidance = null;

        if (hasSearchCriteria)
        {
            searchMatchHeading = string.Format(content.SearchMatchHeadingFormat,
                                                numberOfMatchingQualifications,
                                                totalNumberOfQualifications,
                                                searchCriteria);

            if (numberOfMatchingQualifications == 0)
            {
                searchNoMatchGuidanceIntro =
                    string.Format(content.SearchNoMatchGuidanceIntroFormat, totalNumberOfQualifications);
                searchNoMatchGuidance = await contentParser.ToHtml(content.SearchNoMatchGuidance);
            }
        }

        return new QualificationListModel
               {
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   Filters = GetFilterModel(content),
                   Header = content.Header,
                   QualificationFoundPrefixText = content.QualificationFoundPrefix,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   TotalNumberOfQualifications = totalNumberOfQualifications,
                   SearchWithinHeading = searchWithinHeading,
                   EnterKeywordsContent = enterKeywordsContent,
                   SearchButtonText = content.SearchButtonText,
                   PostQualificationListContentHeading = content.PostQualificationListContentHeading,
                   PostQualificationListContent = await contentParser.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = searchCriteria,
                   // TODO: revert to `await contentParser.ToHtml(content.NoResultsText)` once Contentful is updated
                   NoResultText =
                       "<p class=\"govuk-body\"><a class=\"govuk-link\" href=\"/questions/check-your-answers\">Check your answers</a> to make sure they are correct.</p>" +
                       "<p class=\"govuk-body\">If you checked your answers and still cannot find the qualification you are looking for, the qualification may not be recognised as full and relevant.</p>" +
                       "<p class=\"govuk-body\">If a qualification is not recognised as full and relevant, the qualification holder can still work as an unqualified member of staff in an early years setting.</p>" +
                       "<p class=\"govuk-body\">Go to <a class=\"govuk-link\" href=\"/advice/qualification-not-on-the-list\">I cannot find the qualification</a> for more detail about what to do next.</p>",
                   // TODO: revert to `content.NoMatchingQualificationsHeading` once the field is added in Contentful
                   NoMatchingQualificationsHeading = "No matching qualifications were found",
                   ClearSearchText = content.ClearSearchText,
                   QualificationNumberLabel = content.QualificationNumberLabel,
                   SearchResults = MapQualificationsAndContentToSearchResultContentModel(basicQualificationsModels, content),
                   HasSearchCriteria = hasSearchCriteria,
                   SearchMatchHeading = searchMatchHeading,
                   SearchNoMatchGuidanceIntro = searchNoMatchGuidanceIntro,
                   SearchNoMatchGuidance = searchNoMatchGuidance,
        };
    }

    public FilterModel GetFilterModel(QualificationListPage content)
    {
        var countryAwarded = userJourneyCookieService.GetWhereWasQualificationAwarded()!;
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var (awardedDateMonth, awardedDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var level = userJourneyCookieService.GetLevelOfQualification();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();

        var filterModel = new FilterModel
                          {
                              Country = $"{content.AwardedLocationPrefixText} {countryAwarded}",
                              Level = content.AnyLevelHeading,
                              AwardingOrganisation =
                                  $"{content.AwardedByPrefixText} {content.AnyAwardingOrganisationHeading}"
                          };

        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);

            filterModel.StartDate = date < new DateOnly(2014, 9, 1) ? content.StartDateBeforeSept2014PrefixText : $"{content.StartDatePrefixText} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        if (awardedDateMonth is not null && awardedDateYear is not null)
        {
            var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
            filterModel.AwardedDate =
                $"{content.AwardedDatePrefixText} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {awardedDateYear.Value}";
        }

        if (level > 0)
        {
            filterModel.Level = $"{content.LevelPrefixText} {level}";
        }

        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            filterModel.AwardingOrganisation = $"{content.AwardedByPrefixText} {awardingOrganisation}";
        }

        return filterModel;
    }

    private static List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification> qualifications)
    {
        return qualifications.Select(qualification => 
            new BasicQualificationModel(qualification)
            {
                IsQualificationNameDuplicate = qualifications.Count(x => x.QualificationName.Equals(qualification.QualificationName, StringComparison.OrdinalIgnoreCase)) > 1
            }
        )
        .OrderBy(qualification => qualification.QualificationName)
        .ToList();
    }
    
    private static List<SearchResultContentModel> MapQualificationsAndContentToSearchResultContentModel(List<BasicQualificationModel> qualifications, QualificationListPage content)
    {
        return qualifications.Select(q => new SearchResultContentModel
                                                {
                                                    Qualification = q,
                                                    SearchResultContents = content.SearchResultsContent?.SingleOrDefault(x => x.QualificationId == q.QualificationId)?.AdditionalInformation
                                                                           ?? (q.IsQualificationNameDuplicate && !string.IsNullOrEmpty(q.QualificationNumber) 
                                                                                   ? $"{content.QualificationNumberLabel} {q.QualificationNumber}" 
                                                                                   : null)
                                                }).ToList();
    }
}