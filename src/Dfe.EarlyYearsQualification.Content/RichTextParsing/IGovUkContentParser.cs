using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing;

public interface IGovUkContentParser
{
    Task<string> ToHtml(Document? content);
}