using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class ServiceControllerTests
{
    [TestMethod]
    public void MapToNavigationLinkModel_PassInNull_ReturnNull()
    {
        var result = DummyController.Map(null);
        result.Should().BeNull();
    }
    
    [TestMethod]
    public void MapToNavigationLinkModel_PassInNavigationLink_ReturnModel()
    {
        var navLink = new NavigationLink
                      {
                          Sys = new SystemProperties
                                {
                                    ContentType = new ContentType
                                                  {
                                                      SystemProperties = new SystemProperties
                                                                         { Id = "externalNavigationLink" }
                                                  }
                                },
                          OpenInNewTab = true,
                          DisplayText = "DisplayText",
                          Href = "/"
                      };
        var result = DummyController.Map(navLink);
        result.Should().NotBeNull();
        result.Should().BeOfType<NavigationLinkModel>();
        result!.Href.Should().BeSameAs(navLink.Href);
        result.DisplayText.Should().BeSameAs(navLink.DisplayText);
        result.OpenInNewTab.Should().Be(navLink.OpenInNewTab);
    }
}

public class DummyController : ServiceController
{
    public static NavigationLinkModel? Map(NavigationLink? navigationLink)
    {
        return MapToNavigationLinkModel(navigationLink);
    }
}