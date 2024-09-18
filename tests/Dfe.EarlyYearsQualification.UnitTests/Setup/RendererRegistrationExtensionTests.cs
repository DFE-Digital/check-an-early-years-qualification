using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;
using TableCellRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableCellRenderer;
using TableRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRenderer;
using TableRowRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRowRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.Setup;

[TestClass]
public class RendererRegistrationExtensionTests
{
    [TestMethod]
    public void AddModelRenderers_AddsExpectedRendererServices()
    {
        var services = new ServiceCollection();

        // ReSharper disable once InvokeAsExtensionMethod
        RendererRegistrationExtension.AddModelRenderers(services);

        services.Count.Should().Be(20);

        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(PhaseBannerRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(ParagraphRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(ExternalNavigationLinkRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading1Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading2Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading3Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading4Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading5Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(Heading6Renderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(HyperlinkRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(InsetTextRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(MailtoLinkRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(SuccessBannerParagraphRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(TableCellRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(TableHeadingRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(TableRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(TableRowRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(UnorderedListRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IContentRenderer)
                                             && s.ImplementationInstance != null
                                             && s.ImplementationInstance.GetType() == typeof(UnorderedListHyperlinksRenderer)
                                             && s.Lifetime == ServiceLifetime.Singleton);
        
        services.Should().ContainSingle(s => s.ServiceType == typeof(IGovUkContentfulParser)
                                       && s.ImplementationType == typeof(GovUkContentfulParser)
                                       && s.Lifetime == ServiceLifetime.Transient);

        
    }
}