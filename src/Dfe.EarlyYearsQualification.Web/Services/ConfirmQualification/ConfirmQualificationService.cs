using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.Web.Services.ConfirmQualification;

public class ConfirmQualificationService(
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IConfirmQualificationPageMapper confirmQualificationPageMapper,
    IQualificationSearchService qualificationSearchService
) : IConfirmQualificationService
{
    public async Task<ConfirmQualificationPage?> GetConfirmQualificationPageAsync()
    {
        return await contentService.GetConfirmQualificationPage();
    }

    public async Task<List<Qualification>> GetFilteredQualifications()
    {
        return await qualificationSearchService.GetFilteredQualifications();
    }

    public HelpFormEnquiry GetHelpFormEnquiry()
    {
        return userJourneyCookieService.GetHelpFormEnquiry();
    }

    public Qualification? GetQualificationById(List<Qualification> qualifications, string qualificationId)
    {
        return qualifications.SingleOrDefault(x => x.QualificationId.Equals(qualificationId, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ConfirmQualificationPageModel> Map(ConfirmQualificationPage content, Qualification qualification, List<Qualification> qualifications)
    {
        return await confirmQualificationPageMapper.Map(content, qualification, qualifications);
    }

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry)
    {
        userJourneyCookieService.SetHelpFormEnquiry(formEnquiry);
    }

    public string? GetAwardingOrganisation()
    {
        return userJourneyCookieService.GetAwardingOrganisation();
    }

    public void ValidSubmitSetCookieValues()
    {
        userJourneyCookieService.SetUserSelectedQualificationFromList(YesOrNo.Yes);
        userJourneyCookieService.ClearAdditionalQuestionsAnswers();
    }

    public string SetHelpFormAwardingQualification(string qualificationTitle)
    {
        var awardingOrgFromDropdown = userJourneyCookieService.GetAwardingOrganisation();

        // awardingOrgFromDropdown will be null if selected "Not in list"
        awardingOrgFromDropdown = awardingOrgFromDropdown ?? string.Empty;

        if (awardingOrgFromDropdown == string.Empty && qualificationTitle != AwardingOrganisations.Various)
        {
            return qualificationTitle;
        }

        return awardingOrgFromDropdown;
    }
}