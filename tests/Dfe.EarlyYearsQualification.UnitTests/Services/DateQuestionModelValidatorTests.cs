using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class DateQuestionModelValidatorTests
{
    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthAndYearAreNull_ValidatesFalse()
    {
        const string questionId = "questionId";
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(2024, 7, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = null, SelectedYear = null, QuestionId = questionId };

        var questionPage = new DateQuestion
                           {
                               ErrorMessage = "Missing month and year error message",
                               ErrorBannerLinkText = "Missing month and year banner link text"
                           };

        var result = validator.IsValid(model, questionPage);

        model.QuestionId.Should().Be(questionId);
        result.MonthValid.Should().BeFalse();
        result.YearValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle(questionPage.ErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.ErrorBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthIsNull_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(2024, 7, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = null, SelectedYear = 2024 };

        var questionPage = new DateQuestion
                           {
                               MissingMonthErrorMessage = "Missing month error message",
                               MissingMonthBannerLinkText = "Missing month banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeFalse();
        result.YearValid.Should().BeTrue();
        result.ErrorMessages.Should().ContainSingle(questionPage.MissingMonthErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.MissingMonthBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenYearIsNull_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(2024, 7, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 12, SelectedYear = null };

        var questionPage = new DateQuestion
                           {
                               MissingYearErrorMessage = "Missing year error message",
                               MissingYearBannerLinkText = "Missing year banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeTrue();
        result.YearValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle(questionPage.MissingYearErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.MissingYearBannerLinkText);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(13)]
    [DataRow(999)]
    public void DateQuestionModelValidator_GivenMonthIsBefore1OrAfter12_ValidatesFalse(int selectedMonth)
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(2024, 7, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = selectedMonth, SelectedYear = 2024 };

        var questionPage = new DateQuestion
                           {
                               MonthOutOfBoundsErrorMessage = "Month out of bounds error message",
                               MonthOutOfBoundsErrorLinkText = "Month out of bounds error link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeFalse();
        result.YearValid.Should().BeTrue();
        result.ErrorMessages.Should().ContainSingle(questionPage.MonthOutOfBoundsErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.MonthOutOfBoundsErrorLinkText);
    }

    [TestMethod]
    [DataRow(1899)]
    [DataRow(1)]
    [DataRow(2025)]
    [DataRow(9999)]
    public void DateQuestionModelValidator_GivenYearBefore1900OrAfterCurrentYear_ValidatesFalse(int selectedYear)
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = selectedYear };

        var questionPage = new DateQuestion
                           {
                               YearOutOfBoundsErrorMessage = "Year out of bounds error message",
                               YearOutOfBoundsErrorLinkText = "Year out of bounds error link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeTrue();
        result.YearValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle(questionPage.YearOutOfBoundsErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.YearOutOfBoundsErrorLinkText);
    }

    [TestMethod]
    [DataRow(8, 2024)]
    [DataRow(12, 2024)]
    public void DateQuestionModelValidator_GivenDateLaterThanCurrentMonthButSameYear_ValidatesFalse(int selectedMonth, int selectedYear)
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = selectedMonth, SelectedYear = selectedYear };

        var questionPage = new DateQuestion
                           {
                               FutureDateErrorMessage = "Future date error message",
                               FutureDateErrorBannerLinkText = "Future date error banner link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeFalse();
        result.YearValid.Should().BeFalse();
        result.ErrorMessages.Should().ContainSingle(questionPage.FutureDateErrorMessage);
        result.BannerErrorMessages.Should().ContainSingle(questionPage.FutureDateErrorBannerLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_BothMonthAndYearAreInvalid_ValidatesFalse()
    {
        const int thisYear = 2024;
        const int thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 20 };

        var questionPage = new DateQuestion
                           {
                               MonthOutOfBoundsErrorMessage = "Month out of bounds error message",
                               MonthOutOfBoundsErrorLinkText = "Month out of bounds error link text",
                               YearOutOfBoundsErrorMessage = "Year out of bounds error message",
                               YearOutOfBoundsErrorLinkText = "Year out of bounds error link text"
                           };

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeFalse();
        result.YearValid.Should().BeFalse();
        result.ErrorMessages.Should().Contain(questionPage.MonthOutOfBoundsErrorMessage);
        result.ErrorMessages.Should().Contain(questionPage.YearOutOfBoundsErrorMessage);
        result.BannerErrorMessages.Should().Contain(questionPage.MonthOutOfBoundsErrorLinkText);
        result.BannerErrorMessages.Should().Contain(questionPage.YearOutOfBoundsErrorLinkText);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInRecentPast_ValidatesTrue()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2024, 7, 16, 13, 1, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };
        var questionPage = new DateQuestion();

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeTrue();
        result.YearValid.Should().BeTrue();
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

        var questionPage = new DateQuestion();

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeTrue();
        result.YearValid.Should().BeTrue();
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

        var questionPage = new DateQuestion();

        var result = validator.IsValid(model, questionPage);
        result.MonthValid.Should().BeTrue();
        result.YearValid.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(9, 2014, 1, 2025, true)]
    [DataRow(1, 2019, 12, 2020, true)]
    [DataRow(1, 2025, 12, 2024, false)]
    [DataRow(1, 2025, 12, 2020, false)]
    [DataRow(1, 25, 12, 20, false)]
    public void IsAwardedDateAfterStartedDate_GoodDates_ReturnsExpected(int startedMonth, int startedYear, int awardedMonth, int awardedYear, bool expectedResult)
    {
        var startedDate = new DateQuestionModel
                          {
                              SelectedMonth = startedMonth,
                              SelectedYear = startedYear
                          };

        var awardedDate = new DateQuestionModel
                          {
                              SelectedMonth = awardedMonth,
                              SelectedYear = awardedYear
                          };
        var validator = new DateQuestionModelValidator(new Mock<IDateTimeAdapter>().Object);

        validator.IsAwardedDateAfterStartDate(startedDate, awardedDate).Should().Be(expectedResult);
    }
}