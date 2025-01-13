using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RedirectIfDateMissingAttribute() : TypeFilterAttribute(typeof(RedirectIfDateMissingFilterAttribute))
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RedirectIfDateMissingFilterAttribute(IUserJourneyCookieService userJourneyCookieService)
        : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();

            if (startMonth == null || startYear == null)
            {
                userJourneyCookieService.SetWhereWasQualificationAwarded("england");
                context.Result = new RedirectResult("/questions/when-was-the-qualification-started");
            }
        }
    }
}