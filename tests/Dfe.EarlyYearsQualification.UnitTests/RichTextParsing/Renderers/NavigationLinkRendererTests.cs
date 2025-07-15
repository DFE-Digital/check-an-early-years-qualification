using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class NavigationLinkRendererTests
{
    [TestMethod]
    public void SupportsContent_IsNotEntryStructure_ReturnsFalse()
    {
        var content = new Paragraph();
        new NavigationLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsNull_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = null } };

        new NavigationLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsNotCustomNode_DoesNotSupport()
    {
        var content = new EntryStructure { Data = new EntryStructureData { Target = new Text() } };

        new NavigationLinkRenderer().SupportsContent(content).Should().BeFalse();
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

        new NavigationLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsCustomNodeWithJObject_OtherId_NotSupported()
    {
        var navigationLink = new NavigationLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "externalNavigationLink123"
                                                                          }
                                                   }
                                 }
                             };

        var jObject = JObject.FromObject(navigationLink);

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

        new NavigationLinkRenderer().SupportsContent(content).Should().BeFalse();
    }

    [TestMethod]
    public void SupportsContent_DataTargetIsCustomNodeWithJObject_IsSupported()
    {
        var navigationLink = new NavigationLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "externalNavigationLink"
                                                                          }
                                                   }
                                 }
                             };

        var jObject = JObject.FromObject(navigationLink);

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

        new NavigationLinkRenderer().SupportsContent(content).Should().BeTrue();
    }

    [TestMethod]
    public async Task RenderAsync_IncludesTarget()
    {
        var navigationLink = new NavigationLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "externalNavigationLink"
                                                                          }
                                                   }
                                 },
                                 Href = "/",
                                 DisplayText = "Display text",
                                 OpenInNewTab = true
                             };

        var jObject = JObject.FromObject(navigationLink);

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

        var renderer = new NavigationLinkRenderer();
        var result = await renderer.RenderAsync(content);

        result.Should().Be("<a href='/' target='_blank' class='govuk-link'>Display text</a>");
    }

    [TestMethod]
    public async Task RenderAsync_ExcludesTarget()
    {
        var navigationLink = new NavigationLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "externalNavigationLink"
                                                                          }
                                                   }
                                 },
                                 Href = "/",
                                 DisplayText = "Display text",
                                 OpenInNewTab = false
                             };

        var jObject = JObject.FromObject(navigationLink);

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

        var renderer = new NavigationLinkRenderer();
        var result = await renderer.RenderAsync(content);

        result.Should().Be("<a href='/' class='govuk-link'>Display text</a>");
    }
}