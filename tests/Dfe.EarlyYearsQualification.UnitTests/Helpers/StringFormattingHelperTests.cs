using Dfe.EarlyYearsQualification.Web.Helpers;

namespace Dfe.EarlyYearsQualification.UnitTests.Helpers;

[TestClass]
public class StringFormattingHelperTests
{
    [TestMethod]
    [DataRow("123/456/789", "123 / 456 / 789")]
    [DataRow("1/2/", "1 / 2 / ")]
    [DataRow("1 /   2   /   3456", "1 / 2 / 3456")]
    [DataRow(null, null)]
    [DataRow("somestring", "somestring")]
    public void FormatSlashedNumbers_ReturnsExpected(string input, string expected)
    {
        // Act
        var result = StringFormattingHelper.FormatSlashedNumbers(input);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("User_Name-Test", "user_name-test")]
    [DataRow("123 ABC", "123-abc")]
    [DataRow("Name/With/Slash", "name-with-slash")]
    [DataRow("", "")]
    public void ToHtmlId_ReturnsExpected(string input, string expected)
    {
        var result = StringFormattingHelper.ToHtmlId(input);
        result.Should().Be(expected);
    }
}