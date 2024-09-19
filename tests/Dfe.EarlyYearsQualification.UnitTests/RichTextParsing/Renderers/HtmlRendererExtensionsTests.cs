using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using GovParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class HtmlRendererExtensionsTests
{
    [TestMethod]
    public void HtmlRendererExtensions_AddsRenderersCorrectly()
    {
        var navLink = new NavigationLink
                      {
                          Sys = new SystemProperties
                                {
                                    ContentType = new ContentType
                                                  {
                                                      SystemProperties = new SystemProperties
                                                                         { Id = "externalNavigationLink" }
                                                  }
                                }
                      };
        var jObject = JObject.FromObject(navLink);
        var renderer = new HtmlRenderer();

        // ReSharper disable once InvokeAsExtensionMethod
        HtmlRendererExtensions.AddCommonRenderers(renderer);

        renderer.Renderers.GetRendererForContent(new Hyperlink()).Should().BeOfType<HyperlinkRenderer>();
        renderer.Renderers.GetRendererForContent(new Heading1()).Should().BeOfType<Heading1Renderer>();
        renderer.Renderers.GetRendererForContent(new Heading2()).Should().BeOfType<Heading2Renderer>();
        renderer.Renderers.GetRendererForContent(new Heading3()).Should().BeOfType<Heading3Renderer>();
        renderer.Renderers.GetRendererForContent(new Heading4()).Should().BeOfType<Heading4Renderer>();
        renderer.Renderers.GetRendererForContent(new Heading5()).Should().BeOfType<Heading5Renderer>();
        renderer.Renderers.GetRendererForContent(new Heading6()).Should().BeOfType<Heading6Renderer>();
        renderer.Renderers.GetRendererForContent(new Paragraph()).Should().BeOfType<GovParagraphRenderer>();
        renderer.Renderers.GetRendererForContent(new EntryStructure
                                                 {
                                                     Data = new EntryStructureData
                                                            {
                                                                Target = new CustomNode
                                                                         {
                                                                             JObject = jObject
                                                                         }
                                                            }
                                                 }).Should()
                .BeOfType<ExternalNavigationLinkRenderer>();
    }
}