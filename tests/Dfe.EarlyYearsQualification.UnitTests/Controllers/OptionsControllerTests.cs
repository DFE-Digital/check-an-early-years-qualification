using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class OptionsControllerTests
{
    [TestMethod]
    public async Task Get_InProduction_ReturnsNotFound()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();

        var contentOptionsManager = new Mock<IContentOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var result = await controller.Index();

        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Get_InLowerEnvironments_ReturnsView()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(o => o.GetCachingOption())
                             .ReturnsAsync(CachingOption.UseCache);

        var contentOptionsManager = new Mock<IContentOptionsManager>();
        contentOptionsManager.Setup(o => o.GetContentOption())
                             .ReturnsAsync(ContentOption.UsePublished);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var result = await controller.Index();

        result.Should().BeOfType<ViewResult>();

        var viewResult = (ViewResult)result;

        viewResult.Model.Should().BeOfType<OptionsPageModel>();

        var model = viewResult.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PublishedOptionValue);
    }

    [TestMethod]
    public async Task Get_InLowerEnvironments_ReturnsViewWithOptionSet()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(o => o.GetCachingOption())
                             .ReturnsAsync(CachingOption.BypassCache);

        var contentOptionsManager = new Mock<IContentOptionsManager>();
        contentOptionsManager.Setup(o => o.GetContentOption())
                             .ReturnsAsync(ContentOption.UsePreview);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var result = await controller.Index();

        result.Should().BeOfType<ViewResult>();

        var viewResult = (ViewResult)result;

        viewResult.Model.Should().BeOfType<OptionsPageModel>();

        var model = viewResult.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PreviewOptionValue);
    }

    [TestMethod]
    public async Task Post_InProduction_ReturnsNotFound()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();

        var contentOptionsManager = new Mock<IContentOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var result = await controller.Index(new OptionsPageModel());

        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Post_InLowerEnvironments_ReturnsRedirectToStartPage()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();
        cachingOptionsManager.Setup(o => o.GetCachingOption())
                             .ReturnsAsync(CachingOption.UseCache);

        var contentOptionsManager = new Mock<IContentOptionsManager>();
        contentOptionsManager.Setup(o => o.GetContentOption())
                             .ReturnsAsync(ContentOption.UsePublished);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var result = await controller.Index(new OptionsPageModel());

        result.Should().BeOfType<RedirectToActionResult>();

        var toActionResult = result as RedirectToActionResult;

        toActionResult!.ActionName.Should().Be("Index");
        toActionResult.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task PostToUsePublished_InLowerEnvironments_SetsOptions()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();

        var contentOptionsManager = new Mock<IContentOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var model = new OptionsPageModel();
        model.SetOption(ContentOption.UsePublished);

        await controller.Index(model);

        cachingOptionsManager.Verify(x => x.SetCachingOption(CachingOption.UseCache),
                                     Times.Once);

        contentOptionsManager.Verify(x => x.SetContentOption(ContentOption.UsePublished),
                                     Times.Once);
    }

    [TestMethod]
    public async Task PostToUsePreview_InLowerEnvironments_SetsOption()
    {
        var cachingOptionsManager = new Mock<ICachingOptionsManager>();

        var contentOptionsManager = new Mock<IContentOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var controller = new OptionsController(NullLogger<OptionsController>.Instance,
                                               cachingOptionsManager.Object,
                                               contentOptionsManager.Object,
                                               config.Object);

        var model = new OptionsPageModel();
        model.SetOption(ContentOption.UsePreview);

        await controller.Index(model);

        cachingOptionsManager.Verify(x => x.SetCachingOption(CachingOption.BypassCache),
                                     Times.Once);

        contentOptionsManager.Verify(x => x.SetContentOption(ContentOption.UsePreview),
                                     Times.Once);
    }
}