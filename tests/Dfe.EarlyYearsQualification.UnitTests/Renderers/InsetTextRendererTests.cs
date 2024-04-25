using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class InsetTextRendererTests
{
    [TestMethod]
    public void InsetTextRenderer_EmptyContent_DoesNotSupport()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var content = new EntryStructure();

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsNull_DoesNotSupport()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var content = new EntryStructure { Data = new EntryStructureData { Target = null } };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsNotCustomNode_DoesNotSupport()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var content = new EntryStructure { Data = new EntryStructureData { Target = new Text() } };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsEmptyCustomNode_NotSupported()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = new CustomNode()
                                 }
                      };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsCustomNodeWithJObject_OtherId_Supported()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var govUkInsetTextModel = new GovUkInsetTextModel
                                  {
                                      Sys =
                                      {
                                          ContentType = new ContentType
                                                        {
                                                            SystemProperties = new SystemProperties
                                                                               {
                                                                                   Id = "govUkInsetText2"
                                                                               }
                                                        }
                                      }
                                  };
        
        var jObject = JObject.FromObject(govUkInsetTextModel);

        var customNode = new CustomNode
                         {
                             JObject = jObject
                         };

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = customNode
                                 }
                      };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DataTargetIsCustomNodeWithJObject_Supported()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var govUkInsetTextModel = new GovUkInsetTextModel
                                  {
                                      Sys =
                                      {
                                          ContentType = new ContentType
                                                        {
                                                            SystemProperties = new SystemProperties
                                                                               {
                                                                                   Id = "govUkInsetText"
                                                                               }
                                                        }
                                      }
                                  };
        
        var jObject = JObject.FromObject(govUkInsetTextModel);

        var customNode = new CustomNode
                         {
                             JObject = jObject
                         };

        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = customNode
                                 }
                      };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(content).Should().BeTrue();
    }

    [TestMethod]
    public void InsetTextRenderer_DoesNotSupportAnotherList()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var list = new List { NodeType = "ordered-list" };

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void InsetTextRenderer_DoesNotSupportHyperlink()
    {
        var contentClientMock = new Mock<IContentfulClient>();

        var list = new Hyperlink();

        new InsetTextRenderer(contentClientMock.Object).SupportsContent(list).Should().BeFalse();
    }
}