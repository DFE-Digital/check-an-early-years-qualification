using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
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

        services.Count.Should().Be(22);

        VerifyService<IContentRenderer, PhaseBannerRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, ParagraphRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IContentRenderer, NavigationLinkRenderer>(services, ServiceLifetime.Singleton);

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
        
        VerifyService<IContentRenderer, OrderedListRenderer>(services, ServiceLifetime.Singleton);

        VerifyService<IGovUkContentParser, GovUkContentParser>(services, ServiceLifetime.Transient, AllowNulls.Yes);

        VerifyService<IContentRenderer, EmbeddedParagraphRenderer>(services, ServiceLifetime.Singleton);
    }

    [TestMethod]
    public void VerifyService_WithService_Passes()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStuffDoer>(new StuffDoer());

        VerifyService<IStuffDoer, StuffDoer>(services, ServiceLifetime.Singleton);
    }

    [TestMethod]
    public void VerifyService_WithNoServiceRegistered_Throws()
    {
        var services = new ServiceCollection();

        var exceptionCaught = false;

        try
        {
            VerifyService<IStuffDoer, StuffDoer>(services, ServiceLifetime.Singleton);
        }
        catch
        {
            exceptionCaught = true;
        }

        exceptionCaught.Should().BeTrue();
    }

    [TestMethod]
    public void VerifyService_WithNoServiceInstance_Throws()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStuffDoer>();

        var exceptionCaught = false;

        try
        {
            VerifyService<IStuffDoer, StuffDoer>(services, ServiceLifetime.Singleton);
        }
        catch
        {
            exceptionCaught = true;
        }

        exceptionCaught.Should().BeTrue();
    }

    [TestMethod]
    public void VerifyService_WithNoServiceInstance_ButAllowNull_Passes()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStuffDoer>();

        VerifyService<IStuffDoer, StuffDoer>(services, ServiceLifetime.Singleton, AllowNulls.Yes);
    }

    [TestMethod]
    public void VerifyService_WithServiceInstance_WrongLifetime_Throws()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStuffDoer>(new StuffDoer());

        var exceptionCaught = false;

        try
        {
            VerifyService<IStuffDoer, StuffDoer>(services, ServiceLifetime.Transient);
        }
        catch
        {
            exceptionCaught = true;
        }

        exceptionCaught.Should().BeTrue();
    }

    /// <summary>
    ///     This method is tested in the VerifyService_* methods
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <param name="allowNulls"></param>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TInstance"></typeparam>
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

    private interface IStuffDoer;

    private class StuffDoer : IStuffDoer;
}