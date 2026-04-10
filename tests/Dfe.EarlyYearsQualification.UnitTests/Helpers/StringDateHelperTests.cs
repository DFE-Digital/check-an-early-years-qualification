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
        var (startMonth, startYear) = StringDateHelper.SplitDate(date);

        // Assert
        startMonth.Should().Be(int.Parse(date.Split('/')[0]));
        startYear.Should().Be(int.Parse(date.Split('/')[1]));
    }

    [TestMethod]
    [DataRow("25")]
    [DataRow("26/03/2000")]
    [DataRow("Text/Text")]
    public void SplitDate_IntoMonthAndYear_ReturnsNull(string date)
    {
        // Act
        var (startMonth, startYear) = StringDateHelper.SplitDate(date);

        // Assert
        startMonth.Should().BeNull();
        startYear.Should().BeNull();
    }

    [TestMethod]
    [DataRow("Sep-19", 9, 2019)]
    [DataRow("Jan-23", 1, 2023)]
    [DataRow("Dec-99", 12, 1999)]
    public void ConvertDate_ValidFormat_ReturnsMonthAndYear(string date, int expectedMonth, int expectedYear)
    {
        // Act
        var result = StringDateHelper.ConvertDate(date);

        // Assert
        result.Should().NotBeNull();
        result.Value.startMonth.Should().Be(expectedMonth);
        result.Value.startYear.Should().Be(expectedYear);
    }

    [TestMethod]
    [DataRow("InvalidDate")]
    [DataRow("25")]
    [DataRow("12/2022")]
    [DataRow("")]
    [DataRow("Text-Text")]
    public void ConvertDate_InvalidFormat_ReturnsNull(string date)
    {
        // Act
        var result = StringDateHelper.ConvertDate(date);

        // Assert
        result.Should().BeNull();
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