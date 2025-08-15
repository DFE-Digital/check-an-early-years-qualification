using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormQuestionRadioModel : BaseFeedbackFormQuestionModel
{
    public List<IOptionItemModel> OptionsItems { get; init; } = [];
}