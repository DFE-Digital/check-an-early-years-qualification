using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(o => o.IsProduction()).Returns(true);

        var sut =
            new OptionsIndicatorViewComponent(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                              optionsManager.Object,
                                              environmentService.Object);

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

        var environmentService = new Mock<IEnvironmentService>();
        environmentService.Setup(o => o.IsProduction()).Returns(false);

        var sut =
            new OptionsIndicatorViewComponent(NullLogger<OptionsIndicatorViewComponent>.Instance,
                                              optionsManager.Object,
                                              environmentService.Object);

        var result = await sut.InvokeAsync();

        result.Should().BeOfType<ViewViewComponentResult>();

        var componentResult = result as ViewViewComponentResult;

        componentResult!.ViewData!.Model.Should().BeOfType<OptionsPageModel>();

        var model = componentResult.ViewData.Model as OptionsPageModel;

        model!.Option.Should().Be(OptionsPageModel.PreviewOptionValue);
    }
}