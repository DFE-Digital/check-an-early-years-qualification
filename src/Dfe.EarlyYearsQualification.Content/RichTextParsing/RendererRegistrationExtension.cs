using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using Microsoft.Extensions.DependencyInjection;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;
using TableCellRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableCellRenderer;
using TableRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRenderer;
using TableRowRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRowRenderer;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing;

public static class RendererRegistrationExtension
{
    public static void AddModelRenderers(this IServiceCollection services)
    {
        // Register each individual renderer
        // Check any renderers that rely on a custom NodeType first

        services.AddSingleton<IContentRenderer>(new PhaseBannerRenderer { Order = 1 })
                .AddSingleton<IContentRenderer>(new SuccessBannerRenderer { Order = 2 })
                .AddSingleton<IContentRenderer>(new EmbeddedParagraphRenderer { Order = 3 })
                .AddSingleton<IContentRenderer>(new ParagraphRenderer { Order = 4 })
                .AddSingleton<IContentRenderer>(new NavigationLinkRenderer { Order = 5 })
                .AddSingleton<IContentRenderer>(new Heading1Renderer { Order = 6 })
                .AddSingleton<IContentRenderer>(new Heading2Renderer { Order = 7 })
                .AddSingleton<IContentRenderer>(new Heading3Renderer { Order = 8 })
                .AddSingleton<IContentRenderer>(new Heading4Renderer { Order = 9 })
                .AddSingleton<IContentRenderer>(new Heading5Renderer { Order = 10 })
                .AddSingleton<IContentRenderer>(new Heading6Renderer { Order = 11 })
                .AddSingleton<IContentRenderer>(new HyperlinkRenderer { Order = 12 })
                .AddSingleton<IContentRenderer>(new InsetTextRenderer { Order = 13 })
                .AddSingleton<IContentRenderer>(new MailtoLinkRenderer { Order = 14 })
                .AddSingleton<IContentRenderer>(new TableCellRenderer { Order = 15 })
                .AddSingleton<IContentRenderer>(new TableHeadingRenderer { Order = 16 })
                .AddSingleton<IContentRenderer>(new TableRenderer { Order = 17 })
                .AddSingleton<IContentRenderer>(new TableRowRenderer { Order = 18 })
                .AddSingleton<IContentRenderer>(new UnorderedListRenderer { Order = 19 })
                .AddSingleton<IContentRenderer>(new UnorderedListHyperlinksRenderer { Order = 20 });

        // Register GovUk parser after each renderer registration
        services.AddTransient<IGovUkContentParser, GovUkContentParser>();
    }
}