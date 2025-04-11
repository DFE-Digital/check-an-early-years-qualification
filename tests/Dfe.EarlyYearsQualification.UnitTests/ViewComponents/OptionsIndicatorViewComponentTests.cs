using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class OptionsIndicatorViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_InProduction_ReturnsView_WithDefaultOption()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption())
                      .ReturnsAsync(CachingOption.BypassCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        OptionsIndicatorViewComponent sut = new(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                                optionsManager.Object,
                                                config.Object);

        IViewComponentResult result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        ViewViewComponentResult? componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        OptionsPageModel? model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.DefaultOptionValue);
    }

    [TestMethod]
    public async Task InvokeAsync_IfEnvironmentNotSet_ReturnsView_WithDefaultOption()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.BypassCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns((string)null!);

        OptionsIndicatorViewComponent sut = new(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                                optionsManager.Object,
                                                config.Object);

        IViewComponentResult result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        ViewViewComponentResult? componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        OptionsPageModel? model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.DefaultOptionValue);
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironmentsReturnsView_WithCorrectOption()
    {
        var optionsManager = new Mock<ICachingOptionsManager>();
        optionsManager.Setup(o => o.GetCachingOption()).ReturnsAsync(CachingOption.BypassCache);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        OptionsIndicatorViewComponent sut = new(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                                optionsManager.Object,
                                                config.Object);

        IViewComponentResult result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        ViewViewComponentResult? componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        OptionsPageModel? model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.BypassCacheOptionValue);
    }
}