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
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await contentParser.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   LevelHeading = content.LevelHeading,
                   AwardingOrganisationHeading = content.AwardingOrganisationHeading,
                   PostSearchCriteriaContent = await contentParser.ToHtml(content.PostSearchCriteriaContent),
                   PostQualificationListContent = await contentParser.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels.OrderBy(x => x.QualificationName).ToList(),
                   NoResultText = await contentParser.ToHtml(content.NoResultsText),
                   ClearSearchText = content.ClearSearchText,
                   NoQualificationsFoundText = content.NoQualificationsFoundText
               };
    }

    public FilterModel GetFilterModel(QualificationListPage content)
    {
        var countryAwarded = userJourneyCookieService.GetWhereWasQualificationAwarded()!;
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var level = userJourneyCookieService.GetLevelOfQualification();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();

        var filterModel = new FilterModel
                          {
                              Country = countryAwarded,
                              Level = content.AnyLevelHeading,
                              AwardingOrganisation = content.AnyAwardingOrganisationHeading
                          };

        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
            filterModel.StartDate = $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        if (level > 0)
        {
            filterModel.Level = $"Level {level}";
        }

        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            filterModel.AwardingOrganisation = awardingOrganisation;
        }

        return filterModel;
    }

    public List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification> qualifications)
    {
        var basicQualificationsModels = new List<BasicQualificationModel>();

        if (qualifications is not null && qualifications.Count > 0)
        {
            basicQualificationsModels.AddRange(
                                               qualifications.Select(
                                                                     qualification => new BasicQualificationModel
                                                                                      {
                                                                                          QualificationId = qualification.QualificationId,
                                                                                          QualificationLevel = qualification.QualificationLevel,
                                                                                          QualificationName = qualification.QualificationName,
                                                                                          AwardingOrganisationTitle = qualification.AwardingOrganisationTitle
                                                                                      }
                                                                    )
                                              );
        }

        return basicQualificationsModels;
    }
}