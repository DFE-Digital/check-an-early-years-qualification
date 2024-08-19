using Dfe.EarlyYearsQualification.Web.Helpers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class PlaceholderUpdaterTests
{
    [TestMethod]
    public void Replace_ValueToCheckIsEmptyString_ReturnsEmptyString()
    {
        var placeholderUpdater = new PlaceholderUpdater();
        var result = placeholderUpdater.Replace(string.Empty);
        result.Should().BeEmpty();
    }
    
    [TestMethod]
    public void Replace_ValueToCheckIsNull_ReturnsNullString()
    {
        var placeholderUpdater = new PlaceholderUpdater();
        var result = placeholderUpdater.Replace(null!);
        result.Should().BeNull();
    }
    
    [TestMethod]
    public void Replace_ValueToCheckContainsNoPlaceholders_ReturnsString()
    {
        var placeholderUpdater = new PlaceholderUpdater();
        const string valueToCheck = "This contains no placeholders";
        var result = placeholderUpdater.Replace(valueToCheck);
        result.Should().Match(valueToCheck);
    }
    
    [TestMethod]
    public void Replace_ValueToCheckContainsActualYearPlaceholder_ReturnsString()
    {
        var placeholderUpdater = new PlaceholderUpdater();
        const string valueToCheck = "The year is $[actual-year]$";
        var result = placeholderUpdater.Replace(valueToCheck);
        var expectedResult = $"The year is {DateTimeOffset.Now.Year}"; 
        result.Should().Match(expectedResult);
    }
}