using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class FooterLinksViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_CallsContentService_ReturnsNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        var navigationLink = new NavigationLink
                             { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true };

        mockContentService.Setup(x => x.GetNavigationLinks())
                          .ReturnsAsync(new List<NavigationLink> { navigationLink });

        var footerLinksViewComponent = new FooterLinksViewComponent(mockContentService.Object, mockLogger.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as List<NavigationLink>;
        data.Should().NotBeNull();

        data![0].DisplayText.Should().Be(navigationLink.DisplayText);
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceReturnsNull_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        mockContentService.Setup(x => x.GetNavigationLinks()).ReturnsAsync((List<NavigationLink>?)default);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockContentService.Object, mockLogger.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<NavigationLink>>();

        var data = ((IEnumerable<NavigationLink>)model!).ToList();
        data.Should().BeEmpty();
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        mockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var footerLinksViewComponent = new FooterLinksViewComponent(mockContentService.Object, mockLogger.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<NavigationLink>>();

        var data = ((IEnumerable<NavigationLink>)model!).ToList();

        data.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }
}