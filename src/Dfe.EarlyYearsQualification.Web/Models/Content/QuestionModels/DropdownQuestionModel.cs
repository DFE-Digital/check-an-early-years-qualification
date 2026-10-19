using Dfe.EarlyYearsQualification.Web.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class DropdownQuestionModel : BaseQuestionModel
{
    [IncludeInTelemetry]
    public string? SelectedValue { get; set; } = string.Empty;

    public List<SelectListItem> Values { get; init; } = [];

    public string DropdownHeading { get; set; } = string.Empty;

    public string NotInListText { get; set; } = string.Empty;

    public string NotInListHintText { get; set; } = string.Empty;

    public string PageParagraph { get; set; } = string.Empty;

    public bool HasErrors { get; set; }

    public string DropdownId { get; init; } = "awarding-organisation-select";

    public string CheckboxId { get; init; } = "awarding-organisation-not-in-list";

    [IncludeInTelemetry]
    public bool NotInTheList { get; set; }

    public bool HasPageParagraph => !string.IsNullOrEmpty(PageParagraph);

    public bool HasNotInListHint => !string.IsNullOrEmpty(NotInListHintText);

    public string NotInListHintId => $"{CheckboxId}-hint";

    public IDictionary<string, object> CheckboxAttributes
    {
        get
        {
            var attributes = new Dictionary<string, object>
                             {
                                 { "class", "govuk-checkboxes__input" },
                                 { "id", CheckboxId },
                                 { "name", "awarding-organisation" }
                             };

            if (HasNotInListHint)
            {
                attributes["aria-describedby"] = NotInListHintId;
            }

            return attributes;
        }
    }
}