using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

public class RadioQuestionModel : BaseQuestionModel
{
    public List<IOptionItemModel> OptionsItems { get; set; } = [];
    
    public bool HasErrors { get; set; }

    [Required] public string Option { get; set; } = string.Empty;
}