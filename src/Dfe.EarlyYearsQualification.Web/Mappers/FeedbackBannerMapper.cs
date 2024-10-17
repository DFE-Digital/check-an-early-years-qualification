using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackBannerMapper
{
    public static FeedbackBannerModel? Map(FeedbackBanner? feedbackBanner, string? bodyHtml)
    {
        if (feedbackBanner is null || bodyHtml is null) return null;
        return new FeedbackBannerModel
               {
                   Heading = feedbackBanner.Heading,
                   Body = bodyHtml,
                   BannerTitle = feedbackBanner.BannerTitle
               };
    }
}