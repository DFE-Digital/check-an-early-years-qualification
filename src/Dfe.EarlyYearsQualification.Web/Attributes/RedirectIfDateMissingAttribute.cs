using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RedirectIfDateMissingAttribute() : TypeFilterAttribute(typeof(RedirectIfDateMissingFilter))
{
    public class RedirectIfDateMissingFilter(IUserJourneyCookieService userJourneyCookieService) : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cookie = userJourneyCookieService.GetUserJourneyModelFromCookie();

            if (string.IsNullOrEmpty(cookie.WhenWasQualificationAwarded))
            {
                context.Result = new RedirectResult("/questions/when-was-the-qualification-started");
            }
        }
    }
}

