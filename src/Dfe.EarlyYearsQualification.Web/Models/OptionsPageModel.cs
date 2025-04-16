using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models;

public class OptionsPageModel
{
    public const string PublishedOptionValue = "Published"; // default
    public const string PreviewOptionValue = "Preview";
    private string _option = PublishedOptionValue;

    public string FormHeading { get; init; } = "Select an option";

    public List<OptionModel> Options { get; init; } =
        [
            new OptionModel
            {
                Label = "Published",
                Hint = "Use published Contentful content with caching, just like in production",
                Value = PublishedOptionValue
            },
            new OptionModel
            {
                Label = "Preview",
                Hint =
                    "The service will always ask for the latest preview content from Contentful, and not use the cache",
                Value = PreviewOptionValue
            }
        ];

    [Required]
    public string Option
    {
        get { return _option; }
        set
        {
            _option = value;

            if (value != PreviewOptionValue)
            {
                _option = PublishedOptionValue;
            }
        }
    }

    public string OptionShortText
    {
        get
        {
            return Option switch
                   {
                       PublishedOptionValue => "Published content",
                       PreviewOptionValue => "Preview content",
                       _ => "?"
                   };
        }
    }

    public string? ButtonText { get; init; } = "Select option and continue";

    public static string OptionAnswer
    {
        get { return "OptionAnswer"; }
    }
}