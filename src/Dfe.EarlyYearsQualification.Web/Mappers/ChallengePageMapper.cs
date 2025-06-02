using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class ChallengePageMapper
{
    public static ChallengePageModel Map(ChallengePageModel model, ChallengePage content,
                                         string sanitisedReferralAddress, string footerContentHtml,
                                         string mainContentHtml)
    {
        model.RedirectAddress = sanitisedReferralAddress;
        model.FooterContent = footerContentHtml;
        model.InputHeading = content.InputHeading;
        model.MainContent = mainContentHtml;
        model.MainHeading = content.MainHeading;
        model.SubmitButtonText = content.SubmitButtonText;
        model.ShowPasswordButtonText = content.ShowPasswordButtonText;

        return model;
    }
}