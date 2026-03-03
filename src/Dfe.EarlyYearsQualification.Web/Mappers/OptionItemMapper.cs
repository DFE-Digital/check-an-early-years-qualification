using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models;
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
                results.Add(new OptionModel { Hint = option.Hint, Value = option.Value, Label = option.Label });
            }
            if (optionItem.GetType() == typeof(Divider))
            {
                var divider = (Divider)optionItem;
                results.Add(new DividerModel { Text = divider.Text });
            }
            if (optionItem.GetType() == typeof(RadioButtonAndDateInput))
            {

                //todo

                var content = (RadioButtonAndDateInput)optionItem;

                var eee = new RadioButtonAndDateInputModel
                {
                    Label = content.Label,
                    Hint = content.Hint,
                    Value = content.Value,
                    StartedQuestion = new()
                    {
                        // ErrorSummaryLinks = divider.DateQuestion.ErrorSummaryLinks,
                        MonthId = "divider.DateQuestion.MonthId",
                        ErrorMessage = content.StartedQuestion.ErrorMessage,
                        MonthLabel = content.StartedQuestion.MonthLabel,
                        Prefix = "divider.DateQuestion.Prefix",
                        YearId = "divider.DateQuestion.YearId",
                        YearLabel = content.StartedQuestion.YearLabel,
                        QuestionHeader = content.StartedQuestion.QuestionHeader,
                        QuestionHint = content.StartedQuestion.QuestionHint,
                        QuestionId = "divider.DateQuestion.QuestionId",
                    }
                };

                results.Add(
                    new RadioButtonAndDateInputModel
                    {
                        Label = content.Label,
                        Hint = content.Hint,
                        Value = content.Value,
                        StartedQuestion = MapDate(eee.StartedQuestion, "started", "StartedQuestion").Item1
                    }
                );
            }
        }

        return results;
    }

    private static (DateQuestionModel, List<ErrorSummaryLink>) MapDate(DateQuestionModel dateQuestion, string prefix, string fieldName)
    {
        var errorLinks = new List<ErrorSummaryLink>();
        dateQuestion.Prefix = prefix;
        dateQuestion.QuestionId = $"date-{prefix}";
        dateQuestion.MonthId = $"{fieldName}.SelectedMonth";
        dateQuestion.YearId = $"{fieldName}.SelectedYear";

        foreach (var errorSummaryLink in dateQuestion.ErrorSummaryLinks)
        {
            errorSummaryLink.ElementLinkId = errorSummaryLink.ElementLinkId switch
            {
                nameof(FieldId.Month) => dateQuestion.MonthId,
                nameof(FieldId.Year) => dateQuestion.YearId,
                _ => errorSummaryLink.ElementLinkId
            };
        }

        if (dateQuestion.MonthError || dateQuestion.YearError)
        {
            errorLinks.AddRange(dateQuestion.ErrorSummaryLinks);
        }

        return (dateQuestion, errorLinks);
    }
}