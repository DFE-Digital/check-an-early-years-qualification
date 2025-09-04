using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class CookiesPageMapper(IGovUkContentParser contentParser) : ICookiesPageMapper
{
    public async Task<CookiesPageModel> Map(CookiesPage content)
    {
        var bodyContent = await contentParser.ToHtml(content.Body);
        var successBannerContent = await contentParser.ToHtml(content.SuccessBannerContent);
        return new CookiesPageModel
               {
                   Heading = content.Heading,
                   BodyContent = bodyContent,
                   Options = content.Options
                                    .Select(x => new OptionModel { Label = x.Label, Value = x.Value, Hint = x.Hint })
                                    .ToList(),
                   ButtonText = content.ButtonText,
                   SuccessBannerContent = successBannerContent,
                   SuccessBannerHeading = content.SuccessBannerHeading,
                   ErrorText = content.ErrorText,
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   FormHeading = content.FormHeading
               };
    }
}