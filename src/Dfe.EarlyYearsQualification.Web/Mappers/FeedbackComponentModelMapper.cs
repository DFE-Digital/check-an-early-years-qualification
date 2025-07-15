using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackComponentModelMapper
{
    public static FeedbackComponentModel Map(string header, string body)
    {
        return new FeedbackComponentModel
               {
                   Header = header,
                   Body = body
               };
    }
}