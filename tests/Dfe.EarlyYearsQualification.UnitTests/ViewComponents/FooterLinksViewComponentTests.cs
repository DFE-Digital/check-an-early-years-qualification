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
    private ILogger<FooterLinksViewComponent> MockLogger = new NullLoggerFactory().CreateLogger<FooterLinksViewComponent>();
    private Mock<IContentService> MockContentService = new Mock<IContentService>();
    
    [TestInitialize]
    public void BeforeTestRun()
    {
        MockLogger = new NullLoggerFactory().CreateLogger<FooterLinksViewComponent>();
        MockContentService = new Mock<IContentService>();
    }

    [TestMethod]
    public async Task InvokeAsync_CallsContentService_ReturnsNavigationLinks()
    {
        var navigationLink = new NavigationLink() { DisplayText = "Test", Href = "https://test.com", OpenInNewTab = true };

        MockContentService.Setup(x => x.GetNavigationLinks()).ReturnsAsync(new List<NavigationLink>(){ navigationLink });

        var footerLinksViewComponent = new FooterLinksViewComponent(MockContentService.Object, MockLogger);
        var result = await footerLinksViewComponent.InvokeAsync();

        Assert.IsNotNull(result);
        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        Assert.IsNotNull(model);

        var data = model as List<NavigationLink>;
        Assert.IsNotNull(data);

        Assert.AreEqual(navigationLink.DisplayText, data[0].DisplayText);
    }

    [TestMethod]
    public async Task InvokeAsync_ContentServiceThrowsException_ReturnsEmptyNavigationLinks()
    {
        MockContentService.Setup(x => x.GetNavigationLinks()).ThrowsAsync(new Exception());

        var footerLinksViewComponent = new FooterLinksViewComponent(MockContentService.Object, MockLogger);
        var result = await footerLinksViewComponent.InvokeAsync();

        Assert.IsNotNull(result);
        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        Assert.IsNotNull(model);

        var data = model as List<NavigationLink>;
        Assert.IsNull(data);
    }
}
