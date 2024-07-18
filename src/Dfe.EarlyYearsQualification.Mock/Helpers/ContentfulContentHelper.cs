using Contentful.Core.Models;

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
}