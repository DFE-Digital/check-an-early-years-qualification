using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class RadioQuestionMapper
{
    public static RadioQuestionModel Map(RadioQuestionModel model,
                                         RadioQuestionPage question,
                                         string actionName,
                                         string controllerName,
                                         string additionalInformationBodyHtml,
                                         string? selectedAnswer)
    {
        model.Question = question.Question;
        model.OptionsItems = MapOptionItems(question.Options);
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

    private static List<IOptionItemModel> MapOptionItems(List<IOptionItem> questionOptions)
    {
        var results = new List<IOptionItemModel>();

        foreach (var optionItem in questionOptions)
        {
            if (optionItem.GetType() == typeof(Option))
            {
                var option = (Option)optionItem;
                results.Add(new OptionModel { Hint = option.Hint, Value = option.Value, Label = option.Label });
            }
            else if (optionItem.GetType() == typeof(Divider))
            {
                var divider = (Divider)optionItem;
                results.Add(new DividerModel { Text = divider.Text });
            }
        }

        return results;
    }
}