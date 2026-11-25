using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class BackToTopButtonMapperTests
{
    [TestMethod]
    public void Map_NullButton_ReturnsNull()
    {
        BackToTopButtonMapper.Map(null).Should().BeNull();
    }

    [TestMethod]
    public void Map_ButtonWithText_MapsText()
    {
        var result = BackToTopButtonMapper.Map(new BackToTopButton { Text = "Custom text" });

        result.Should().NotBeNull();
        result!.Text.Should().Be("Custom text");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void Map_ButtonWithBlankText_FallsBackToDefault(string? text)
    {
        var result = BackToTopButtonMapper.Map(new BackToTopButton { Text = text! });

        result.Should().NotBeNull();
        result!.Text.Should().Be("Back to top");
    }
}
