using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Services.DatesAndTimes;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class PlaceholderUpdaterTests
{
    [TestMethod]
    public void Replace_TextIsEmptyString_ReturnsEmptyString()
    {
        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        var result = placeholderUpdater.Replace(string.Empty);
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Replace_TextIsNull_ReturnsNullString()
    {
        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        var result = placeholderUpdater.Replace(null!);
        result.Should().BeNull();
    }

    [TestMethod]
    public void Replace_TextContainsNoPlaceholders_ReturnsString()
    {
        var now = new DateTime(2024, 11, 01, 00, 00, 00, DateTimeKind.Local);

        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        mockDateTimeAdapter.Setup(x => x.Now()).Returns(now);
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        const string text = "This contains no placeholders";
        var result = placeholderUpdater.Replace(text);
        result.Should().Match(text);
    }

    [TestMethod]
    public void Replace_TextContainsActualYearPlaceholder_ReturnsString()
    {
        var now = new DateTime(2024, 11, 01, 00, 00, 00, DateTimeKind.Local);

        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        mockDateTimeAdapter.Setup(x => x.Now()).Returns(now);
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        const string text = "The year is $[actual-year]$";
        var result = placeholderUpdater.Replace(text);
        result.Should().Match("The year is 2024");
    }

    [TestMethod]
    public void Replace_TextContainsMultiplePlaceholders_ReturnsString()
    {
        var now = new DateTime(2024, 11, 01, 00, 00, 00, DateTimeKind.Local);

        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        mockDateTimeAdapter.Setup(x => x.Now()).Returns(now);
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        const string text = "Year is $[actual-year]$ and year again is $[actual-year]$";
        var result = placeholderUpdater.Replace(text);
        result.Should().Match("Year is 2024 and year again is 2024");
    }
    
    [TestMethod]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    public void Replace_TextContainsLevelForAug14ToSept19PlaceholderAndLevelIsBelow6_ReturnsConvertedString(int level)
    {
        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.GetLevelOfQualification()).Returns(level);
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        var result = placeholderUpdater.Replace("This should be $[level-for-Sept14-to-Aug19]$");
        result.Should().Be("This should be level 3");
    }
    
    [TestMethod]
    [DataRow(6)]
    [DataRow(7)]
    public void Replace_TextContainsLevelForAug14ToSept19PlaceholderAndLevelIs6OrAbove_ReturnsConvertedString(int level)
    {
        var mockDateTimeAdapter = new Mock<IDateTimeAdapter>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        mockUserJourneyCookieService.Setup(x => x.GetLevelOfQualification()).Returns(level);
        var placeholderUpdater = new PlaceholderUpdater(mockDateTimeAdapter.Object, mockUserJourneyCookieService.Object);
        var result = placeholderUpdater.Replace("This should be $[level-for-Sept14-to-Aug19]$");
        result.Should().Be("This should be level 3 or level 6");
    }
}