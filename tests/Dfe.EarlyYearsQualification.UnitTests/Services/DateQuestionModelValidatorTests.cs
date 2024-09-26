using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class DateQuestionModelValidatorTests
{
    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInRecentPast_ValidatesTrue()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2024, 7, 16, 13, 1, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };
        var questionPage = new DateQuestionPage();

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateEarlierThisMonth_ValidatesTrue()
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 16, 13, 1, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        var questionPage = new DateQuestionPage();

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateThisMonthOnFirstOfMonth_ValidatesTrue()
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        var questionPage = new DateQuestionPage();

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateLaterThisYear_ValidatesFalse()
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth + 1, SelectedYear = thisYear };

        var questionPage = new DateQuestionPage();

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInFuture_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2022, 10, 10, 15, 32, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };

        var questionPage = new DateQuestionPage
                           {
                               FutureDateErrorMessage = "Future date error message",
                               FutureDateErrorBannerLinkText = "Future date banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Match(questionPage.FutureDateErrorMessage);
        result.BannerErrorMessage.Should().Match(questionPage.FutureDateErrorBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateBefore1900_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 12, SelectedYear = 1899 };

        var questionPage = new DateQuestionPage
                           {
                               IncorrectMonthFormatErrorMessage = "Incorrect format error message",
                               IncorrectMonthFormatErrorBannerLinkText = "Incorrect format banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorMessage);
        result.BannerErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthBefore1_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 2024 };

        var questionPage = new DateQuestionPage
                           {
                               IncorrectMonthFormatErrorMessage = "Incorrect format error message",
                               IncorrectMonthFormatErrorBannerLinkText = "Incorrect format banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorMessage);
        result.BannerErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthAfter12_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 13, SelectedYear = 2024 };

        var questionPage = new DateQuestionPage
                           {
                               IncorrectMonthFormatErrorMessage = "Incorrect format error message",
                               IncorrectMonthFormatErrorBannerLinkText = "Incorrect format banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorMessage);
        result.BannerErrorMessage.Should().Match(questionPage.IncorrectMonthFormatErrorBannerLinkText);
    }
    
    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthAndYearAre0_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 0 };

        var questionPage = new DateQuestionPage
                           {
                               ErrorMessage = "Generic error message",
                               ErrorBannerLinkText = "Generic banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Match(questionPage.ErrorMessage);
        result.BannerErrorMessage.Should().Match(questionPage.ErrorBannerLinkText);
    }
}