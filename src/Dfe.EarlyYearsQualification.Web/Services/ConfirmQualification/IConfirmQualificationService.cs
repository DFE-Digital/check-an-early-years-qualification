using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.ConfirmQualification;

public interface IConfirmQualificationService
{
    public Task<ConfirmQualificationPage?> GetConfirmQualificationPageAsync();

    public Task<Qualification?> GetQualificationById(string qualificationId);

    public Task<List<Qualification>> GetFilteredQualifications(string? searchCriteriaOverride = null);

    Task<ConfirmQualificationPageModel> Map(ConfirmQualificationPage content, Qualification qualification, List<Qualification> qualifications);

    public HelpFormEnquiry GetHelpFormEnquiry();

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry);

    public void ValidSubmitSetCookieValues();

    public string SetHelpFormAwardingQualification(string title);
}