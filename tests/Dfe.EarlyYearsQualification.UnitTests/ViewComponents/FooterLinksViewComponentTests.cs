using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Models.Content;
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
                          .ReturnsAsync([navigationLink]);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object, mockContentService.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as IEnumerable<NavigationLinkModel>;

        var navigationLinkModels = data!.ToList();

        navigationLinkModels.Should().NotBeNull();

        var linkModel = navigationLinkModels[0];

        linkModel.DisplayText.Should().Be(navigationLink.DisplayText);
        linkModel.Href.Should().Be(navigationLink.Href);
        linkModel.OpenInNewTab.Should().Be(navigationLink.OpenInNewTab);
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        mockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object, mockContentService.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<NavigationLinkModel>>();

        var data = ((IEnumerable<NavigationLinkModel>)model!).ToList();

        data.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }
}