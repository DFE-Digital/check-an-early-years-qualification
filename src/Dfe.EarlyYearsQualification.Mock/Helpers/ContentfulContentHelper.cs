using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Mock.Helpers;

public static class ContentfulContentHelper
{
    public static Document Text(string text)
    {
        return new Document { Content = [new Text { Value = text }] };
    }

    public static Document Paragraph(string text)
    {
        return new Document
               {
                   Content =
                   [
                       new Paragraph
                       {
                           Content =
                           [
                               new Text { Value = text }
                           ]
                       }
                   ]
               };
    }

    public static Document ParagraphThenUnorderedList(string paragraphText, params string[] listItems)
    {
        return new Document
               {
                   Content =
                   [
                       new Paragraph { Content = [new Text { Value = paragraphText }] },
                       new List
                       {
                           NodeType = "unordered-list",
                           Content = listItems.Select(text => (IContent)new ListItem
                                                      {
                                                          Content =
                                                          [
                                                              new Paragraph { Content = [new Text { Value = text }] }
                                                          ]
                                                      })
                                              .ToList()
                       }
                   ]
               };
    }

    public static Document Link(string text, string href)
    {
        return new Document
               {
                   Content =
                   [
                       new Hyperlink
                       { Data = new HyperlinkData { Uri = href }, Content = [new Text { Value = text }] }
                   ]
               };
    }

    public static Paragraph ParagraphWithEmbeddedLink(string text, string linkText, string linkHref,
                                                       string textAfterLink = "")
    {
        var navigationLink = new NavigationLink
                             {
                                 Sys =
                                 {
                                     ContentType = new ContentType
                                                   {
                                                       SystemProperties = new SystemProperties
                                                                          {
                                                                              Id = "navigationLink"
                                                                          }
                                                   }
                                 },
                                 DisplayText = linkText,
                                 Href = linkHref
                             };

        var jObject = JObject.FromObject(navigationLink);

        var customNode = new CustomNode
                         {
                             JObject = jObject
                         };

        var externalNavigationLink = new EntryStructure
                                     {
                                         Data = new EntryStructureData
                                                {
                                                    Target = customNode
                                                }
                                     };

        List<IContent> content =
        [
            new Text { Value = text },
            externalNavigationLink
        ];

        if (!string.IsNullOrEmpty(textAfterLink))
        {
            content.Add(new Text { Value = textAfterLink });
        }

        return new Paragraph
               {
                   Content = content
               };
    }
}