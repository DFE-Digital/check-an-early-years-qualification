using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpQualificationDetailsPageMapper : IHelpQualificationDetailsPageMapper
{
    public QualificationDetailsPageViewModel MapQualificationDetailsContentToViewModel(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage content)
    {
        var startedDateSubmittedMonth = model.RadioButtonWithDateInputModel.Question.SelectedMonth;
        var startedDateSubmittedYear = model.RadioButtonWithDateInputModel.Question.SelectedYear;

        // map content to page
        model.BackButton = new()
        {
            DisplayText = content.BackButton.DisplayText,
            Href = content.BackButton.Href
        };
        model.Heading = content.Heading;
        model.PostHeadingContent = content.PostHeadingContent;
        model.CtaButtonText = content.CtaButtonText;
        model.QualificationNameHeading = content.QualificationNameHeading;
        model.QualificationNameErrorMessage = content.QualificationNameErrorMessage;
        model.AwardingOrganisationHeading = content.AwardingOrganisationHeading;
        model.AwardingOrganisationErrorMessage = content.AwardingOrganisationErrorMessage;
        model.ErrorBannerHeading = content.ErrorBannerHeading;
        model.MissingStartedDateOptionErrorMessage = content.MissingStartedDateOptionErrorMessage;
        model.Before2014Option = OptionItemMapper.Map(content.BeforeSeptember2014Option);

        var started = MapDateContentToDateQuestionModel(new DateQuestionModel(), content.AfterSeptember2014Option.StartedQuestion, "started", "date-started", "RadioButtonWithDateInputModel.Question.SelectedMonth", "RadioButtonWithDateInputModel.Question.SelectedYear");
        started.SelectedMonth = startedDateSubmittedMonth;
        started.SelectedYear = startedDateSubmittedYear;
        started.DisplayHeader = false;
        model.RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
        {
            Value = content.AfterSeptember2014Option.Value,
            Label = content.AfterSeptember2014Option.Label,
            Hint = content.AfterSeptember2014Option.Hint,
            Question = started
        };

        model.AwardedDate = MapDateContentToDateQuestionModel(model.AwardedDate, content.AwardedDateQuestion, "awarded", "date-awarded", "AwardedDate.SelectedMonth", "AwardedDate.SelectedYear");

        return model;
    }

    private static DateQuestionModel MapDateContentToDateQuestionModel(DateQuestionModel model, DateQuestion content, string prefix, string questionId, string monthId, string yearId)
    {
        model.QuestionHeader = content.QuestionHeader;
        model.QuestionHint = content.QuestionHint;
        model.MonthLabel = content.MonthLabel;
        model.YearLabel = content.YearLabel;
        model.Prefix = prefix;
        model.QuestionId = questionId;
        model.MonthId = monthId;
        model.YearId = yearId;
        model.MonthLabel = content.MonthLabel;
        model.YearLabel = content.YearLabel;
        model.QuestionHint = content.QuestionHint;
        model.ErrorMessage = content.ErrorMessage;
        model.QuestionHeader = content.QuestionHeader;

        return model;
    }
}