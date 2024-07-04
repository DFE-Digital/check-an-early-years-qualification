using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class DropdownQuestionModel : BaseQuestionModel
{
    public string SelectedValue { get; init; } = string.Empty;

    public List<SelectListItem> Values { get; init; } = [];

    public string DropdownHeading { get; set; } = string.Empty;

    public string NotInListText { get; set; } = string.Empty;

    public string DropdownId { get; init; } = "awarding-organisation-select";

    public string CheckboxId { get; init; } = "awarding-organisation-not-in-list";

    public bool NotInTheList { get; init; }
}