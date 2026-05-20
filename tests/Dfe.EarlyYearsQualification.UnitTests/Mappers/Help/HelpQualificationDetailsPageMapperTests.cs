using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpQualificationDetailsPageMapperTests
{
    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsBasicFieldsCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.Should().NotBeNull();
        result.Heading.Should().Be("What are the qualification details?");
        result.PostHeadingContent.Should().Be("We need to know the following qualification details.");
        result.CtaButtonText.Should().Be("Continue");
        result.QualificationNameHeading.Should().Be("Qualification name");
        result.QualificationNameErrorMessage.Should().Be("Enter the qualification name");
        result.AwardingOrganisationHeading.Should().Be("Awarding organisation");
        result.AwardingOrganisationErrorMessage.Should().Be("Enter the awarding organisation");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.MissingStartedDateOptionErrorMessage.Should().Be("Select when you started the qualification");
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsBackButtonCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.BackButton.Should().NotBeNull();
        result.BackButton!.DisplayText.Should().Be("Back to get help");
        result.BackButton.Href.Should().Be("/help/get-help");
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsBefore2014OptionCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.Before2014Option.Should().NotBeNull();
        result.Before2014Option.Value.Should().Be("Before1September2014");
        result.Before2014Option.Label.Should().Be("Before 1 September 2014");
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsAfter2014RadioButtonCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.RadioButtonWithDateInputModel.Should().NotBeNull();
        result.RadioButtonWithDateInputModel.Value.Should().Be("OnOrAfter1September2014");
        result.RadioButtonWithDateInputModel.Label.Should().Be("On or after 1 September 2014");
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsStartedDateQuestionCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        var startedQuestion = result.RadioButtonWithDateInputModel.Question;
        startedQuestion.Should().NotBeNull();
        startedQuestion.QuestionHeader.Should().Be("Start date");
        startedQuestion.QuestionHint.Should().Be("For example, 09 2020");
        startedQuestion.MonthLabel.Should().Be("Month");
        startedQuestion.YearLabel.Should().Be("Year");
        startedQuestion.Prefix.Should().Be("started");
        startedQuestion.QuestionId.Should().Be("date-started");
        startedQuestion.MonthId.Should().Be("RadioButtonWithDateInputModel.Question.SelectedMonth");
        startedQuestion.YearId.Should().Be("RadioButtonWithDateInputModel.Question.SelectedYear");
        startedQuestion.ErrorMessage.Should().Be("Enter the month and year that the qualification was started");
        startedQuestion.DisplayHeader.Should().BeFalse();
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_PreservesSubmittedStartedDateValues()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel
        {
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel
                {
                    SelectedMonth = 9,
                    SelectedYear = 2020
                }
            }
        };

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.RadioButtonWithDateInputModel.Question.SelectedMonth.Should().Be(9);
        result.RadioButtonWithDateInputModel.Question.SelectedYear.Should().Be(2020);
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsAwardedDateQuestionCorrectly()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.AwardedDate.Should().NotBeNull();
        result.AwardedDate.QuestionHeader.Should().Be("Award date");
        result.AwardedDate.QuestionHint.Should().Be("For example, 12 2022");
        result.AwardedDate.MonthLabel.Should().Be("Month");
        result.AwardedDate.YearLabel.Should().Be("Year");
        result.AwardedDate.Prefix.Should().Be("awarded");
        result.AwardedDate.QuestionId.Should().Be("date-awarded");
        result.AwardedDate.MonthId.Should().Be("AwardedDate.SelectedMonth");
        result.AwardedDate.YearId.Should().Be("AwardedDate.SelectedYear");
        result.AwardedDate.ErrorMessage.Should().Be("Enter the date the qualification was awarded");
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_PreservesSubmittedAwardedDateValues()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel
        {
            AwardedDate = new DateQuestionModel
            {
                SelectedMonth = 12,
                SelectedYear = 2022
            }
        };

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.AwardedDate.SelectedMonth.Should().Be(12);
        result.AwardedDate.SelectedYear.Should().Be(2022);
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_HandlesNullSelectedDates()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.RadioButtonWithDateInputModel.Question.SelectedMonth.Should().BeNull();
        result.RadioButtonWithDateInputModel.Question.SelectedYear.Should().BeNull();
        result.AwardedDate.SelectedMonth.Should().BeNull();
        result.AwardedDate.SelectedYear.Should().BeNull();
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_ReturnsOriginalModelReference()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.Should().BeSameAs(model);
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_PreservesBothStartedAndAwardedDates()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel
        {
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel
                {
                    SelectedMonth = 6,
                    SelectedYear = 2018
                }
            },
            AwardedDate = new DateQuestionModel
            {
                SelectedMonth = 3,
                SelectedYear = 2021
            }
        };

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.RadioButtonWithDateInputModel.Question.SelectedMonth.Should().Be(6);
        result.RadioButtonWithDateInputModel.Question.SelectedYear.Should().Be(2018);
        result.AwardedDate.SelectedMonth.Should().Be(3);
        result.AwardedDate.SelectedYear.Should().Be(2021);
    }

    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_SetsDisplayHeaderToFalseForStartedDate()
    {
        var content = GetHelpQualificationDetailsPageContent();
        var model = new QualificationDetailsPageViewModel();

        var result = new HelpQualificationDetailsPageMapper().MapQualificationDetailsContentToViewModel(model, content);

        result.RadioButtonWithDateInputModel.Question.DisplayHeader.Should().BeFalse();
    }

    private static HelpQualificationDetailsPage GetHelpQualificationDetailsPageContent()
    {
        return new HelpQualificationDetailsPage
        {
            Heading = "What are the qualification details?",
            PostHeadingContent = "We need to know the following qualification details.",
            CtaButtonText = "Continue",
            BackButton = new NavigationLink
            {
                DisplayText = "Back to get help",
                Href = "/help/get-help",
                OpenInNewTab = false
            },
            QualificationNameHeading = "Qualification name",
            QualificationNameErrorMessage = "Enter the qualification name",
            AwardingOrganisationHeading = "Awarding organisation",
            AwardingOrganisationErrorMessage = "Enter the awarding organisation",
            ErrorBannerHeading = "There is a problem",
            MissingStartedDateOptionErrorMessage = "Select when you started the qualification",
            BeforeSeptember2014Option = new Option
            {
                Value = "Before1September2014",
                Label = "Before 1 September 2014",
            },
            AfterSeptember2014Option = new RadioButtonAndDateInput
            {
                Value = "OnOrAfter1September2014",
                Label = "On or after 1 September 2014",
                StartedQuestion = new DateQuestion
                {
                    MonthLabel = "Month",
                    YearLabel = "Year",
                    QuestionHeader = "Start date",
                    QuestionHint = "For example, 09 2020",
                    ErrorMessage = "Enter the month and year that the qualification was started"
                }
            },
            AwardedDateQuestion = new DateQuestion
            {
                MonthLabel = "Month",
                YearLabel = "Year",
                QuestionHeader = "Award date",
                QuestionHint = "For example, 12 2022",
                ErrorMessage = "Enter the date the qualification was awarded"
            }
        };
    }
}