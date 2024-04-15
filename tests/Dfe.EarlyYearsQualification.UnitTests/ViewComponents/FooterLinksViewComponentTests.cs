using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class FooterLinksViewComponentTests
{
    private ILogger<FooterLinksViewComponent> _mockLogger = new NullLoggerFactory().CreateLogger<FooterLinksViewComponent>();
    private Mock<IContentService> _mockContentService = new();
    
    [TestInitialize]
    public void BeforeTestRun()
    {
        _mockLogger = new NullLoggerFactory().CreateLogger<FooterLinksViewComponent>();
        _mockContentService = new Mock<IContentService>();
    }

    [TestMethod]
    public async Task InvokeAsync_CallsContentService_ReturnsNavigationLinks()
    {
        var navigationLink = new NavigationLink { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true };

        _mockContentService.Setup(x => x.GetNavigationLinks()).ReturnsAsync(new List<NavigationLink> { navigationLink });

        var footerLinksViewComponent = new FooterLinksViewComponent(_mockContentService.Object, _mockLogger);
        var result = await footerLinksViewComponent.InvokeAsync();

        Assert.IsNotNull(result);
        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        Assert.IsNotNull(model);

        var data = model as List<NavigationLink>;
        Assert.IsNotNull(data);

        Assert.AreEqual(navigationLink.DisplayText, data[0].DisplayText);
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceReturnsNull_ReturnsEmptyNavigationLinks()
    {
        _mockContentService.Setup(x => x.GetNavigationLinks()).ReturnsAsync((List<NavigationLink>)default!);

        var footerLinksViewComponent = new FooterLinksViewComponent(_mockContentService.Object, _mockLogger);
        var result = await footerLinksViewComponent.InvokeAsync();

        Assert.IsNotNull(result);
        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        Assert.IsNotNull(model);

        var data = model as List<NavigationLink>;
        Assert.IsNull(data);
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        _mockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var footerLinksViewComponent = new FooterLinksViewComponent(_mockContentService.Object, _mockLogger);
        var result = await footerLinksViewComponent.InvokeAsync();

        Assert.IsNotNull(result);
        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        Assert.IsNotNull(model);

        var data = model as List<NavigationLink>;
        Assert.IsNull(data);
    }
}
