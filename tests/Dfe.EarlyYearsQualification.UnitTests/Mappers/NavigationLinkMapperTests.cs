using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class NavigationLinkMapperTests
{
    [TestMethod]
    public void Map_BackButtonIsNull_ReturnsNull()
    {
        var result = NavigationLinkMapper.Map(null);

        result.Should().BeNull();
    }

    [TestMethod]
    public void Map_BackButtonIsNotNull_ReturnsModel()
    {
        var backButton = new NavigationLink
                         {
                             DisplayText = "Back",
                             OpenInNewTab = true,
                             Href = "/"
                         };

        var result = NavigationLinkMapper.Map(backButton);
        
        result.Should().NotBeNull();
        result!.DisplayText.Should().BeSameAs(backButton.DisplayText);
        result.OpenInNewTab.Should().BeTrue();
        result.Href.Should().BeSameAs(backButton.Href);
    }
}