using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Helpers;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class InsetTextRendererTests
{
    [TestMethod]
    public void InsetTextRenderer_EmptyContent_DoesNotSupport()
    {
        var content = new EntryStructure();

        new InsetTextRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsNull_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = null } };

        new InsetTextRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsNotCustomNode_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = new Text() } };

        new InsetTextRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsEmptyCustomNode_NotSupported()
    {
        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = new CustomNode()
                                 }
                      };

        new InsetTextRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsGovUkInsetTextModel_IsSupported()
    {
        var govUkInsetTextModel = new GovUkInsetTextModel();

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = govUkInsetTextModel
                                 }
                      };

        new InsetTextRenderer().SupportsContent(content).Should().BeTrue();
    }

    [TestMethod]
    public void InsetTextRenderer_DoesNotSupportAnotherList()
    {
        var list = new List { NodeType = "ordered-list" };

        new InsetTextRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DoesNotSupportHyperlink()
    {
        var list = new Hyperlink();

        new InsetTextRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public async Task InsetTextRenderer_RendersDivWithId()
    {
        var govUkInsetTextModel = new GovUkInsetTextModel
                                  {
                                      Content = ContentfulContentHelper.Text("Test GovUk Inset Text Content")
                                  };

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = govUkInsetTextModel
                                 }
                      };

        var renderer = new InsetTextRenderer();

        string result = await renderer.RenderAsync(content);

        result.Should().Be("<div class=\"govuk-inset-text\">Test GovUk Inset Text Content</div>");
    }
}