using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Helpers;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class EmbeddedParagraphRendererTests
{
    [TestMethod]
    public void EmbeddedParagraphRenderer_EmptyContent_DoesNotSupport()
    {
        var content = new EntryStructure();

        new EmbeddedParagraphRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void EmbeddedParagraphRenderer_DataTargetIsNull_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = null } };

        new EmbeddedParagraphRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void EmbeddedParagraphRenderer_DataTargetIsNotCustomNode_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = new Text() } };

        new EmbeddedParagraphRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void EmbeddedParagraphRenderer_DataTargetIsEmptyCustomNode_NotSupported()
    {
        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = new CustomNode()
                                 }
                      };

        new EmbeddedParagraphRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void EmbeddedParagraphRenderer_DataTargetIsEmbeddedParagraph_IsSupported()
    {
        var embeddedParagraph = new EmbeddedParagraph();

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = embeddedParagraph
                                 }
                      };

        new EmbeddedParagraphRenderer().SupportsContent(content).Should().BeTrue();
    }

    [TestMethod]
    public async Task EmbeddedParagraphRenderer_RendersContent()
    {
        var embeddedParagraph = new EmbeddedParagraph
                                {
                                    Content = ContentfulContentHelper.Paragraph("Test Content")
                                };

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = embeddedParagraph
                                 }
                      };

        var renderer = new EmbeddedParagraphRenderer();

        string result = await renderer.RenderAsync(content);

        result.Should().Be("<p class=\"govuk-body\">Test Content</p>");
    }
}