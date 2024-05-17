using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class ModelRenderersSetupTests
{
    [TestMethod]
    public void AddModelRenderers_AddsExpectedRendererServices()
    {
        var services = new ServiceCollection();

        services.AddModelRenderers();

        services.Count.Should().Be(6);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IHtmlRenderer));
        services.Should().ContainSingle(s => s.ServiceType == typeof(IPhaseBannerRenderer));
        services.Should().ContainSingle(s => s.ServiceType == typeof(ISideContentRenderer));
        services.Should().ContainSingle(s => s.ServiceType == typeof(IHtmlTableRenderer));
        services.Should().ContainSingle(s => s.ServiceType == typeof(ISuccessBannerRenderer));
        services.Should().ContainSingle(s => s.ServiceType == typeof(IGovUkInsetTextRenderer));
    }
}