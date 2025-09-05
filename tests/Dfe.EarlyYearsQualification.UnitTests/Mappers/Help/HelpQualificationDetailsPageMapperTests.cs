using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers.Help;

[TestClass]
public class HelpQualificationDetailsPageMapperTests
{
    [TestMethod]
    public void MapQualificationDetailsContentToViewModel_MapsToViewModel()
    {
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var content = new HelpQualificationDetailsPage()
        {
            Heading = "What are the qualification details?",
            PostHeadingContent = "We need to know the following qualification details to quickly and accurately respond to any questions you may have.",
            CtaButtonText = "Continue",
            BackButton = new NavigationLink
            {
                DisplayText = "Back to get help with the Check an early years qualification service",
                Href = "/help/get-help",
                OpenInNewTab = false
            },
            QualificationNameHeading = "Qualification name",
            QualificationNameErrorMessage = "Enter the qualification name",
            AwardingOrganisationHeading = "Awarding organisation",
            AwardingOrganisationErrorMessage = "Enter the awarding organisation",
            ErrorBannerHeading = "There is a problem",
            AwardedDateIsAfterStartedDateErrorText = "The awarded date must be after the started date",
            StartDateQuestion = new DateQuestion
            {
                MonthLabel = "Month",
                YearLabel = "Year",
                QuestionHeader = "Start date",
                QuestionHint = null,
                ErrorBannerLinkText = "Enter the month and year that the qualification was started",
                ErrorMessage = "Enter the month and year that the qualification was started",
                FutureDateErrorBannerLinkText = "The date the qualification was started must be in the past",
                FutureDateErrorMessage = "The date the qualification was started must be in the past",
                MissingMonthErrorMessage = "Enter the month that the qualification was started",
                MissingYearErrorMessage = "Enter the year that the qualification was started",
                MissingMonthBannerLinkText = "Enter the month that the qualification was started",
                MissingYearBannerLinkText = "Enter the year that the qualification was started",
                MonthOutOfBoundsErrorLinkText = "The month the qualification was started must be between 1 and 12",
                MonthOutOfBoundsErrorMessage = "The month the qualification was started must be between 1 and 12",
                YearOutOfBoundsErrorLinkText = "The year the qualification was started must be between 1900 and $[actual-year]$",
                YearOutOfBoundsErrorMessage = "The year the qualification was started must be between 1900 and $[actual-year]$"
            },
            AwardedDateQuestion = new DateQuestion
            {
                MonthLabel = "Month",
                YearLabel = "Year",
                QuestionHeader = "Award date",
                QuestionHint = null,
                ErrorBannerLinkText = "Enter the month and year that the qualification was awarded",
                ErrorMessage = "Enter the date the qualification was awarded",
                FutureDateErrorBannerLinkText = "The date the qualification was awarded must be in the past",
                FutureDateErrorMessage = "The date the qualification was awarded must be in the past",
                MissingMonthErrorMessage = "Enter the month that the qualification was awarded",
                MissingYearErrorMessage = "Enter the year that the qualification was awarded",
                MissingMonthBannerLinkText = "Enter the month that the qualification was awarded",
                MissingYearBannerLinkText = "Enter the year that the qualification was awarded",
                MonthOutOfBoundsErrorLinkText = "The month the qualification was awarded must be between 1 and 12",
                MonthOutOfBoundsErrorMessage = "The month the qualification was awarded must be between 1 and 12",
                YearOutOfBoundsErrorLinkText = "The year the qualification was awarded must be between 1900 and $[actual-year]$",
                YearOutOfBoundsErrorMessage = "The year the qualification was awarded must be between 1900 and $[actual-year]$"
            },

        };

        var viewModel = new QualificationDetailsPageViewModel()
        {
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new DateQuestionModel()
                {
                    QuestionHeader = content.StartDateQuestion.QuestionHeader,
                    MonthLabel = content.StartDateQuestion.MonthLabel,
                    YearLabel = content.StartDateQuestion.YearLabel,
                    SelectedMonth = 1,
                    SelectedYear = 2000
                },
                AwardedQuestion = new DateQuestionModel()
                {
                    QuestionHeader = content.AwardedDateQuestion.QuestionHeader,
                    MonthLabel = content.AwardedDateQuestion.MonthLabel,
                    YearLabel = content.AwardedDateQuestion.YearLabel,
                    SelectedMonth = 1,
                    SelectedYear = 2002
                }
            }
        };

        var modelState = new ModelStateDictionary();

        var result = new HelpQualificationDetailsPageMapper(mockPlaceholderUpdater.Object).MapQualificationDetailsContentToViewModel(viewModel, content, null, modelState);

        result.Should().NotBeNull();

        result.AwardingOrganisationErrorMessage.Should().Be(content.AwardingOrganisationErrorMessage);
        result.AwardingOrganisationHeading.Should().Be(content.AwardingOrganisationHeading);
        result.BackButton.Should().BeEquivalentTo(new NavigationLinkModel
        {
            DisplayText = "Back to get help with the Check an early years qualification service",
            Href = "/help/get-help",
            OpenInNewTab = false
        });

        result.CtaButtonText.Should().Be("Continue");
        result.ErrorBannerHeading.Should().Be("There is a problem");
        result.Heading.Should().Be(content.Heading);
        result.PostHeadingContent.Should().Be(content.PostHeadingContent);
        result.QualificationNameErrorMessage.Should().Be(content.QualificationNameErrorMessage);
        result.QualificationNameHeading.Should().Be(content.QualificationNameHeading);
        result.HasAwardingOrganisationError.Should().BeFalse();
        result.HasQualificationNameError.Should().BeFalse();
        result.HasValidationErrors.Should().BeFalse();
        result.QuestionModel.Should().NotBeNull();

        result.QuestionModel.StartedQuestion.Should().NotBeNull();
        result.QuestionModel.StartedQuestion.QuestionHeader.Should().Be(content.StartDateQuestion.QuestionHeader);
        result.QuestionModel.StartedQuestion.QuestionHint.Should().BeNullOrEmpty();
        result.QuestionModel.StartedQuestion.MonthLabel.Should().Be(content.StartDateQuestion.MonthLabel);
        result.QuestionModel.StartedQuestion.YearLabel.Should().Be(content.StartDateQuestion.YearLabel);
        result.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(1);
        result.QuestionModel.StartedQuestion.SelectedYear.Should().Be(2000);

        result.QuestionModel.AwardedQuestion.Should().NotBeNull();
        result.QuestionModel.AwardedQuestion.QuestionHeader.Should().Be(content.AwardedDateQuestion.QuestionHeader);
        result.QuestionModel.AwardedQuestion.QuestionHint.Should().BeNullOrEmpty();
        result.QuestionModel.AwardedQuestion.MonthLabel.Should().Be(content.AwardedDateQuestion.MonthLabel);
        result.QuestionModel.AwardedQuestion.YearLabel.Should().Be(content.AwardedDateQuestion.YearLabel);
        result.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(1);
        result.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(2002);
    }
}