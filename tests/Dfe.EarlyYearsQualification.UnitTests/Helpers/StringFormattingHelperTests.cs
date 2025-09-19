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
}