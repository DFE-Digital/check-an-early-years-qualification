using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.TestSupport;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class FooterLinksViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_InProduction_CallsContentService_ReturnsNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        var navigationLink = new NavigationLink
                             { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true };

        mockContentService.Setup(x => x.GetNavigationLinks())
                          .ReturnsAsync([navigationLink]);

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    environmentService.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        object? model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as IEnumerable<NavigationLinkModel>;

        var navigationLinkModels = data!.ToList();

        navigationLinkModels.Should().NotBeNull();

        navigationLinkModels.Count.Should().Be(1);

        var linkModel = navigationLinkModels[0];

        linkModel.DisplayText.Should().Be(navigationLink.DisplayText);
        linkModel.Href.Should().Be(navigationLink.Href);
        linkModel.OpenInNewTab.Should().Be(navigationLink.OpenInNewTab);
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironments_CallsContentService_ReturnsNavigationLinksPlusOptionsLink()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        var navigationLink = new NavigationLink
                             { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true };

        mockContentService.Setup(x => x.GetNavigationLinks())
                          .ReturnsAsync([navigationLink]);

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    environmentService.Object);
        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        object? model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = model as IEnumerable<NavigationLinkModel>;

        var navigationLinkModels = data!.ToList();

        navigationLinkModels.Should().NotBeNull();

        navigationLinkModels.Count.Should().Be(2);

        var linkModel = navigationLinkModels[0];

        linkModel.DisplayText.Should().Be(navigationLink.DisplayText);
        linkModel.Href.Should().Be(navigationLink.Href);
        linkModel.OpenInNewTab.Should().Be(navigationLink.OpenInNewTab);

        var optionsLinkModel = navigationLinkModels[1];

        optionsLinkModel.DisplayText.Should().Be("Options");
        optionsLinkModel.Href.Should().Be("/options");
        optionsLinkModel.OpenInNewTab.Should().Be(false);
    }

    [TestMethod]
    public async Task InvokeAsync_InProduction_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        mockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(true);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    environmentService.Object);

        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        object? model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<NavigationLinkModel>>();

        var data = ((IEnumerable<NavigationLinkModel>)model!).ToList();

        data.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironments_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<FooterLinksViewComponent>>();

        mockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(x => x.IsProduction()).Returns(false);

        var footerLinksViewComponent = new FooterLinksViewComponent(mockLogger.Object,
                                                                    mockContentService.Object,
                                                                    environmentService.Object);

        var result = await footerLinksViewComponent.InvokeAsync();

        result.Should().NotBeNull();

        object? model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull().And.BeAssignableTo<IEnumerable<NavigationLinkModel>>();

        var data = ((IEnumerable<NavigationLinkModel>)model!).ToList();

        data.Should().BeEmpty();

        mockLogger.VerifyError("Error retrieving navigation links for footer");
    }
}