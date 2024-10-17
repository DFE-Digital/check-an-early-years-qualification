using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Base;

/// <summary>
///     Controller class that is guarded by a <see cref="IChallengeResourceFilterAttribute" />
///     so that it is possible, while in private beta and in non-production environments,
///     to configure a secret that must be entered to gain access to the service.
///     All controllers except <see cref="ChallengeController" />, <see cref="ErrorController" />
///     and <see cref="HealthController" /> should derive from this type.
/// </summary>
[ServiceFilter<IChallengeResourceFilterAttribute>]
public class ServiceController : Controller
{
    protected static NavigationLinkModel? MapToNavigationLinkModel(NavigationLink? navigationLink)
    {
        if (navigationLink is null) return null;
        
        return new NavigationLinkModel
               {
                   Href = navigationLink.Href,
                   DisplayText = navigationLink.DisplayText,
                   OpenInNewTab = navigationLink.OpenInNewTab
               };
    }

    protected static async Task<FeedbackBannerModel?> MapToFeedbackBannerModel(
        FeedbackBanner? feedbackBanner, IGovUkContentParser contentParser)
    {
        if (feedbackBanner is null) return null;
        return new FeedbackBannerModel
               {
                   Heading = feedbackBanner.Heading,
                   Body = await contentParser.ToHtml(feedbackBanner.Body),
                   BannerTitle = feedbackBanner.BannerTitle
               };
    }

    protected static async Task<string?> GetFeedbackBannerBodyToHtml(FeedbackBanner? feedbackBanner, IGovUkContentParser contentParser)
    {
        return feedbackBanner is not null
                                   ? await contentParser.ToHtml(feedbackBanner.Body)
                                   : null;
    }
}