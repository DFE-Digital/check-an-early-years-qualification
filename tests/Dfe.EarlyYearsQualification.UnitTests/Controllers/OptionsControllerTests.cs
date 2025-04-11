using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
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
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.UseCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        IActionResult result = await controller.Index();

        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Get_InLowerEnvironments_ReturnsView()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.UseCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        IActionResult result = await controller.Index();

        result.Should().BeOfType<ViewResult>();

        ViewResult viewResult = (ViewResult)result;

        viewResult.Model.Should().BeOfType<OptionsPageModel>();

        OptionsPageModel? model = viewResult.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.DefaultOptionValue);
    }

    [TestMethod]
    public async Task Get_InLowerEnvironments_ReturnsViewWithOptionSet()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.BypassCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        IActionResult result = await controller.Index();

        result.Should().BeOfType<ViewResult>();

        ViewResult viewResult = (ViewResult)result;

        viewResult.Model.Should().BeOfType<OptionsPageModel>();

        OptionsPageModel? model = viewResult.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.BypassCacheOptionValue);
    }

    [TestMethod]
    public async Task Post_InProduction_ReturnsNotFound()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.UseCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        IActionResult result = await controller.Index(new OptionsPageModel());

        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Post_InLowerEnvironments_ReturnsRedirectToStartPage()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.UseCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        IActionResult result = await controller.Index(new OptionsPageModel());

        result.Should().BeOfType<RedirectToActionResult>();

        RedirectToActionResult? toActionResult = result as RedirectToActionResult;

        toActionResult!.ActionName.Should().Be("Index");
        toActionResult!.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task PostToUseCache_InLowerEnvironments_SetsOption()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        await controller.Index(new OptionsPageModel { Option = OptionsPageModel.DefaultOptionValue });

        optionsManager.Verify(x => x.SetCachingOption(CachingOption.UseCache), Times.Once);
    }

    [TestMethod]
    public async Task PostToBypassCache_InLowerEnvironments_SetsOption()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsController controller = new(NullLogger<OptionsController>.Instance,
                                           optionsManager.Object,
                                           config.Object);

        await controller.Index(new OptionsPageModel { Option = OptionsPageModel.BypassCacheOptionValue });

        optionsManager.Verify(x => x.SetCachingOption(CachingOption.BypassCache), Times.Once);
    }
}