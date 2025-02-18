using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

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
        var model = await MapList(qualificationListPage, filteredQualifications);
        return model;
    }

    public async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await qualificationsRepository.Get(
                                                  level,
                                                  startDateMonth,
                                                  startDateYear,
                                                  awardingOrganisation,
                                                  searchCriteria
                                                 );
    }

    public async Task<QualificationListModel> MapList(QualificationListPage content, List<Qualification>? qualifications)
    {
        var basicQualificationsModels = qualifications == null ? [] : GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel(content);

        return new QualificationListModel
               {
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   Filters = filterModel,
                   Header = content.Header,
                   QualificationFoundPrefixText = content.QualificationFoundPrefix,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await contentParser.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   PostQualificationListContent = await contentParser.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels,
                   NoResultText = await contentParser.ToHtml(content.NoResultsText),
                   ClearSearchText = content.ClearSearchText,
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
                              AwardingOrganisation = $"{content.AwardedByPrefixText} {content.AnyAwardingOrganisationHeading}"
                          };

        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
            filterModel.StartDate = $"{content.StartDatePrefixText} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        if (awardedDateMonth is not null && awardedDateYear is not null)
        {
            var date = new DateOnly(awardedDateYear.Value, awardedDateMonth.Value, 1);
            filterModel.AwardedDate = $"{content.AwardedDatePrefixText} {date.ToString("MMMM", CultureInfo.InvariantCulture)} {awardedDateYear.Value}";
        }
        if (level > 0)
        {
            filterModel.Level = $"level {level}";
        }

        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            filterModel.AwardingOrganisation = $"{content.AwardedByPrefixText} {awardingOrganisation}";
        }

        return filterModel;
    }

    private static List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification> qualifications)
    {
        return qualifications.Select(qualification => new BasicQualificationModel(qualification))
                             .OrderBy(qualification => qualification.QualificationName)
                             .ToList();
    }
}