using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class OptionsPageModel
{
    private const string DefaultOptionValue = "Normal";
    public string FormHeading { get; init; } = "Select an option";

    public List<OptionModel> Options { get; init; } =
        [
            new()
            {
                Label = "Normal operation",
                Hint = "Use the cache",
                Value = DefaultOptionValue
            },
            new()
            {
                Label = "Bypass cache",
                Hint = "The service will always ask for the latest content",
                Value = "BypassCache"
            }
        ];

    [Required]
    public string Option { get; set; } = DefaultOptionValue;

    public string? ButtonText { get; init; } = "Select option and continue";

    public static string OptionAnswer
    {
        get { return "OptionAnswer"; }
    }
}