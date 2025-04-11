using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models;

public class OptionsPageModel
{
    public const string DefaultOptionValue = "None";
    public const string BypassCacheOptionValue = "BypassCache";

    public string FormHeading { get; init; } = "Select an option";

    public List<OptionModel> Options { get; init; } =
        [
            new OptionModel
            {
                Label = "Normal operation",
                Hint = "Use published Contentful content with caching, just like in production",
                Value = DefaultOptionValue
            },
            new OptionModel
            {
                Label = "Bypass cache",
                Hint = "The service will always ask for the latest preview content, and not cache it",
                Value = BypassCacheOptionValue
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