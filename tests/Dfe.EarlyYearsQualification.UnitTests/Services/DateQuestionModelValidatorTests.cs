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

        validator.IsValid(model).Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateEarlierThisMonth_ValidatesTrue()
    {
        var thisYear = 2024;
        var thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 16, 13, 1, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        validator.IsValid(model).Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateThisMonthOnFirstOfMonth_ValidatesTrue()
    {
        var thisYear = 2024;
        var thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        validator.IsValid(model).Should().BeTrue();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInFuture_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2022, 10, 10, 15, 32, 12, DateTimeKind.Local));

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };

        validator.IsValid(model).Should().BeFalse();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateBefore1900_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 12, SelectedYear = 1899 };

        validator.IsValid(model).Should().BeFalse();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthBefore1_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 2024 };

        validator.IsValid(model).Should().BeFalse();
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthAfter12_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();

        var validator = new DateQuestionModelValidator(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 13, SelectedYear = 2024 };

        validator.IsValid(model).Should().BeFalse();
    }
}