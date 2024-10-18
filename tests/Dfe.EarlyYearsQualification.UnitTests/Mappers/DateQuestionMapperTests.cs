using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class DateQuestionMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnModel()
    {
        var question = new DateQuestionPage
                       {
                           Question = "Question",
                           CtaButtonText = "Button Text",
                           QuestionHint = "Hint",
                           MonthLabel = "Month label",
                           YearLabel = "Year label",
                           BackButton = new NavigationLink
                                        {
                                            DisplayText = "Back",
                                            OpenInNewTab = true,
                                            Href = "/"
                                        },
                           ErrorBannerHeading = "Error banner heading",
                           AdditionalInformationHeader = "Additional information header"
                       };
        const string actionName = "action";
        const string controllerName = "controller";
        const string errorBannerLinkText = "error banner link text";
        const string errorMessage = "error message";
        const string additionalInformationBody = "additional information body";
        var dateValidationResult = new DateValidationResult { MonthValid = false, YearValid = false };
        const int selectedMonth = 2;
        const int selectedYear = 2016;

        var result = DateQuestionMapper.Map(new DateQuestionModel(), question, actionName, controllerName,
                                            errorBannerLinkText, errorMessage, additionalInformationBody,
                                            dateValidationResult, selectedMonth, selectedYear);

        result.Should().NotBeNull();
        result.Question.Should().BeSameAs(question.Question);
        result.CtaButtonText.Should().BeSameAs(question.CtaButtonText);
        result.ActionName.Should().BeSameAs(actionName);
        result.ControllerName.Should().BeSameAs(controllerName);
        result.MonthLabel.Should().BeSameAs(question.MonthLabel);
        result.YearLabel.Should().BeSameAs(question.YearLabel);
        result.BackButton.Should().BeEquivalentTo(question.BackButton, options => options.Excluding(x => x.Sys));
        result.ErrorBannerHeading.Should().BeSameAs(question.ErrorBannerHeading);
        result.ErrorBannerLinkText.Should().BeSameAs(errorBannerLinkText);
        result.ErrorMessage.Should().BeSameAs(errorMessage);
        result.AdditionalInformationHeader.Should().BeSameAs(question.AdditionalInformationHeader);
        result.AdditionalInformationBody.Should().BeSameAs(additionalInformationBody);
        result.MonthError.Should().BeTrue();
        result.YearError.Should().BeTrue();
        result.SelectedMonth.Should().NotBeNull();
        result.SelectedMonth!.Value.Should().Be(selectedMonth);
        result.SelectedYear.Should().NotBeNull();
        result.SelectedYear!.Value.Should().Be(selectedYear);
    }
}