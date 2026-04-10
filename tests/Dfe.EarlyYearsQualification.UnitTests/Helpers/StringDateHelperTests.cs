using Dfe.EarlyYearsQualification.Web.Helpers;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class StringDateHelperTests
{
    [TestMethod]
    [DataRow("6/2001")]
    [DataRow("12/2022")]
    public void SplitDate_IntoMonthAndYear_ReturnsInts(string date)
    {
        // Act
        var split = StringDateHelper.SplitDate(date);

        // Assert
        split.startMonth.Should().Be(int.Parse(date.Split('/')[0]));
        split.startYear.Should().Be(int.Parse(date.Split('/')[1]));
    }

    [TestMethod]
    [DataRow("25")]
    [DataRow("26/03/2000")]
    [DataRow("Text/Text")]
    public void SplitDate_IntoMonthAndYear_ReturnsNull(string date)
    {
        // Act
        var split = StringDateHelper.SplitDate(date);

        // Assert
        split.startMonth.Should().BeNull();
        split.startYear.Should().BeNull();
    }

    [TestMethod]
    public void ConvertToDateString_NullMonthAndYear_ReturnsEmptyString()
    {
        var result = StringDateHelper.ConvertToDateString(null, null);
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConvertToDateString_NullYear_ReturnsEmptyString()
    {
        var result = StringDateHelper.ConvertToDateString(5, null);
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ConvertToDateString_NullMonth_ReturnsEmptyString()
    {
        var result = StringDateHelper.ConvertToDateString(null, 2020);
        result.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(9, 2013, "September 2013")]
    [DataRow(1, 2000, "January 2000")]
    [DataRow(12, 2024, "December 2024")]
    public void ConvertToDateString_ValidMonthAndYear_ReturnsFormattedString(int month, int year, string expected)
    {
        var result = StringDateHelper.ConvertToDateString(month, year);
        result.Should().Be(expected);
    }
}