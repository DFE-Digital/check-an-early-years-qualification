using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class PreCheckPageMapper(IGovUkContentParser contentParser) : IPreCheckPageMapper
{
    public async Task<PreCheckPageModel> Map(PreCheckPage preCheckPage)
    {
        var postHeaderContent = await contentParser.ToHtml(preCheckPage.PostHeaderContent);
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