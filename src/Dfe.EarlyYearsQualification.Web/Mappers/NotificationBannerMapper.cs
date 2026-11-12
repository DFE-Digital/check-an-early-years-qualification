using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class NotificationBannerMapper
{
    public static NotificationBannerModel? Map(NotificationBanner? notificationBanner, string body, string id,
                                               bool fullWidth = false)
    {
        if (notificationBanner is null) return null;

        return new NotificationBannerModel
               {
                   Id = id,
                   Heading = notificationBanner.Heading,
                   Body = body,
                   FullWidth = fullWidth
               };
    }
}
