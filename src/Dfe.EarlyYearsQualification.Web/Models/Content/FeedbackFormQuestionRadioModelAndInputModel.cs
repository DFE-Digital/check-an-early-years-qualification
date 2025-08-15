using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormQuestionRadioAndInputModel : BaseFeedbackFormQuestionModel
{
    public List<IOptionItemModel> OptionsItems { get; init; } = [];

    public string InputHeading { get; set; } = string.Empty;

    public string InputHeadingHintText { get; set; } = string.Empty;

    public string ErrorMessageForInput { get; set; } = string.Empty;
}