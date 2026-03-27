using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class OptionsMapper
{
    public static List<OptionModel> Map(List<Option> options)
    {
        var results = new List<OptionModel>();

        foreach (var option in options)
        {
            results.Add(new OptionModel { Label = option.Label, Value = option.Value, Hint = option.Hint });
        }

        return results;
    }
}