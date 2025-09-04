using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class ChallengePageMapper(IGovUkContentParser contentParser) : IChallengePageMapper
{
    public async Task<ChallengePageModel> Map(ChallengePageModel model, ChallengePage content,
                                         string sanitisedReferralAddress)
    {
        var footerContentHtml = await contentParser.ToHtml(content.FooterContent);
        var mainContentHtml = await contentParser.ToHtml(content.MainContent);
        
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