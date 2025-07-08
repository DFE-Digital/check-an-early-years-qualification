using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackComponentMapper
{
    public static FeedbackComponentModel Map(string header, string body)
    {
        return new FeedbackComponentModel
               {
                   FeedbackHeader = header,
                   FeedbackBody = body
               };
    }
}