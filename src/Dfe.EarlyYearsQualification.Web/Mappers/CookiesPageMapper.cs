using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class CookiesPageMapper
{
    public static CookiesPageModel Map(CookiesPage content, string bodyContentHtml, string successBannerContentHtml)
    {
        return new CookiesPageModel
               {
                   Heading = content.Heading,
                   BodyContent = bodyContentHtml,
                   Options = content.Options
                                    .Select(x => new OptionModel { Label = x.Label, Value = x.Value, Hint = x.Hint })
                                    .ToList(),
                   ButtonText = content.ButtonText,
                   SuccessBannerContent = successBannerContentHtml,
                   SuccessBannerHeading = content.SuccessBannerHeading,
                   ErrorText = content.ErrorText,
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   FormHeading = content.FormHeading
               };
    }
}