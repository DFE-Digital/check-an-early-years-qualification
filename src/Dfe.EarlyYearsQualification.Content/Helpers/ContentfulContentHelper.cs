using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Newtonsoft.Json.Linq;

namespace Dfe.EarlyYearsQualification.Content.Helpers;

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

    public static Document ParagraphWithBold(string text)
    {
        return new Document
               {
                   Content =
                   [
                       new Paragraph
                       {
                           Content =
                           [
                               new Text
                               {
                                   Value = text, Marks =
                                   [
                                       new Mark
                                       {
                                           Type = "bold"
                                       }
                                   ]
                               }
                           ]
                       }
                   ]
               };
    }

    public static Paragraph ParagraphWithEmbeddedLink(string text, string linkText, string linkHref)
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

        return new Paragraph
               {
                   Content =
                   [
                       new Text { Value = text },
                       externalNavigationLink
                   ]
               };
    }
}