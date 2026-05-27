using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class OptionItemMapper
{
    public static List<IOptionItemModel> Map(List<IOptionItem> questionOptions)
    {
        var results = new List<IOptionItemModel>();

        foreach (var optionItem in questionOptions)
        {
            if (optionItem.GetType() == typeof(Option))
            {
                var option = (Option)optionItem;
                results.Add(Map(option));
            }

            if (optionItem.GetType() == typeof(Divider))
            {
                var divider = (Divider)optionItem;
                results.Add(new DividerModel { Text = divider.Text });
            }

            if (optionItem.GetType() == typeof(RadioButtonAndDateInput))
            {
                var content = (RadioButtonAndDateInput)optionItem;

                results.Add(
                            new RadioButtonAndDateInputModel
                            {
                                Label = content.Label,
                                Hint = content.Hint,
                                Value = content.Value,
                                Question = new DateQuestionModel()
                                           {
                                               MonthId = "Month",
                                               YearId = "Year",
                                               ErrorMessage = content.StartedQuestion.ErrorMessage,
                                               MonthLabel = content.StartedQuestion.MonthLabel,
                                               YearLabel = content.StartedQuestion.YearLabel,
                                               QuestionHeader = content.StartedQuestion.QuestionHeader,
                                               QuestionHint = content.StartedQuestion.QuestionHint,
                                               Prefix = "question",
                                               QuestionId = StringFormattingHelper.ToHtmlId(content.StartedQuestion.QuestionHeader),
                                           }
                            }
                           );
            }
        }

        return results;
    }

    public static OptionModel Map(Option option)
    {
        return new OptionModel { Hint = option.Hint, Value = option.Value, Label = option.Label };
    }
}