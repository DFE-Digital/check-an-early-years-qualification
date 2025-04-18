using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class NestedContentHelperTests
{
    [TestMethod]
    public void ContentHelper_RenderHeading1()
    {
        var heading = new Heading1 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h1 class='govuk-heading-l'>Text</h1>");
    }

    [TestMethod]
    public void ContentHelper_RenderHeading2()
    {
        var heading = new Heading2 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h2 class='govuk-heading-l'>Text</h2>");
    }

    [TestMethod]
    public void ContentHelper_RenderHeading3()
    {
        var heading = new Heading3 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h3 class='govuk-heading-m'>Text</h3>");
    }

    [TestMethod]
    public void ContentHelper_RenderHeading4()
    {
        var heading = new Heading4 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h4 class='govuk-heading-s'>Text</h4>");
    }

    [TestMethod]
    public void ContentHelper_RenderHeading5()
    {
        var heading = new Heading5 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h5 class='govuk-heading-m'>Text</h5>");
    }

    [TestMethod]
    public void ContentHelper_RenderHeading6()
    {
        var heading = new Heading6 { Content = [] };

        var text = new Text { Value = "Text" };

        heading.Content.Add(text);

        var content = new List<IContent> { heading };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<h6 class='govuk-heading-m'>Text</h6>");
    }

    [TestMethod]
    public void ContentHelper_RenderParagraph()
    {
        var paragraph = new Paragraph { Content = [] };

        var text = new Text { Value = "Text" };

        paragraph.Content.Add(text);

        var content = new List<IContent> { paragraph };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<p class=\"govuk-body\">Text</p>");
    }

    [TestMethod]
    public void ContentHelper_RenderHyperlink()
    {
        var hyperlink = new Hyperlink
                        {
                            Content = [],
                            Data = new HyperlinkData
                                   {
                                       Uri = "https://my.uri"
                                   }
                        };

        var text = new Text { Value = "Hyperlink Content" };

        hyperlink.Content.Add(text);

        var content = new List<IContent> { hyperlink };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<a href='https://my.uri' class='govuk-link'>Hyperlink Content</a>");
    }

    [TestMethod]
    public void ContentHelper_RenderList()
    {
        var paragraph = new Paragraph { Content = [new Text { Value = "Test list item" }] };

        var list = new List
                   {
                       Content = [new ListItem { Content = [paragraph] }]
                   };

        var content = new List<IContent> { list };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<ul class=\"govuk-list govuk-list--bullet\"><li>Test list item</li></ul>");
    }

    [TestMethod]
    public void ContentHelper_RenderText()
    {
        var text = new Text { Value = "Some text" };

        var content = new List<IContent> { text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("Some text");
    }

    [TestMethod]
    public void ContentHelper_RenderBoldText()
    {
        var text = new Text
                   {
                       Value = "Some text", Marks =
                       [
                           new Mark
                           {
                               Type = "bold"
                           }
                       ]
                   };

        var content = new List<IContent> { text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<b>Some text</b>");
    }

    [TestMethod]
    public void ContentHelper_RenderCollection()
    {
        var paragraph = new Paragraph { Content = [] };

        var paraText = new Text { Value = "Text" };

        paragraph.Content.Add(paraText);

        var hyperlink = new Hyperlink
                        {
                            Content = [],
                            Data = new HyperlinkData
                                   {
                                       Uri = "https://my.uri"
                                   }
                        };

        var hlText = new Text { Value = "Hyperlink Content" };

        hyperlink.Content.Add(hlText);

        var text = new Text { Value = "Some text" };

        var content = new List<IContent> { paragraph, hyperlink, text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should()
              .Be("<p class=\"govuk-body\">Text</p><a href='https://my.uri' class='govuk-link'>Hyperlink Content</a>Some text");
    }

    [TestMethod]
    public void ContentHelper_RenderExternalNavigationLink()
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

        var entryStructure = new EntryStructure
                             {
                                 Data = new EntryStructureData
                                        {
                                            Target = customNode
                                        }
                             };

        var content = new List<IContent> { entryStructure };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<a href='/' target='_blank' class='govuk-link'>Display text</a>");
    }

    [TestMethod]
    public void ContentHelper_RenderMailtoLink()
    {
        var navigationLink = new MailtoLink
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
                                 Text = "Display Text",
                                 Email = "Some Email"
                             };

        var jObject = JObject.FromObject(navigationLink);

        var customNode = new CustomNode
                         {
                             JObject = jObject
                         };

        var entryStructure = new EntryStructure
                             {
                                 Data = new EntryStructureData
                                        {
                                            Target = customNode
                                        }
                             };

        var content = new List<IContent> { entryStructure };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<a class='govuk-link' href='mailto:Some Email'>Display Text</a>");
    }
}