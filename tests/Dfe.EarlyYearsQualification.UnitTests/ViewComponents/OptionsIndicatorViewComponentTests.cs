using Dfe.EarlyYearsQualification.Content.Options;
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
        var optionsManager = new Mock<IContentOptionsManager>();
        optionsManager.Setup(o => o.GetContentOption())
                      .ReturnsAsync(ContentOption.UsePublished);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("production");

        var sut =
            new OptionsIndicatorViewComponent(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                              optionsManager.Object,
                                              config.Object);

        var result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        var componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        var model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PublishedOptionValue);
    }

    [TestMethod]
    public async Task InvokeAsync_IfEnvironmentNotSet_ReturnsView_WithDefaultOption()
    {
        var optionsManager = new Mock<IContentOptionsManager>();
        optionsManager.Setup(o => o.GetContentOption())
                      .ReturnsAsync(ContentOption.UsePublished);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns((string)null!);

        var sut =
            new OptionsIndicatorViewComponent(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                              optionsManager.Object,
                                              config.Object);

        var result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        var componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        var model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PublishedOptionValue);
    }

    [TestMethod]
    public async Task InvokeAsync_InLowerEnvironmentsReturnsView_WithCorrectOption()
    {
        var optionsManager = new Mock<IContentOptionsManager>();
        optionsManager.Setup(o => o.GetContentOption())
                      .ReturnsAsync(ContentOption.UsePreview);

        var config = new Mock<IConfiguration>();
        config.Setup(c => c["ENVIRONMENT"]).Returns("development");

        var sut =
            new OptionsIndicatorViewComponent(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                              optionsManager.Object,
                                              config.Object);

        var result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        var componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        var model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PreviewOptionValue);
    }
}