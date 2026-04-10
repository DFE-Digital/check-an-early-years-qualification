using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.WebView;

public class WebViewService(
    IContentService contentService,
    IWebViewPageMapper webViewPageMapper,
    IUserJourneyCookieService userJourneyCookieService,
    IQualificationsRepository qualificationsRepository

) : ServiceController, IWebViewService
{
    public async Task<WebViewPage?> GetWebViewPage()
    {
        return await contentService.GetWebViewPage();
    }

    public async Task<EarlyYearsQualificationListModel> MapWebViewPageContentToViewModelAsync(WebViewPage content)
    {
        var filters = GetWebViewFilters();

        var allQualifications = await GetQualifications(filters);

        return await webViewPageMapper.Map(content, filters, allQualifications);
    }

    public async Task<List<Qualification>> GetQualifications(WebViewFilters filters)
    {
        var searchCriteria = string.IsNullOrWhiteSpace(filters.SearchTerm) ? null : filters.SearchTerm;
        var qualificationLevel = GetQualificationLevel(filters.QualificationLevel);

        var qualifications = await qualificationsRepository.Get(
                                                  qualificationLevel,
                                                  null,
                                                  null,
                                                  null,
                                                  searchCriteria
                                                 );

        qualifications = FilterQualificationsByStartDate(qualifications, filters.QualificationStartDate);

        return [..qualifications
            .OrderBy(x => x.QualificationLevel)
            .ThenBy(x => x.QualificationName)];
    }

    public void ApplyFilters(EarlyYearsQualificationListModel model)
    {
        var filters = GetWebViewFilters();
        filters.SearchTerm = model.SearchTermFilter;
        filters.QualificationStartDate = model.QualificationStartDateFilter;
        filters.QualificationLevel = model.QualificationLevelFilter;
        SetWebViewFilters(filters);
    }

    public void RemoveFilter(string filter)
    {
        var filters = GetWebViewFilters();

        if (filter.Contains("qualification-level"))
        {
            filters.QualificationLevel = string.Empty;
        }
        if (filter.Contains("start-date"))
        {
            filters.QualificationStartDate = string.Empty;
        }
        if (filter.Contains("search-term"))
        {
            filters.SearchTerm = string.Empty;
        }

        SetWebViewFilters(filters);
    }

    public void SetWebViewFilters(WebViewFilters filters)
    {
        userJourneyCookieService.SetWebViewFilters(filters);
    }

    private WebViewFilters GetWebViewFilters()
    {
        return userJourneyCookieService.GetWebViewFilters();
    }

    private static int? GetQualificationLevel(string qualificationLevel)
    {
        if (!string.IsNullOrWhiteSpace(qualificationLevel) && int.TryParse(qualificationLevel, out var parsedQualificationLevel))
        {
            return parsedQualificationLevel;
        }

        return null;
    }

    private static List<Qualification> FilterQualificationsByStartDate(List<Qualification> qualifications, string startDate)
    {
        if (!string.IsNullOrEmpty(startDate))
        {
            return [.. qualifications.Where(q => q.EyqlTabs?.Select(t => t.Heading).Contains(startDate) == true)];
        }

        return qualifications;
    }
}