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
}