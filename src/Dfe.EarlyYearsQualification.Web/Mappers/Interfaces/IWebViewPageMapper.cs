using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;

public interface IWebViewPageMapper
{
    Task<EarlyYearsQualificationListModel> Map(WebViewPage content, WebViewFilters webViewFilters, List<Qualification> qualifications);
}