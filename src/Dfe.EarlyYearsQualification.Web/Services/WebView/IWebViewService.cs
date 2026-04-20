using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.WebView;

public interface IWebViewService
{
    public Task<WebViewPage?> GetWebViewPage();

    public Task<EarlyYearsQualificationListModel> MapWebViewPageContentToViewModelAsync(WebViewPage content);

    public Task<List<Qualification>> GetQualifications(WebViewFilters filters);

    public void SetWebViewFilters(WebViewFilters filters);

    public void ApplyFilters(EarlyYearsQualificationListModel model);

    public void RemoveFilter(string filter);
}