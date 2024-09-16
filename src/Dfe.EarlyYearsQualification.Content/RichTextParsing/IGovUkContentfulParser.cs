using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing;

public interface IGovUkContentfulParser
{
    Task<string> ToHtml(Document? content);
}