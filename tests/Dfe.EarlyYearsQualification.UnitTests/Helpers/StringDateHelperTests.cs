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
}