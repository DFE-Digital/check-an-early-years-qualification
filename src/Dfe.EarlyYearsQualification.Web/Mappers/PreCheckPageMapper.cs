using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class PreCheckPageMapper
{
    public static PreCheckPageModel Map(PreCheckPage preCheckPage, string postHeaderContent)
    {
        return new PreCheckPageModel
               {
                   Header = preCheckPage.Header,
                   PostHeaderContent = postHeaderContent,
                   BackButton = NavigationLinkMapper.Map(preCheckPage.BackButton),
                   Question = preCheckPage.Question,
                   OptionsItems = OptionItemMapper.Map(preCheckPage.Options),
                   InformationMessage = preCheckPage.InformationMessage,
                   CtaButtonText = preCheckPage.CtaButtonText,
                   ErrorBannerHeading = preCheckPage.ErrorBannerHeading,
                   ErrorMessage = preCheckPage.ErrorMessage,
               };
    } 
}