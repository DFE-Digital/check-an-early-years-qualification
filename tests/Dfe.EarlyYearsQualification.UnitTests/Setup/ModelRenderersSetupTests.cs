using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class ModelRenderersSetupTests
{
    [TestMethod]
    public void AddModelRenderers_IsFluent()
    {
        var services = new ServiceCollection();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddModelRenderers(services)
                                   .Should().BeSameAs(services);
    }

    [TestMethod]
    public void AddModelRenderers_AddsExpectedRendererServices()
    {
        var services = new ServiceCollection();

        // ReSharper disable once InvokeAsExtensionMethod
        ServiceCollectionExtensions.AddModelRenderers(services);

        services.Count.Should().Be(6);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IHtmlRenderer)
                                             && s.ImplementationType == typeof(HtmlModelRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IPhaseBannerRenderer)
                                             && s.ImplementationType == typeof(PhaseBannerRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);

        services.Should().ContainSingle(s => s.ServiceType == typeof(ISideContentRenderer)
                                             && s.ImplementationType == typeof(SideContentRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IHtmlTableRenderer)
                                             && s.ImplementationType == typeof(HtmlTableRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);

        services.Should().ContainSingle(s => s.ServiceType == typeof(ISuccessBannerRenderer)
                                             && s.ImplementationType == typeof(SuccessBannerRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IGovUkInsetTextRenderer)
                                             && s.ImplementationType == typeof(GovUkInsetTextRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
    }
}