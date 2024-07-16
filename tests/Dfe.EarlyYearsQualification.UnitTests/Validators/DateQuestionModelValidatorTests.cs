using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using FluentAssertions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Validators;

[TestClass]
public class DateQuestionModelValidatorTests
{
    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInRecentPast_ValidatesTrue()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2024, 7, 16, 13, 1, 12, DateTimeKind.Local));

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IDateTimeAdapter)))
                       .Returns(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };

        var context = new ValidationContext(model, serviceProvider.Object, null);

        DateQuestionModelValidator.IsValid(model, context).Should().Be(ValidationResult.Success);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateEarlierThisMonth_ValidatesTrue()
    {
        var thisYear = 2024;
        var thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 16, 13, 1, 12, DateTimeKind.Local));

        var serviceProvider = new Mock<IServiceProvider>();

        serviceProvider.Setup(x => x.GetService(typeof(IDateTimeAdapter)))
                       .Returns(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        var context = new ValidationContext(model, serviceProvider.Object, null);

        DateQuestionModelValidator.IsValid(model, context).Should().Be(ValidationResult.Success);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateThisMonthOnFirstOfMonth_ValidatesTrue()
    {
        var thisYear = 2024;
        var thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IDateTimeAdapter)))
                       .Returns(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth, SelectedYear = thisYear };

        var context = new ValidationContext(model, serviceProvider.Object, null);

        DateQuestionModelValidator.IsValid(model, context).Should().Be(ValidationResult.Success);
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateLaterThisYear_ValidatesFalse()
    {
        var thisYear = 2024;
        var thisMonth = 7;

        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now())
                       .Returns(new DateTime(thisYear, thisMonth, 1, 0, 0, 1, DateTimeKind.Local));

        var serviceProvicer = new Mock<IServiceProvider>();
        serviceProvicer.Setup(x => x.GetService(typeof(IDateTimeAdapter)))
                       .Returns(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = thisMonth + 1, SelectedYear = thisYear };

        var context = new ValidationContext(model, serviceProvicer.Object, null);

        var result = DateQuestionModelValidator.IsValid(model, context);

        result!.ErrorMessage.Should().Be("Month cannot be in the future");
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateInFuture_ValidatesFalse()
    {
        var dateTimeAdapter = new Mock<IDateTimeAdapter>();
        dateTimeAdapter.Setup(d => d.Now()).Returns(new DateTime(2022, 10, 10, 15, 32, 12, DateTimeKind.Local));

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IDateTimeAdapter)))
                       .Returns(dateTimeAdapter.Object);

        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2023 };

        var context = new ValidationContext(model, serviceProvider.Object, null);

        var result = DateQuestionModelValidator.IsValid(model, context);

        result!.ErrorMessage.Should().Be("Month cannot be in the future");
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenDateBefore1900_ValidatesFalse()
    {
        var model = new DateQuestionModel { SelectedMonth = 12, SelectedYear = 1899 };

        var context = new ValidationContext(model);

        var result = DateQuestionModelValidator.IsValid(model, context);

        result!.ErrorMessage.Should().Be("Year cannot be before 1900");
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthBefore1_ValidatesFalse()
    {
        var model = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 2024 };

        var context = new ValidationContext(model);

        var result = DateQuestionModelValidator.IsValid(model, context);

        result!.ErrorMessage.Should().Be("Month must be a number between 1 and 12");
    }

    [TestMethod]
    public void DateQuestionModelValidator_GivenMonthAfter12_ValidatesFalse()
    {
        var model = new DateQuestionModel { SelectedMonth = 13, SelectedYear = 2024 };

        var context = new ValidationContext(model);

        var result = DateQuestionModelValidator.IsValid(model, context);

        result!.ErrorMessage.Should().Be("Month must be a number between 1 and 12");
    }
}