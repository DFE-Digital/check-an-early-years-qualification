using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class DateQuestionMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnModel()
    {
        var question = new DateQuestion
                       {
                           QuestionHint = "Hint",
                           QuestionHeader = "Question hint header",
                           MonthLabel = "Month label",
                           YearLabel = "Year label"
                       };
        const string errorBannerLinkText = "error banner link text";
        const string errorMessage = "error message";
        var dateValidationResult = new DateValidationResult { MonthValid = false, YearValid = false };
        const int selectedMonth = 2;
        const int selectedYear = 2016;

        var result = DateQuestionMapper.Map(new DateQuestionModel(), question, errorBannerLinkText, errorMessage,
                                            dateValidationResult, selectedMonth, selectedYear);

        result.Should().NotBeNull();
        result.MonthLabel.Should().BeSameAs(question.MonthLabel);
        result.YearLabel.Should().BeSameAs(question.YearLabel);
        result.ErrorMessage.Should().BeSameAs(errorMessage);
        result.QuestionHint.Should().BeSameAs(question.QuestionHint);
        result.QuestionHeader.Should().BeSameAs(question.QuestionHeader);
        result.MonthError.Should().BeTrue();
        result.YearError.Should().BeTrue();
        result.SelectedMonth.Should().NotBeNull();
        result.SelectedMonth!.Value.Should().Be(selectedMonth);
        result.SelectedYear.Should().NotBeNull();
        result.SelectedYear!.Value.Should().Be(selectedYear);
    }
}