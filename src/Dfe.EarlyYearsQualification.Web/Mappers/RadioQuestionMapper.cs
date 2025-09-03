using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class RadioQuestionMapper(IGovUkContentParser contentParser) : IRadioQuestionMapper
{
    public async Task<RadioQuestionModel> Map(RadioQuestionModel model,
                                         RadioQuestionPage question,
                                         string actionName,
                                         string controllerName,
                                         string? selectedAnswer)
    {
        var additionalInformationBodyHtml = await contentParser.ToHtml(question.AdditionalInformationBody);
        
        model.Question = question.Question;
        model.OptionsItems = OptionItemMapper.Map(question.Options);
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = additionalInformationBodyHtml;
        model.BackButton = NavigationLinkMapper.Map(question.BackButton);
        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = question.ErrorBannerLinkText;
        model.Option = selectedAnswer ?? string.Empty;
        return model;
    }
}