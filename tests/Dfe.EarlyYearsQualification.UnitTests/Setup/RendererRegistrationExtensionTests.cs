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

        VerifyService<IContentRenderer, PhaseBannerRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, ParagraphRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, ExternalNavigationLinkRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading1Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading2Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading3Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading4Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading5Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, Heading6Renderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, HyperlinkRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, InsetTextRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, MailtoLinkRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, SuccessBannerRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, TableCellRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, TableHeadingRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, TableRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, TableRowRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, UnorderedListRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, UnorderedListHyperlinksRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IGovUkContentParser, GovUkContentParser>(services, ServiceLifetime.Transient, AllowNulls.Yes);
    }

    private static void VerifyService<TService, TInstance>(ServiceCollection services, ServiceLifetime lifetime,
                                                           AllowNulls allowNulls = AllowNulls.No)
    {
        services.Should()
                .ContainSingle(s => s.ServiceType == typeof(TService)
                                    && (allowNulls == AllowNulls.Yes
                                        || (s.ImplementationInstance != null &&
                                            s.ImplementationInstance.GetType() == typeof(TInstance)))
                                    && s.Lifetime == lifetime);
    }

    private enum AllowNulls
    {
        No,
        Yes
    }
}