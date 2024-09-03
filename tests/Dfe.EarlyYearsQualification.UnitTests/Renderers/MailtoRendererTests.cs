using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class MailtoRendererTests
{
    [TestMethod]
    public void SupportsContent_IsNotEntryStructure_ReturnsFalse()
    {
        var content = new Paragraph();
        new MailtoLinkRenderer().SupportsContent(content).Should().BeFalse();
    }
    
    [TestMethod]
    public void SupportsContent_DataTargetIsNull_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = null } };

        new MailtoLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsNotCustomNode_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = new Text() } };

        new MailtoLinkRenderer().SupportsContent(content).Should().BeFalse();
    }
    
    [TestMethod]
    public void SupportsContent_DataTargetIsEmptyCustomNode_NotSupported()
    {
        var content = new EntryStructure
                      {
                          Data = new EntryStructureData
                                 {
                                     Target = new CustomNode()
                                 }
                      };

        new MailtoLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsCustomNodeWithJObject_OtherId_NotSupported()
    {
        var mailtoLink = new MailtoLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "not a mailto link ID"
                                                                          }
                                                   }
                                 }
                             };

        var jObject = JObject.FromObject(mailtoLink);

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

        new MailtoLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsCustomNodeWithJObject_IsSupported()
    {
        var mailtoLink = new MailtoLink
                         {
                             Sys =
                             {
                                 ContentType = new ContentType
                                               {
                                                   SystemProperties = new SystemProperties
                                                                      {
                                                                          Id = "mailtoLink"
                                                                      }
                                               }
                             }
                         };

        var jObject = JObject.FromObject(mailtoLink);

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

        new MailtoLinkRenderer().SupportsContent(content).Should().BeTrue();
    }
    
    [TestMethod]
    public async Task RenderAsync_RendersATagWithMailto()
    {
        var mailtoLink = new MailtoLink()
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "mailtoLink"
                                                                          }
                                                   }
                                 },
                                 Text = "Some Text",
                                 Email = "Some Email"
                             };

        var jObject = JObject.FromObject(mailtoLink);

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
        
        var renderer = new MailtoLinkRenderer();
        var result = await renderer.RenderAsync(content);

        result.Should().Be($"<a class='govuk-link' href='mailto:Some Email'>Some Text</a>");
    }
}