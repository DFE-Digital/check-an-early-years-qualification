using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class RadioButtonAndDateInputModel : OptionModel, IOptionItemModel
{
    public DateQuestionModel? StartedQuestion { get; set; }
}