using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QuestionModel
{
    public string Question { get; set; } = string.Empty;

    public List<OptionModel> Options { get; set; } = new List<OptionModel>();

    public string CtaButtonText { get; set; } = string.Empty;

    public string ActionName { get; set; } = string.Empty;

    public string ControllerName { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    [Required]
    public string? Option { get; set; } = string.Empty;

    public bool HasErrors {get; set; }
}

public class OptionModel
{
    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}